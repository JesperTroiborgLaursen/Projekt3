using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BusinessLogic.Operations;
using Domain.DTOModels;
using Domain.Models;


namespace BussinessLogic.Controller
{

    //Left over from testing phase. Not going to be impl. in final system
    public class LCDProducer
    {
        private BlockingCollection<LCD_DTO> _dataQueueLCD;
        private BlockingCollection<Analyze_DTO> _dataQueueAnalyze;
        private ManualResetEvent _calibrationEventLcd;
        private LCD_DTO dto;
        private BPAvg _bpAvg;
        public double blodtryk { get; set; }

        public LCDProducer(BlockingCollection<LCD_DTO> dataQueueLCD, BlockingCollection<Analyze_DTO> dataQueueAnalyze,
            ManualResetEvent calibrationEventLcd)
        {
            _dataQueueLCD = dataQueueLCD;
            _dataQueueAnalyze = dataQueueAnalyze;
            _bpAvg = new BPAvg();
            blodtryk = 50;
            _calibrationEventLcd = calibrationEventLcd;
        }

        public void Run()
        {
            while (_calibrationEventLcd.WaitOne())
            {
                while (!_dataQueueAnalyze.IsCompleted && _calibrationEventLcd.WaitOne())
                {
                    try
                    {
                        var container = _dataQueueAnalyze.Take();

                        blodtryk  = container.AvgBP;

                        dto = new LCD_DTO() {Message = $"Blodtryk = {blodtryk.ToString("n2")}"};

                        _dataQueueLCD.Add(dto);
                    }
                    catch (InvalidOperationException)
                    {
                        continue;
                    }

                    Thread.Sleep(1000);


                }
                Thread.Sleep(0);

            }
        }
    }
}