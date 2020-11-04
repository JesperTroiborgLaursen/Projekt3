using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using DomaineCore.Models;
using RaspberryPiCore.ADC;

namespace RPITest
{
    public class Measure
    {
        private static ADC1015 adc;

        private static SamplePack samplePack;
        private static List<Sample> ls;
        public int id { get; set; }
        private BlockingCollection<DataContainer> _dataQueue = new BlockingCollection<DataContainer>();

        private bool stop = false;
        public bool Stop
        {
            get { return stop = false; }
            set { stop = value; }
        }

        public Measure(BlockingCollection<DataContainer> dataQueue)
        {
            _dataQueue = dataQueue;
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
                    ls.Add(new Sample() { Value = Convert.ToInt16(adc.readADC_SingleEnded(0)) });
                }

                samplePack.SampleList = ls;
                samplePack.Date = DateTime.Now;
                samplePack.ID = id;
                id++;

                DataContainer measurement = new DataContainer() {SamplePack = samplePack};
                _dataQueue.Add(measurement);
            }

            _dataQueue.CompleteAdding();
        }



        //public SamplePack StartMeasurement()
        //{
        //    adc = new ADC1015();
        //    ls = new List<Sample>();

        //    samplePack = new SamplePack();

        //    for (int i = 0; i < 50; i++)
        //    {
        //        ls.Add(new Sample() { Value = Convert.ToInt16(adc.readADC_SingleEnded(0)) });
        //    }

        //    samplePack.SampleList = ls;
        //    samplePack.Date = DateTime.Now;
        //    samplePack.ID = id;
        //    id++;

        //    return samplePack;
        //}
    }
}