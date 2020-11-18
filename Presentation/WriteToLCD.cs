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

        public WriteToLCD(BlockingCollection<LCD_DTO> dataQueueLCD)
        {
            _dataQueueLCD = dataQueueLCD;

            lcd = new DisplayDriver();
        }



        public void Run()
        {
            while (!_dataQueueLCD.IsCompleted)
            {
                try
                {
                    var container = _dataQueueLCD.Take();
                    string message= container.Message;
                    
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