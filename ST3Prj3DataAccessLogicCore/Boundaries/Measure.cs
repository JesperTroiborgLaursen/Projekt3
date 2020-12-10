using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading;
using Domain.DTOModels;
using Domain.Models;
using RaspberryPiCore.ADC;

namespace DataAccesLogic.Boundaries
{

    public class Measure
    {
        private static ADC1015 adc;

        //private static SamplePack samplePack;
        //private static List<Sample> ls;
        //private static short[] shortbuffer;
        public int id { get; set; }
        private BlockingCollection<Broadcast_DTO> _dataQueueBroadcast = new BlockingCollection<Broadcast_DTO>();
        private BlockingCollection<Measure_DTO> _dataQueueMeasure = new BlockingCollection<Measure_DTO>();
        private BlockingCollection<LocalDB_DTO> _dataQueueLocalDB = new BlockingCollection<LocalDB_DTO>();
        private BlockingCollection<ADC_DTO> _dataQueueADC = new BlockingCollection<ADC_DTO>();
        public ManualResetEvent _calibrationEvent { get; set; }
        private double convertingFactor =0.25965; //Beregnet a værdi = 3.7801

        public double ConvertingFactor
        {
            get { return convertingFactor; }
            set { convertingFactor = value; }
        }


        //Stop til at stoppe måling. Når den skal stoppes, sættes denne til true.
        private bool stop = false;

        public bool Stop
        {
            get { return stop = false; }
            set { stop = value; }
        }

        public Measure(BlockingCollection<Broadcast_DTO> dataQueueBroadcast,
            BlockingCollection<Measure_DTO> dataQueueMeasure, BlockingCollection<LocalDB_DTO> dataQueueLocalDb, BlockingCollection<ADC_DTO> dataQueueAdc,
            ManualResetEvent calibrationResetEvent)
        {
            _dataQueueBroadcast = dataQueueBroadcast;
            _dataQueueMeasure = dataQueueMeasure;
            _dataQueueLocalDB = dataQueueLocalDb;
            _dataQueueADC = dataQueueAdc;
            _calibrationEvent = calibrationResetEvent;
            adc = new ADC1015();

        }


        public void Run()
        {
            while(_calibrationEvent.WaitOne()) 
            {
                while (!Stop && _calibrationEvent.WaitOne())
                {
                    //Update battery
                    MeasureBattery();

                    var samplePack = new SamplePack();
                    var ls = new List<Sample>();
                    for (int i = 0; i < 50; i++)
                    {
                        ls.Add(new Sample() {Value = Convert.ToUInt16(adc.readADC_SingleEnded(2)*Math.Sqrt(ConvertingFactor*ConvertingFactor))});//Tager numerisk værdi for at sikre der ikke er minus værdier under test
                        Thread.Sleep(20);
                    }

                    samplePack.SampleList = ls;
                    samplePack.Date = DateTime.Now;
                    //samplePack.ID = id;
                    //id++;

                    //Her vil vi gemme i lokal DB
                    Broadcast_DTO broadcastDto = new Broadcast_DTO() {SamplePack = samplePack};
                    Measure_DTO measureDto = new Measure_DTO() {SamplePack = samplePack};
                    LocalDB_DTO localDbDto = new LocalDB_DTO() {SamplePack = samplePack};

                    //Checking if queues have been closed
                    if (!_dataQueueMeasure.IsCompleted && !_dataQueueBroadcast.IsCompleted && !_dataQueueLocalDB.IsCompleted)
                    {
                        _dataQueueBroadcast.Add(broadcastDto);
                        _dataQueueMeasure.Add(measureDto);
                        _dataQueueLocalDB.Add(localDbDto);
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

            _dataQueueBroadcast.CompleteAdding();
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