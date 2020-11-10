using System;
using System.Collections.Concurrent;
using System.Threading;
using Domain.DTOModels;
using RaspberryPiCore.LCD;

namespace Presentation
{
    public class WriteToLCD
    {
        private BlockingCollection<LCD_DTO> _dataQueueLCD;
        private SerLCD lcd;

        public WriteToLCD(BlockingCollection<LCD_DTO> dataQueueLCD)
        {
            _dataQueueLCD = dataQueueLCD;

            lcd = new SerLCD();
            lcd.lcdDisplay();
            lcd.lcdSetBackLight(238, 29, 203); //Makes color pink
            lcd.lcdSetContrast(0); //0 gives best contrast
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

                    Thread.Sleep(500);
                    
                    

                }
                catch (InvalidOperationException)
                {
                    continue;
                }


            }
        }
    }
}