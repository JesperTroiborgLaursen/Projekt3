using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading;
using BusinessLogic.Operations;
using Domain.DTOModels;

namespace BusinessLogic.Controller
{

    public class AnalyzeLogic
    {
        
        public BlockingCollection<Analyze_DTO> _dataQueueAlarm { get; set; }
        public BlockingCollection<Measure_DTO> _dataQueueMeasure { get; set; }
        public BlockingCollection<Battery_DTO> _dataQueueBattery { get; set; }
        public BlockingCollection<Analyze_DTO> _dataQueueLCD;
        public CalculatePulse calculatePulse { get; set; }
        public BPAvg bpAvg { get; set; }
        public FindDiastolic findDiastolic { get; set; }
        public FindSystolic findSystolic { get; set; }

        private bool stop = false;

        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }

        public AnalyzeLogic(BlockingCollection<Analyze_DTO> dataQueueAlarm,
            BlockingCollection<Measure_DTO> dataQueueMeasure, BlockingCollection<Battery_DTO> dataQueueBattery,
            BlockingCollection<Analyze_DTO> dataQueueLcd)
        {
            _dataQueueMeasure = dataQueueMeasure;
            _dataQueueAlarm = dataQueueAlarm;
            _dataQueueBattery = dataQueueBattery;
            _dataQueueLCD = dataQueueLcd;
            calculatePulse = new CalculatePulse(); 
            bpAvg = new BPAvg();
            findDiastolic = new FindDiastolic();
            findSystolic = new FindSystolic();
        }
        
        public void Run()
        {
            while (!Stop)
            {
                var DTO = new Analyze_DTO();
                var batteryMeasure = _dataQueueBattery.Take();
                var threeSecData = CollectData();
                //Opsaml 3 sekunders data
                //Find puls
                DTO.Pulse = calculatePulse.CalcPulse(threeSecData);
                //Find Dia
                findDiastolic.Find(threeSecData);
                //Find Sys
                findSystolic.Find(threeSecData);
                //Find Avg
                DTO.AvgBP = bpAvg.Avg(threeSecData);
                //Take battery voltage
                DTO.BatteryVoltageInPercent = batteryMeasure.VoltageLeftInPercent;

                //Add to queue
                _dataQueueAlarm.Add(DTO);
                _dataQueueLCD.Add(DTO);
                Thread.Sleep(0);
            }
            _dataQueueAlarm.CompleteAdding();
        }

        List<int> CollectData()
        {
            //Opsamler 3 sekunders data i lsSamples
            List<int> lsSamples = new List<int>();
            for (int i = 0; i < 3;)
            {
                Thread.Sleep(1000);
                if (!_dataQueueMeasure.IsCompleted)
                {
                    try
                    {
                        var container = _dataQueueMeasure.Take();
                        foreach (var sample in container.SamplePack.SampleList)
                        {
                            lsSamples.Add((int) sample.Value);
                        }
                        i++;
                    }
                    catch (InvalidOperationException)
                    {
                        continue;
                    }
                    
                }
            }

            return lsSamples;
        }

       
    }
}