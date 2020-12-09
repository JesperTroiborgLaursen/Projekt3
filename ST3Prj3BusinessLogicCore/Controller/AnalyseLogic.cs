using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading;
using BusinessLogic.Operations;
using Domain.DTOModels;

namespace BusinessLogic.Controller
{

    public class AnalyseLogic
    {
        
        public BlockingCollection<Analyze_DTO> _dataQueueAnalyze { get; set; }
        public BlockingCollection<Measure_DTO> _dataQueueMeasure { get; set; }
        public CalculatePulse CalculatePulse { get; set; }
        public BPAvg BpAvg { get; set; }

        private bool stop = false;

        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }

        public AnalyseLogic(BlockingCollection<Analyze_DTO> dataqueueAnalyze, BlockingCollection<Measure_DTO> dataQueueMeasure)
        {
            _dataQueueMeasure = dataQueueMeasure;
            _dataQueueAnalyze = dataqueueAnalyze;
            CalculatePulse = new CalculatePulse(dataQueueMeasure); 
            BpAvg = new BPAvg();

        }
        
        void Run()
        {
            while (Stop)
            {
                var DTO = new Analyze_DTO();
                //Opsaml 3 sekunders data
                //Find puls
                DTO.Pulse = CalculatePulse.CalcPulse(CollectData());
                //Find Dia
                //Find Sys
                //Find Avg
                DTO.AvgBP = BpAvg.Avg(CollectData());
            }
        }

        List<int> CollectData()
        {
            //Opsamler 3 sekunders data i lsSamples
            List<int> lsSamples = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(1000);
                if (!_dataQueueMeasure.IsCompleted)
                {
                    var container = _dataQueueMeasure.Take();
                    foreach (var sample in container.SamplePack.SampleList)
                    {
                        lsSamples.Add((int) sample.Value);
                    }
                    i++;
                }
            }

            return lsSamples;
        }

       
    }
}