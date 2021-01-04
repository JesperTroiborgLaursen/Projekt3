using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using Domain.DTOModels;
using RaspberryPiCore.LCD;

namespace Presentation
{
    public class WriteToLCD
    {
        private BlockingCollection<Analyze_DTO> _dataQueueAnalyzeLCD;
        private BlockingCollection<LCD_DTO> _dataQueueLCD;
        public SerLCD lcd;
        public ManualResetEvent _calibrationEvent { get; set; }
        private bool stop = false;

        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }
        

        public WriteToLCD(BlockingCollection<LCD_DTO> dataQueueLCD, BlockingCollection<Analyze_DTO> dataQueueAnalyze, ManualResetEvent manualResetEvent)
        {
            lcd = new SerLCD();
            lcd.lcdDisplay();

            _dataQueueLCD = dataQueueLCD;
            _dataQueueAnalyzeLCD = dataQueueAnalyze;
            _calibrationEvent = manualResetEvent;

        }
        
        public void Run()
        {
            while(!Stop)
            {
                while (_dataQueueAnalyzeLCD.Count != 0 || _dataQueueLCD.Count != 0)
                {
                   while (!_calibrationEvent.WaitOne() && _dataQueueAnalyzeLCD.Count == 0)
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

                       Thread.Sleep(2);

                   }
                   
                   while (_calibrationEvent.WaitOne() && _dataQueueAnalyzeLCD.Count != 0)
                   {
                       try
                       {
                           var container = _dataQueueAnalyzeLCD.Take();

                           lcd.lcdClear();
                           lcd.lcdPrint(
                               $"{DateTime.Now}    Battery:{(int)container.BatteryVoltageInPercent}      BP: {(int)container.AvgBP}" +
                               $"   DIA:{(int)container.Dia}   SYS:{(int)container.Sys}   PULSE: {(int)container.Pulse}"
                           );
                           Debug.WriteLine($"{DateTime.Now}    Battery:{(int)container.BatteryVoltageInPercent}      BP: {(int)container.AvgBP}" +
                                           $"   DIA:{(int)container.Dia}   SYS:{(int)container.Sys}   PULSE: {(int)container.Pulse}");

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