using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Domain.DTOModels;
using Domain.Models;
using RaspberryPiCore.ADC;

namespace DataAccesLogic.Boundaries
{

    public class Measure
    {
        private static ADC1015 adc;

        public int id { get; set; }
       

        private BlockingCollection<Broadcast_DTO> _dataQueueBroadcast;
        private BlockingCollection<Measure_DTO> _dataQueueMeasure;
        private BlockingCollection<LocalDB_DTO> _dataQueueLocalDB;
        private BlockingCollection<ADC_DTO> _dataQueueADC;
        private BlockingCollection<Measure_DTO> _dataQueueAnalyze;
        private BlockingCollection<Adjustments_DTO> _dataQueueAdjustments;
        public ManualResetEvent _calibrationEvent { get; set; }

        private double convertingFactor =0.25965; //Value found by experimenting. Calibration adjustment isn't accurate.
        private double zeroPoint;


        public double ConvertingFactor
        {
            get { return convertingFactor; }
            set { convertingFactor = value; }
        }

        

        //Stop for stopping thread
        private bool stop = false;
        


        public bool Stop
        {
            get { return stop = false; }
            set { stop = value; }
        }

        public Measure(BlockingCollection<Broadcast_DTO> dataQueueBroadcast,
            BlockingCollection<Measure_DTO> dataQueueMeasure,
            BlockingCollection<LocalDB_DTO> dataQueueLocalDb, BlockingCollection<ADC_DTO> dataQueueAdc,
            BlockingCollection<Adjustments_DTO> dataQueueAdjustments, BlockingCollection<Measure_DTO> dataQueueAnalyze,
            ManualResetEvent calibrationResetEvent)
        {
            _dataQueueBroadcast = dataQueueBroadcast;
            _dataQueueMeasure = dataQueueMeasure;
            _dataQueueLocalDB = dataQueueLocalDb;
            _dataQueueADC = dataQueueAdc;
            _calibrationEvent = calibrationResetEvent;
            _dataQueueAdjustments = dataQueueAdjustments;
            _dataQueueAnalyze = dataQueueAnalyze;
            adc = new ADC1015();

        }


        public void Run()
        {
            while(true)
            {
                while (_calibrationEvent.WaitOne())
                {
                    while (!Stop && _calibrationEvent.WaitOne())
                    {
                        //Update battery
                        MeasureBattery();
                        if (_dataQueueAdjustments.Count != 0)
                        {
                            var DTO = _dataQueueAdjustments.Take();
                            if (DTO.Calibration != 0)
                            {
                                ConvertingFactor = DTO.Calibration;
                            }
                            else if (DTO.ZeroPoint != 0)
                            {
                                zeroPoint = DTO.ZeroPoint;
                            }
                        }

                        var samplePack = new SamplePack();
                        var ls = new List<Sample>();
                        for (int i = 0; i < 50; i++)
                        {
                            ls.Add(new Sample()
                            {
                                Value = Convert.ToUInt16((adc.readADC_SingleEnded(2) *
                                                          Math.Sqrt(ConvertingFactor * ConvertingFactor)) - zeroPoint)
                            }); //Taking numerical value
                            Thread.Sleep(20);
                        }

                        samplePack.SampleList = ls;
                        samplePack.Date = DateTime.Now;

                        Broadcast_DTO broadcastDto = new Broadcast_DTO() {SamplePack = samplePack};
                        Measure_DTO measureDto = new Measure_DTO() {SamplePack = samplePack};
                        LocalDB_DTO localDbDto = new LocalDB_DTO() {SamplePack = samplePack};


                        //Checking if queues have been closed
                        if (!_dataQueueMeasure.IsCompleted && !_dataQueueBroadcast.IsCompleted &&
                            !_dataQueueLocalDB.IsCompleted)
                        {
                            _dataQueueBroadcast.Add(broadcastDto);
                            _dataQueueMeasure.Add(measureDto);
                            _dataQueueLocalDB.Add(localDbDto);
                            _dataQueueAnalyze.Add((measureDto));
                        }
                        else
                        {
                            break;
                        }

                        //Sleep
                        Thread.Sleep(0);

                    }

                    Thread.Sleep(0);

                }

                Thread.Sleep(1);
            }

            
        }

        void MeasureBattery()
        {
            var adcDto = new ADC_DTO();
            adcDto.voltage = adc.readADC_SingleEnded(0);
            adcDto.voltageOverResistor = adc.readADC_SingleEnded(1);

            _dataQueueADC.Add(adcDto);
        }
    }
}