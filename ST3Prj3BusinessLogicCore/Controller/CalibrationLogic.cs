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
        private static ButtonObserver _button2;
        private static ButtonObserver _button3;
        private static ButtonObserver _button4;
        //private static DisplayDriver lcd;
        private BlockingCollection<LCD_DTO> _dataQueueLCD;


        public CalibrationLogic(ButtonObserver buttonObserver1, ButtonObserver buttonObserver2, ButtonObserver buttonObserver3, ButtonObserver buttonObserver4, BlockingCollection<LCD_DTO> dataQueue)
        {
            _button1Observer = buttonObserver1;
            _button2 = buttonObserver2;
            _button3 = buttonObserver3;
            _button4 = buttonObserver4;
            //lcd = new DisplayDriver();
            _dataQueueLCD = dataQueue;
        }

        public void Calibrate()
        {
            while (true)
            {
                if (_button1Observer.ready.WaitOne())
                {
                    var dto = new LCD_DTO() {Message = $"Button has been pressed"};
                    _dataQueueLCD.Add(dto);
                }

                Thread.Sleep(20);
            }


        }
    }
}