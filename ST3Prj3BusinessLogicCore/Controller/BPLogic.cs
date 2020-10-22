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

namespace BusinessLogicCore.Controller
{
    public class BPLogic
    {
        private ADC1015 adc;
        public BPLogic()
        {
            adc = new ADC1015();
        }

        public SamplePack ReadAdc()
        {
            SamplePack samplePack = new SamplePack();
            for (int i = 0; i < 50; i++)
            {
                samplePack.SampleList.Add(new Sample(){Value = adc.readADC_Differential_0_1()});
            }

            return samplePack;
        }
    }
}
