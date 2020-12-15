using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using DataAccesLogic.Boundaries;
using Domain.DTOModels;
using MathNet.Numerics; //lineær regressions pakke

namespace BusinessLogic.Controller
{
    public class CalibrationLogic
    {
        private static ButtonObserver _button1Observer;
        private static ButtonObserver _button2Observer;
        private static ButtonObserver _button3Observer;
        private static ButtonObserver _button4Observer;

        private BlockingCollection<LCD_DTO> _dataQueueLCD;
        private BlockingCollection<LCD_DTO> _dataQueueCalibrationLCD;
        public BlockingCollection<Measure_DTO> _dataQueueMeasure;
        private BlockingCollection<Adjustments_DTO> _dataQueueAdjustments;
        
        public ManualResetEvent _calibrationEventMeasure { get; set; }
        public ManualResetEvent _calibrationEventLcd { get; set; }
        public ManualResetEvent _calibrationEventLocalDb { get; set; }
        private ManualResetEvent _calibrationJoinEvent;
       
        public List<int> TestPressureList { get; private set; }
        private double[] ydata;
        private double[] xdata;
        private double _convertingFactor;

        private bool stop = false;

        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }


        public CalibrationLogic(ButtonObserver buttonObserver1, ButtonObserver buttonObserver2,
            ButtonObserver buttonObserver3, ButtonObserver buttonObserver4, BlockingCollection<LCD_DTO> dataQueue,
            ManualResetEvent calibrationEventLcd, ManualResetEvent calibrationEventMeasure,
            ManualResetEvent calibrationEventLocalDb, ManualResetEvent calibrationJoinEvent,
            BlockingCollection<Measure_DTO> dataQueueMeasure, BlockingCollection<Adjustments_DTO> dataQueueAdjustments,
            double convertingFactor)
        {
            _button1Observer = buttonObserver1;
            _button2Observer = buttonObserver2;
            _button3Observer = buttonObserver3;
            _button4Observer = buttonObserver4;
            TestPressureList = new List<int>();
            TestPressureList.AddRange(new List<int>{10, 50,100,150,200,250,300}); 
            xdata = new double[] { 10, 50, 100, 150, 200, 250, 300}; 
            ydata = new double[xdata.Length];
            _convertingFactor = convertingFactor;

            _dataQueueLCD = dataQueue;
            _dataQueueMeasure = dataQueueMeasure;
            _dataQueueAdjustments = dataQueueAdjustments;

            _calibrationEventMeasure = calibrationEventMeasure;
            _calibrationEventLcd = calibrationEventLcd;
            _calibrationEventLocalDb = calibrationEventLocalDb;
            _calibrationJoinEvent = calibrationJoinEvent;
            
           


        }

       

        public void Run()
        {

            while (!Stop)
            {
                if (_button3Observer.startCal)
                {
                    _calibrationJoinEvent.Reset();
                    _calibrationEventMeasure.Set();
                    _calibrationEventLocalDb.Set();
                    _calibrationEventLcd.Set();
                    while (_dataQueueLCD.Count != 0)
                    {
                        ClearQueueDisplay(_dataQueueLCD);
                        Thread.Sleep(1);
                    }
                    
                    Calibrate();
                    _button3Observer.startCal = false;
                    _calibrationEventMeasure.Reset();
                    _calibrationEventLcd.Reset();
                    _calibrationEventLocalDb.Reset();
                    _calibrationJoinEvent.Set();
                }

                Thread.Sleep(0);
            }
        }


        public void Calibrate()
        {
            ClearQueueMeasure(_dataQueueMeasure);
            ClearQueueDisplay(_dataQueueLCD);
            bool breakFlag = false;

         
                _dataQueueLCD.Add(new LCD_DTO() { Message = "Getting ready for   calibration..."});
                Debug.WriteLine("Getting ready for   calibration...");

                Thread.Sleep(1000);
            
            

            while (!_button4Observer.IsPressed || !breakFlag)
            {
                _dataQueueLCD.Add(new LCD_DTO()
                    {
                        Message =
                            "Calibration initialized\nPlease press \"START\" to continue\n or press \"Mute\" to stop calibration"
                    });
                Debug.WriteLine("Calibration initialized\nPlease press \"START\" to continue\n or press \"Mute\" to stop calibration");

                while (!_button2Observer.IsPressed)
                {
                    Thread.Sleep(0);
                    if (_button4Observer.IsPressed)
                    {
                        breakFlag = true;
                        break;
                        //buttonObserver sættes til true igen når den slippes, så den hopper ikke ud af den org. løkke, der bruges et flag
                    }
                }

                if (breakFlag)
                {
                    
                        _dataQueueLCD.Add(new LCD_DTO()
                        {
                            Message =
                                "Calibration have been stopped. Nothing applied."
                        });
                    Debug.WriteLine("Calibration have been stopped. Nothing applied.");

                        
                    break;
                }

          
                _dataQueueLCD.Add(new LCD_DTO()
                {
                    Message =
                        "Calibration started."
                });
                Debug.WriteLine("Calibration started.");
                

                int j = 0;
                foreach (var pressure in TestPressureList)
                {
                        _dataQueueLCD.Add(new LCD_DTO()
                        {
                            Message =
                                $"Please apply a test pressure of {pressure} and press \"Start\""
                        });
                        Debug.WriteLine($"Please apply a test pressure of {pressure} and press \"Start\"");
                    //Så længe der ikke er trykket på knap 2, skal den vente.
                    while (!_button2Observer.IsPressed)
                    {
                        Thread.Sleep(0);
                        if (_button4Observer.IsPressed)
                        {
                            breakFlag = true;
                            break;
                                
                        }
                    }

                    if (breakFlag)
                    {
                       
                            _dataQueueLCD.Add(new LCD_DTO()
                            {
                                Message =
                                    "Calibration have been stopped. Nothing applied."
                            });
                            Debug.WriteLine("Calibration have been stopped. Nothing applied.");
                        
                        break;
                    }
                    _dataQueueLCD.Add(new LCD_DTO()
                        {
                            Message =
                                "Measuring. Please wait ..."
                        });
                    Debug.WriteLine("Measuring. Please wait ...");
                    

                        
                    List<double> lsTestPressure = new List<double>();
                    for (int i = 0; i < 3;)
                    {
                        ClearQueueMeasure(_dataQueueMeasure);
                        
                        _calibrationEventMeasure.Reset();

                        while (_dataQueueMeasure.Count == 0)
                        {
                            Thread.Sleep(10);
                        }
                        _calibrationEventMeasure.Set();

                        if (_dataQueueMeasure.Count >= 1)
                        {
                            var container = _dataQueueMeasure.Take();
                            //Gemme de tre containers for første testtryk i liste
                            foreach (var sample in container.SamplePack.SampleList)
                            {
                                lsTestPressure.Add((int) sample.Value/_convertingFactor);
                            }
                            ClearQueueMeasure(_dataQueueMeasure);
                            i++;
                        }
                        else
                        {
                            ClearQueueMeasure(_dataQueueMeasure);
                        }
                    }

                    //Der tages et gnms. af værdierne i listen med testtryk
                    if (lsTestPressure.Count != 0)
                    {
                        var result = lsTestPressure.Average();
                        ydata[j] = result;
                        j++;
                    }
                }


                Tuple<double, double> p = Fit.Line(xdata, ydata);
                double b = p.Item1;
                double a = p.Item2;


                _dataQueueLCD.Add(new LCD_DTO()
                    {
                        Message =
                            $"Original converting factor is: {_convertingFactor}" +
                            $"\nAdjustment suggestion: {_convertingFactor - a}" +
                            $"\nPress \"START\" to apply or \"MUTE\" to discard"
                    });
                
                    Debug.WriteLine($"Original converting factor is: {_convertingFactor}" +
                                    $"\nAdjustment suggestion: {_convertingFactor - a}" +
                                    $"\nPress \"START\" to apply or \"MUTE\" to discard");
                bool discardLock = false;
                _calibrationEventMeasure.Reset();
                while (!discardLock)
                {
                    if (_button2Observer.IsPressed)
                    {
                        Adjustments_DTO DTO = new Adjustments_DTO();
                        DTO.Calibration = a;
                        _dataQueueAdjustments.Add(DTO);
                       
                        _dataQueueLCD.Add(new LCD_DTO()
                        {
                            Message =
                                "Calibration applied. Press Start to Continue or power off"
                        });
                        Debug.WriteLine("Calibration applied. Press Start to Continue or power off");
                        discardLock = true;
                        break;
                    }
                    else if (_button4Observer.IsPressed)
                    {
                        _dataQueueLCD.Add(new LCD_DTO()
                        {
                            Message =
                                "Calibration discarded. Press Start to Continue or power off"
                        });
                        Debug.WriteLine("Calibration discarded. Press Start to Continue or power off");
                        discardLock = true;
                        break;
                    }

                }
                //Close the while loop
                if (discardLock)
                {
                    break;
                }
            }
            

        }

        


        public void ClearQueueMeasure(BlockingCollection<Measure_DTO> blockingCollection)
        {
            //Made to be sure that only testpressure measurements are left in queue
            //For at fully working system, it wouldn't be smart to clear and delete measured data, but because our system is working quite slowly
            //it was necessary to secure a better calibration.
            while (blockingCollection.Count > 0)
            {
                blockingCollection.TryTake(out _);
            }
        }

        public void ClearQueueDisplay(BlockingCollection<LCD_DTO> blockingCollection)
        {
            while (blockingCollection.Count > 0)
            {
                blockingCollection.TryTake(out _);
            }
        }
    }
}