using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesCore;
using DomaineCore;
using DomaineCore.Models;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using DataAccessLogicCore.Boundaries;

namespace BusinessLogicCore.Controller
{
    public class BPLogic : IBusinessLogic
    {
        private ADC1015 adc;
        private CtrlDataAccessLogic cdal;
        public BPLogic()
        {
            adc = new ADC1015();
            cdal = new CtrlDataAccessLogic();
        }

        public SamplePack ReadAdc()
        {
            SamplePack samplePack = new SamplePack(DateTime.Now, GenerateSamplePackID());
            for (int i = 0; i < 50; i++)
            {
                samplePack.SampleList.Add(new Sample(){Value = adc.readADC_Differential_0_1()});
            }

            return samplePack;
        }

        public int GenerateSamplePackID()
        {
            int lastId = cdal.GetLastSamplePackID();
            int newId = lastId++;
            return newId;
        }
    }
}
