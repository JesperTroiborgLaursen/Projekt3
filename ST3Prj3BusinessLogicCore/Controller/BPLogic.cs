using System;
using Domain.Models;
using RaspberryPiCore.ADC;

namespace BusinessLogic.Controller
{
    public class BPLogic
    {
        private ADC1015 adc;
        //private CtrlDataAccessLogic cdal;
        public BPLogic()
        {
            adc = new ADC1015();
            //cdal = new CtrlDataAccessLogic();
        }

        //public SamplePack ReadAdc()
        //{
        //    SamplePack samplePack = new SamplePack(DateTime.Now, GenerateSamplePackID());
        //    for (int i = 0; i < 50; i++)
        //    {
        //        samplePack.SampleList.Add(new Sample(){Value = Convert.ToInt16(adc.readADC_SingleEnded(0))});
        //    }

        //    return samplePack;
        //}

        //private int GenerateSamplePackID()
        //{
        //    int lastId = cdal.GetLastSamplePackID();
        //    int newId = lastId++;
        //    return newId;
        //}
    }
}
