using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using DataAccesLogic.Boundaries;
using DataAccesLogic.Drivers;
using Domain.DTOModels;
using Domain.Models;
using Interfaces;

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
        public ManualResetEvent _calibrationEventMeasure { get; set; }
        public ManualResetEvent _calibrationEventLcd { get; set; }
        public ManualResetEvent _calibrationEventLocalDb { get; set; }
        public BlockingCollection<Measure_DTO> _dataQueueMeasure { get; set; }
        public List<int> TestPressureList { get; private set; }


        public CalibrationLogic(ButtonObserver buttonObserver1, ButtonObserver buttonObserver2,
            ButtonObserver buttonObserver3, ButtonObserver buttonObserver4, BlockingCollection<LCD_DTO> dataQueue,
            ManualResetEvent calibrationEventLcd, ManualResetEvent calibrationEventMeasure, 
            ManualResetEvent calibrationEventLocalDb, BlockingCollection<Measure_DTO> dataQueueMeasure)
        {
            _button1Observer = buttonObserver1;
            _button2Observer = buttonObserver2;
            _button3Observer = buttonObserver3;
            _button4Observer = buttonObserver4;
            lcd = new DisplayDriver();

            _dataQueueLCD = dataQueue;
            _dataQueueMeasure = dataQueueMeasure;

            _calibrationEventMeasure = calibrationEventMeasure;
            _calibrationEventLcd = calibrationEventLcd;
            _calibrationEventLocalDb = calibrationEventLocalDb;

            
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
                    lcd.lcdPrint("Getting ready for calibration...");
                    Calibrate();
                    _calibrationEventMeasure.Reset();
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
                lcd.lcdPrint("Calibration initialized\nPlease press \"START\" to continue\n or press \"Mute\" to stop calibration");
                if (_button2Observer.IsPressed)
                {
                    foreach (var pressure in TestPressureList)
                    {
                        
                        lcd.lcdPrint($"Calibration started.\nPlease apply a test pressure of {pressure} and press \"Start\"");
                        if (_button2Observer.IsPressed)
                        {
                            lcd.lcdPrint("Measuring. Please wait ...");

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