using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DataAccesLogic.Boundaries;
using DataAccesLogic.Drivers;
using Domain.DTOModels;
using Domain.Models;
using Interfaces;
using MathNet.Numerics; //lineær regressions pakke

namespace BusinessLogic.Controller
{
    public class CalibrationLogic
    {
        private static ButtonObserver _button1Observer;
        private static ButtonObserver _button2Observer;
        private static ButtonObserver _button3Observer;
        private static ButtonObserver _button4Observer;
        private static DisplayDriver lcd;
        private BlockingCollection<LCD_DTO> _dataQueueLCD;
        public Measure _measure { get; private set; }
        public ManualResetEvent _calibrationEventMeasure { get; set; }
        public ManualResetEvent _calibrationEventLcd { get; set; }
        public ManualResetEvent _calibrationEventLocalDb { get; set; }
        public BlockingCollection<Measure_DTO> _dataQueueMeasure { get; set; }
        public List<int> TestPressureList { get; private set; }
        private double[] ydata;
        private double[] xdata;
        private ManualResetEvent _calibrationJoinEvent;


        public CalibrationLogic(ButtonObserver buttonObserver1, ButtonObserver buttonObserver2,
            ButtonObserver buttonObserver3, ButtonObserver buttonObserver4, BlockingCollection<LCD_DTO> dataQueue,
            ManualResetEvent calibrationEventLcd, ManualResetEvent calibrationEventMeasure,
            ManualResetEvent calibrationEventLocalDb, ManualResetEvent calibrationJoinEvent,
            BlockingCollection<Measure_DTO> dataQueueMeasure, Measure measure,
            DisplayDriver displayDriver)
        {
            _button1Observer = buttonObserver1;
            _button2Observer = buttonObserver2;
            _button3Observer = buttonObserver3;
            _button4Observer = buttonObserver4;
            TestPressureList = new List<int>();
            TestPressureList.AddRange(new List<int>{10, 50,100,150,200,250,300}); //,100,150,200,250,300
            xdata = new double[] { 10, 50, 100, 150, 200, 250, 300}; //, 100, 150, 200, 250, 300 
            ydata = new double[xdata.Length];

            _dataQueueLCD = dataQueue;
            _dataQueueMeasure = dataQueueMeasure;

            _calibrationEventMeasure = calibrationEventMeasure;
            _calibrationEventLcd = calibrationEventLcd;
            _calibrationEventLocalDb = calibrationEventLocalDb;
            _calibrationJoinEvent = calibrationJoinEvent;
            _measure = measure;
            //lcd = displayDriver;


        }

       

        public void Run()
        {

            while (true)
            {
                if (_button3Observer.startCal)
                {
                    _calibrationJoinEvent.Set();
                    _calibrationEventMeasure.Set();
                    _calibrationEventLocalDb.Set();
                    _calibrationEventLcd.Set();
                    while (_dataQueueLCD.Count != 0)
                    {
                        Thread.Sleep(1);
                    }
                    
                    Calibrate();
                    _button3Observer.startCal = false;
                    _calibrationEventMeasure.Reset();
                    _calibrationEventLcd.Reset();
                    _calibrationEventLocalDb.Reset();
                    _calibrationJoinEvent.Reset();
                }

                Thread.Sleep(0);
            }
        }


        public void Calibrate()
        {
            ClearQueueMeasure(_dataQueueMeasure);
            ClearQueueDisplay(_dataQueueLCD);
            bool breakFlag = false;

            DisplayDriver lcd = new DisplayDriver();
            lcd.lcdClear();
            lcd.lcdGotoXY(0,0);
            lcd.lcdPrint("Getting ready for   calibration...");

                while (!_button4Observer.IsPressed || !breakFlag)
                {
                    lcd.lcdClear();
                    lcd.lcdPrint("123456789 123456789 123456789 123456789");
                    //lcd.lcdPrint("Calibration initialized\nPlease press \"START\" to continue\n or press"); // \"Mute\" to stop calibration

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
                        lcd.lcdClear();
                        lcd.lcdPrint("Calibration have been stopped. Nothing applied.");
                        break;
                    }

                    lcd.lcdClear();
                    lcd.lcdPrint($"Calibration started.");

                    int j = 0;
                    foreach (var pressure in TestPressureList)
                    {
                        lcd.lcdClear();
                        lcd.lcdPrint($"Please apply a test pressure of {pressure} and press \"Start\"");
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
                            lcd.lcdClear();
                            lcd.lcdPrint("Calibration have been stopped. Nothing applied.");
                            break;
                        }

                        lcd.lcdClear();
                        lcd.lcdPrint("Measuring. Please wait ...");

                        
                        List<double> lsTestPressure = new List<double>();
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
                                    lsTestPressure.Add((int) sample.Value/_measure.ConvertingFactor);
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
                            
                        

                        //Gnms. gemmes på i's plads i ydata-arrayet


                    }


                    Tuple<double, double> p = Fit.Line(xdata, ydata);
                    double b = p.Item1;
                    double a = p.Item2;
                    //a = a * _measure.ConvertingFactor;
                    //b = b * _measure.ConvertingFactor;
                    lcd.lcdClear();
                    lcd.lcdPrint($"Original converting factor is: {_measure.ConvertingFactor}" +
                                 $"\nAdjustment suggestion: {_measure.ConvertingFactor - a}" +
                                 $"\nPress \"START\" to apply or \"MUTE\" to discard");
                    bool discardLock = false;
                    _calibrationEventMeasure.Reset();
                    while (!discardLock)
                    {
                        if (_button2Observer.IsPressed)
                        {
                            _measure.ConvertingFactor = a;
                            lcd.lcdClear();
                            lcd.lcdPrint("Calibration applied. ");
                            discardLock = true;
                            break;
                        }
                        else if (_button4Observer.IsPressed)
                        {
                            lcd.lcdClear();
                            lcd.lcdPrint("Calibration discarded.");
                            discardLock = true;
                            break;
                        }

                    }
                    //Close the while loop
                    if (discardLock)
                    {
                        break;
                    }

                    //_dataQueueMeasure.Take();
                }
            

        }

        


        public void ClearQueueMeasure(BlockingCollection<Measure_DTO> blockingCollection)
        {
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