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


        public CalibrationLogic(ButtonObserver buttonObserver1, ButtonObserver buttonObserver2,
            ButtonObserver buttonObserver3, ButtonObserver buttonObserver4, BlockingCollection<LCD_DTO> dataQueue,
            ManualResetEvent calibrationEventLcd, ManualResetEvent calibrationEventMeasure, 
            ManualResetEvent calibrationEventLocalDb, BlockingCollection<Measure_DTO> dataQueueMeasure, Measure measure)
        {
            _button1Observer = buttonObserver1;
            _button2Observer = buttonObserver2;
            _button3Observer = buttonObserver3;
            _button4Observer = buttonObserver4;
            lcd = new DisplayDriver();
            TestPressureList = new List<int>();
            TestPressureList.AddRange(new List<int>{10,50,100,150,200,250,300});
            xdata = new double[] { 10, 50, 100, 150, 200, 250, 300 };
            ydata = new double[xdata.Length];

            _dataQueueLCD = dataQueue;
            _dataQueueMeasure = dataQueueMeasure;

            _calibrationEventMeasure = calibrationEventMeasure;
            _calibrationEventLcd = calibrationEventLcd;
            _calibrationEventLocalDb = calibrationEventLocalDb;
            _measure = measure;


        }

       

        public void Run()
        {
            while (true)
            {
                if (_button3Observer.startCal)
                {
                    _calibrationEventMeasure.Set();
                    _calibrationEventLocalDb.Set();
                    _calibrationEventLcd.Set();
                    _button3Observer.startCal = false;
                    lcd.lcdClear();
                    lcd.lcdPrint("Getting ready for calibration...");
                    Calibrate();
                    _calibrationEventMeasure.Reset();
                    _calibrationEventLcd.Reset();
                    _calibrationEventLocalDb.Reset();
                }

                Thread.Sleep(20);
            }
        }


        public void Calibrate()
        {
            ClearQueueMeasure(_dataQueueMeasure);
            ClearQueueDisplay(_dataQueueLCD);
            while (!_button4Observer.IsPressed)
            {
                lcd.lcdClear();
                lcd.lcdDisplay();
                Thread.Sleep(0);
                lcd.lcdPrint("123456789 123456789 123456789");
                //lcd.lcdPrint("Calibration initialized\nPlease press \"START\" to continue\n or press"); // \"Mute\" to stop calibration

                if (_button2Observer.IsPressed)
                {
                    int j = 0;
                    foreach (var pressure in TestPressureList)
                    {

                        lcd.lcdPrint($"Calibration started."); //\nPlease apply a test pressure of {pressure} and press \"Start\""
                        if (_button2Observer.IsPressed)
                        {
                            lcd.lcdPrint("Measuring. Please wait ...");

                            _calibrationEventMeasure.Reset();
                            List<int> lsTestPressure = new List<int>();
                            for (int i = 0; i < 3; i++)
                            {
                                Thread.Sleep(1000);
                                if (_dataQueueMeasure != null)
                                {
                                    var container = _dataQueueMeasure.Take();
                                    //Gemme de tre containers for første testtryk i liste
                                    foreach (var sample in container.SamplePack.SampleList)
                                    {
                                        lsTestPressure.Add((int)sample.Value);
                                    }
                                    i++;
                                }
                            }

                            //Der tages et gnms. af værdierne i listen med testtryk
                            var result = lsTestPressure.Average();
                            //Gnms. gemmes på i's plads i ydata-arrayet
                            
                            ydata[j] = result;
                            j++;
                        }


                    }


                    Tuple<double, double> p = Fit.Line(xdata, ydata);
                    double b = p.Item1;
                    double a = p.Item2;
                    lcd.lcdPrint($"Original converting factor is: {_measure.ConvertingFactor}" +
                                 $"\nAdjustment suggestion: {_measure.ConvertingFactor-a}" +
                                 $"\nPress \"START\" to apply or \"MUTE\" to discard");
                    bool discardLock = false;
                    while(!_button2Observer.IsPressed && !discardLock)
                    {
                        if (_button2Observer.IsPressed)
                        {
                            _measure.ConvertingFactor = a;
                            lcd.lcdPrint("Calibration applied. ");
                            discardLock = true;
                        }
                        else if (_button4Observer.IsPressed)
                        {
                            lcd.lcdPrint("Calibration discarded.");
                            discardLock = true;
                        }
                        
                    }
                }

                _dataQueueMeasure.Take();
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