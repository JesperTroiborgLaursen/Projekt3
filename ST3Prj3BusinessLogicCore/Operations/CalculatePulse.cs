using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Domain.DTOModels;

namespace BusinessLogic.Operations
{
    public class CalculatePulse
    {
        private int threshold = 90; //mmHg
        private List<int> lsSamples;
        private bool Thresholdreached1 = false;
        private bool Thresholdreached2 = false;
        private bool Thresholdreached3 = false;
        public BlockingCollection<Measure_DTO> _dataQueueMeasure { get; set; }

        public CalculatePulse(BlockingCollection<Measure_DTO> dataQueueMeasure)
        {
            _dataQueueMeasure = dataQueueMeasure;
            lsSamples = new List<int>();
        }

        public int CalcPulse()
        {
            //Opsamler 3 sekunders data i lsSamples
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(1000);
                if (_dataQueueMeasure != null)
                {
                    var container = _dataQueueMeasure.Take();
                    foreach (var sample in container.SamplePack.SampleList)
                    {
                        lsSamples.Add((int) sample.Value);
                    }
                    i++;
                }
            }
            //Sammenligner med Threshold, alle værdier derover, kan potentielt være pulsslag.
            foreach (var value in lsSamples)
            {
                int highestSample1 = 0;
                if (value >= threshold)
                {
                    Thresholdreached1 = true;
                    highestSample1 = value;
                    int highestSampleIndex1 = lsSamples.IndexOf(value);
                }

                if (Thresholdreached1 == true && value < threshold)
                {
                    
                }
            }
            
            var result = new int();

            return result;
        }

    }
}
