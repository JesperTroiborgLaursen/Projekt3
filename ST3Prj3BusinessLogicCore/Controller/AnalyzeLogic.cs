using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using BusinessLogic.Operations;
using Domain.DTOModels;

namespace BusinessLogic.Controller
{

    public class AnalyzeLogic
    {

        public BlockingCollection<Analyze_DTO> _dataQueueAlarm;
        public BlockingCollection<Measure_DTO> _dataQueueMeasure;
        public BlockingCollection<Battery_DTO> _dataQueueBattery;
        public BlockingCollection<Analyze_DTO> _dataQueueLCD;
        private BlockingCollection<Analyze_DTO> _dataQueueAnalyzeToBroadcast;
        public CalculatePulse calculatePulse { get; set; }
        public BPAvg bpAvg { get; set; }
        public FindDiastolic findDiastolic { get; set; }
        public FindSystolic findSystolic { get; set; }
        private Battery_DTO batteryMeasure;

        private bool stop = false;
        

        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }

        public AnalyzeLogic(BlockingCollection<Analyze_DTO> dataQueueAlarm,
            BlockingCollection<Measure_DTO> dataQueueMeasure, BlockingCollection<Battery_DTO> dataQueueBattery,
            BlockingCollection<Analyze_DTO> dataQueueLcd, BlockingCollection<Analyze_DTO> dataQueueAnalyzeToBroadcast)
        {
            _dataQueueMeasure = dataQueueMeasure;
            _dataQueueAlarm = dataQueueAlarm;
            _dataQueueBattery = dataQueueBattery;
            _dataQueueLCD = dataQueueLcd;
            _dataQueueAnalyzeToBroadcast = dataQueueAnalyzeToBroadcast;
            calculatePulse = new CalculatePulse(); 
            bpAvg = new BPAvg();
            findDiastolic = new FindDiastolic();
            findSystolic = new FindSystolic();
            var batteryMeasure = new Battery_DTO();
        }
        
        public void Run()
        {
            while (!Stop)
            {
                var DTO = new Analyze_DTO();

                while (_dataQueueBattery.Count == 0)
                {
                    Thread.Sleep(1);
                }

                try
                {
                    batteryMeasure = _dataQueueBattery.Take();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
                var threeSecData = CollectData();

                //Opsaml 3 sekunders data
                //Find puls
                DTO.Pulse = calculatePulse.CalcPulse(threeSecData);
                //Find Dia
                DTO.Dia = findDiastolic.Find(threeSecData);
                //Find Sys
                DTO.Sys = findSystolic.Find(threeSecData);
                //Find Avg
                DTO.AvgBP = bpAvg.Avg(threeSecData);
                //Take battery voltage
                DTO.BatteryVoltageInPercent = batteryMeasure.VoltageLeftInPercent;

                //Add to queue
                _dataQueueAlarm.Add(DTO);
                _dataQueueLCD.Add(DTO);
                _dataQueueAnalyzeToBroadcast.Add(DTO);
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
                while (_dataQueueMeasure.Count == 0)
                {
                    Thread.Sleep(50);
                }
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