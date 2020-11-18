using System;
using System.Collections.Concurrent;
using System.Threading;
using DataAccesLogic.Drivers;
using Domain.DTOModels;
using RaspberryPiCore.LCD;

namespace Presentation
{
    public class WriteToLCD
    {
        private BlockingCollection<LCD_DTO> _dataQueueLCD;
        private DisplayDriver lcd;
        public ManualResetEvent _calibrationEvent { get; set; }

        public WriteToLCD(BlockingCollection<LCD_DTO> dataQueueLCD, ManualResetEvent autoResetEvent)
        {
            _dataQueueLCD = dataQueueLCD;
            _calibrationEvent = autoResetEvent;
            lcd = new DisplayDriver();
        }



        public void Run()
        {
            while(_calibrationEvent.WaitOne()) 
            {
                while (!_dataQueueLCD.IsCompleted && _calibrationEvent.WaitOne())
                {
                    try
                    {
                        var container = _dataQueueLCD.Take();
                        string message = container.Message;

                        lcd.lcdClear();
                        lcd.lcdPrint(message);


                    }
                    catch (InvalidOperationException)
                    {
                        continue;
                    }

                    Thread.Sleep(500);
                }

            }
        }
    }
}