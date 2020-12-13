using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using DataAccesLogic.Boundaries;
using DataAccesLogic.Drivers;
using Domain.DTOModels;

namespace BusinessLogic.Controller
{
    public class StartUp
    {
       private BlockingCollection<Measure_DTO> _dataQueueMeasure;
        private BlockingCollection<Adjustments_DTO> _dataQueueAdjustments;
        private BlockingCollection<LCD_DTO> _dataQueueLCD;
        private ButtonObserver _button1Observer;
        private ButtonObserver _button2Observer;
        private ButtonObserver _button3Observer;
        private ButtonObserver _button4Observer;


        private ManualResetEvent _calibrationEventMeasure;
        //private bool stop=false;

        //public bool Stop
        //{
        //    get { return stop; }
        //    set { stop = value; }
        //}

        public StartUp(BlockingCollection<Measure_DTO> dataQueueMeasure,
            BlockingCollection<Adjustments_DTO> dataQueueAdjustments, ButtonObserver buttonObserver1,
            ButtonObserver buttonObserver2,
            ButtonObserver buttonObserver3, ButtonObserver buttonObserver4, ManualResetEvent calibrationEventMeasure,
            BlockingCollection<LCD_DTO> dataQueueLcd)
        {
            
            _dataQueueMeasure = dataQueueMeasure;
            _dataQueueAdjustments = dataQueueAdjustments;
            _button1Observer = buttonObserver1;
            _button2Observer = buttonObserver2;
            _button3Observer = buttonObserver3;
            _button4Observer = buttonObserver4;
            _calibrationEventMeasure = calibrationEventMeasure;
            _dataQueueLCD = dataQueueLcd;

        }
        public void Run()
        {
            Start:
            
                _dataQueueLCD.Add(new LCD_DTO()
                {
                    Message =
                        "Welcome. Please press Prepare til proceed."
                });
                Debug.WriteLine("Welcome. Please press Prepare til proceed.");
            

            while (!_button1Observer.IsPressed)
            {
                Thread.Sleep(0);
            }

       
               
                _dataQueueLCD.Add(new LCD_DTO()
                {
                    Message =
                        "Please position the PVC tap as shown in the manual and press Prepare to initialize the device"
                });
                Debug.WriteLine("Please position the PVC tap as shown in the manual and press Prepare to initialize the device");
            
            while (!_button1Observer.IsPressed)
            {
                Thread.Sleep(0);
            }
     
                
                _dataQueueLCD.Add(new LCD_DTO()
                {
                    Message =
                        "Measuring...        Please don't move PVC tap.."
                });
                Debug.WriteLine("Measuring...        Please don't move PVC tap..");
                if (MeasureZeroPoint()< 650 || MeasureZeroPoint()> 800)//Lowest and highest recorded airpressure in mmHg soucre:
                                                                   //https://sciencing.com/understand-barometric-pressure-readings-5397464.html
            {
                
                    _dataQueueLCD.Add(new LCD_DTO()
                    {
                        Message =
                            "The measured pressure was not as expected. Please make sure the position of the PVC tap is correct, and try again."
                    });

                    Debug.WriteLine("The measured pressure was not as expected. Please make sure the position of the PVC tap is correct, and try again.");
                
                goto Start;
            }
            else
            {
                Adjustments_DTO DTO = new Adjustments_DTO();
                DTO.ZeroPoint = MeasureZeroPoint();
                _dataQueueAdjustments.Add(DTO);
        
                    _dataQueueLCD.Add(new LCD_DTO()
                    {
                        Message =
                            "The device have been initialized. Press Start to start measuring"
                    });
                    Debug.WriteLine("The device have been initialized. Press Start to start measuring");
               
            }


        }

        double MeasureZeroPoint()
        {
            List<int> zeroPointMeasures = new List<int>();
            ClearQueueMeasure(_dataQueueMeasure);
            for (int i = 0; i < 3;)
            {
                _calibrationEventMeasure.Reset();

                while (_dataQueueMeasure.Count == 0)
                {
                    Thread.Sleep(0);
                }
                _calibrationEventMeasure.Set();

                if (_dataQueueMeasure.Count >= 1)
                {
                    var container = _dataQueueMeasure.Take();
                    //Gemme de tre containers for første testtryk i liste
                    foreach (var sample in container.SamplePack.SampleList)
                    {
                        zeroPointMeasures.Add(sample.Value);
                    }
                    ClearQueueMeasure(_dataQueueMeasure);
                    i++;
                }
                else
                {
                    ClearQueueMeasure(_dataQueueMeasure);
                }
            }
            
            return zeroPointMeasures.Average();
        }



        public void ClearQueueMeasure(BlockingCollection<Measure_DTO> blockingCollection)
        {
            while (blockingCollection.Count > 0)
            {
                blockingCollection.TryTake(out _);
            }
        }
    }
}