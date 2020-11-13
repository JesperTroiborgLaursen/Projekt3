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

        private static SamplePack samplePack;
        private static List<Sample> ls;
        private static short[] shortbuffer;
        public int id { get; set; }
        private BlockingCollection<Broadcast_DTO> _dataQueueBroadcast = new BlockingCollection<Broadcast_DTO>();
        private BlockingCollection<Measure_DTO> _dataQueueMeasure = new BlockingCollection<Measure_DTO>();
        private BlockingCollection<LocalDB_DTO> _dataQueueLocalDB = new BlockingCollection<LocalDB_DTO>();


        //Stop til at stoppe måling. Når den skal stoppes, sættes denne til true.
        private bool stop = false;
        public bool Stop
        {
            get { return stop = false; }
            set { stop = value; }
        }

        public Measure(BlockingCollection<Broadcast_DTO> dataQueueBroadcast, BlockingCollection<Measure_DTO> dataQueueMeasure, BlockingCollection<LocalDB_DTO> dataQueueLocalDb)
        {
            _dataQueueBroadcast = dataQueueBroadcast;
            _dataQueueMeasure = dataQueueMeasure;
            _dataQueueLocalDB = dataQueueLocalDb;
        }


        public void Run()
        {
            adc = new ADC1015();
            ls = new List<Sample>();
            while (!stop)
            {
                samplePack = new SamplePack();
                
                for (int i = 0; i < 50; i++)
                {
                    ls.Add(new Sample() {Value = Convert.ToUInt16(adc.readADC_SingleEnded(0))});
                }

                samplePack.SampleList = ls;
                samplePack.Date = DateTime.Now;
                samplePack.ID = id;
                id++;

                //Her vil vi gemme i lokal DB
                Broadcast_DTO broadcastDto = new Broadcast_DTO() {SamplePack = samplePack};
                Measure_DTO measureDto = new Measure_DTO() { SamplePack = samplePack };
                LocalDB_DTO localDbDto = new LocalDB_DTO() { SamplePack = samplePack };
                _dataQueueBroadcast.Add(broadcastDto);
                _dataQueueMeasure.Add(measureDto);
                _dataQueueLocalDB.Add(localDbDto);
            }

            _dataQueueBroadcast.CompleteAdding();
        }
    }
}