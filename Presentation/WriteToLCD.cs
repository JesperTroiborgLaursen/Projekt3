using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using DataAccesLogic.Drivers;
using Domain.DTOModels;
using RaspberryPiCore.LCD;

namespace Presentation
{
    public class WriteToLCD
    {
        private BlockingCollection<Analyze_DTO> _dataQueueAnalyzeLCD;
        private BlockingCollection<LCD_DTO> _dataQueueLCD;
        //private DisplayDriver lcd;
        public SerLCD lcd { get; private set; }
        public ManualResetEvent _calibrationEvent { get; set; }
        

        public WriteToLCD(BlockingCollection<LCD_DTO> dataQueueLCD, BlockingCollection<Analyze_DTO> dataQueueAnalyze, ManualResetEvent manualResetEvent)
        {

            lcd = new SerLCD();
            lcd.lcdDisplay();
            //lcd.lcdSetBackLight(238, 29, 203); //Makes color pink
            //lcd.lcdSetContrast(0);

            _dataQueueLCD = dataQueueLCD;
            _dataQueueAnalyzeLCD = dataQueueAnalyze;
            _calibrationEvent = manualResetEvent;

            //lcd = new DisplayDriver();
        }
        
        public void Run()
        {
            while(true)
            {
                while (_dataQueueAnalyzeLCD.Count != 0 || _dataQueueLCD.Count != 0)
                {
                   while (!_calibrationEvent.WaitOne() && _dataQueueAnalyzeLCD.Count == 0)
                        {
                            try
                            {
                                var container = _dataQueueLCD.Take();
                                string message = container.Message;
                                //message = "Test";


                                lcd.lcdClear();
                                lcd.lcdPrint(message);
                                    
                            }
                            catch (InvalidOperationException)
                            {
                                continue;
                            }

                            Thread.Sleep(2);

                        }

                        Thread.Sleep(0);
                    

                    while (_dataQueueAnalyzeLCD.Count != 0)
                    {
                        try
                        {
                            var container = _dataQueueAnalyzeLCD.Take();


                            lcd.lcdClear();
                            lcd.lcdPrint(
                                $"{DateTime.Now}    Battery:{container.BatteryVoltageInPercent}      BP: {container.AvgBP}" +
                                $"   DIA:{container.Dia}   SYS:{container.Sys}   PULSE: {container.Pulse}"
                            );
                            Debug.WriteLine($"{DateTime.Now}    Battery:{container.BatteryVoltageInPercent}      BP: {container.AvgBP}" +
                                            $"   DIA:{container.Dia}   SYS:{container.Sys}   PULSE: {container.Pulse}");

                        }
                        catch (InvalidOperationException)
                        {
                            continue;
                        }

                        Thread.Sleep(500);
                    }

                    Thread.Sleep(0);
                }
                Thread.Sleep(0);
            }
            
        }
    }
}