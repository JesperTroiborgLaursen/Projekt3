using System;
using System.Collections.Concurrent;
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
        public ManualResetEvent _calibrationEvent { get; set; }


        public CalibrationLogic(ButtonObserver buttonObserver1, ButtonObserver button2Observer,
            ButtonObserver buttonObserver3, ButtonObserver buttonObserver4, BlockingCollection<LCD_DTO> dataQueue,
            ManualResetEvent autoResetEvent)
        {
            _button1Observer = buttonObserver1;
            _button2Observer = button2Observer;
            _button3Observer = buttonObserver3;
            _button4Observer = buttonObserver4;
            lcd = new DisplayDriver();
            _dataQueueLCD = dataQueue;
            _calibrationEvent = autoResetEvent;
        }

        public void Calibrate()
        {
            while (true)
            {
                if (_button1Observer.IsPressed)
                {
                    //var dto = new LCD_DTO() {Message = $"Button has been pressed"};
                    //_dataQueueLCD.Add(dto);
                    
                }

                if (_button2Observer.IsPressed)
                {
                    _calibrationEvent.Set();
                }

                if (_button1Observer.startCal)
                {
                    _calibrationEvent.Reset();
                    _button1Observer.startCal = false;
                    lcd.lcdPrint("Calibration started");

                }

                Thread.Sleep(20);
            }


        }
    }
}