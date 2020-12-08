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
    public class LCDProducer
    {
        private BlockingCollection<LCD_DTO> _dataQueueLCD;
        private BlockingCollection<Measure_DTO> _dataQueueMeasure;
        private ManualResetEvent _calibrationEventLcd;
        private LCD_DTO dto;
        private BPAvg _bpAvg;
        public double blodtryk { get; set; }

        public LCDProducer(BlockingCollection<LCD_DTO> dataQueueLCD, BlockingCollection<Measure_DTO> dataQueueMeasure,
            ManualResetEvent calibrationEventLcd)
        {
            _dataQueueLCD = dataQueueLCD;
            _dataQueueMeasure = dataQueueMeasure;
            _bpAvg = new BPAvg();
            blodtryk = 50;
            _calibrationEventLcd = calibrationEventLcd;
        }

        public void Run()
        {
            while (_calibrationEventLcd.WaitOne())
            {
                while (!_dataQueueMeasure.IsCompleted && _calibrationEventLcd.WaitOne())
                {
                    try
                    {
                        var container = _dataQueueMeasure.Take();
                        SamplePack samplePack = container.SamplePack;
                        blodtryk = _bpAvg.Avg(samplePack);

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