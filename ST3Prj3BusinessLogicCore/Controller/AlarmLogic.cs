using System;
using System.Collections.Concurrent;
using System.Threading;
using DataAccessLogic.Boundaries;
using Domain.DTOModels;

namespace BusinessLogic.Controller
{
    public class AlarmLogic
    {
        public BlockingCollection<Analyze_DTO> _dataQueueAnalyze { get; set; }
        public BlockingCollection<ADC_DTO> _dataQueueBattery { get; set; }
        public Analyze_DTO OldAnalyzeDto { get; set; }
        public AlarmConcreteSubject alarm { get; set; }

        private bool stop=false;

        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }

        public AlarmLogic(BlockingCollection<Analyze_DTO> dataQueueAnalyze)
        {
            //AnalyzeLogicqueue
            _dataQueueAnalyze = dataQueueAnalyze;
            OldAnalyzeDto = new Analyze_DTO();
            alarm = new AlarmConcreteSubject();
        }

        public void Run()
        {
            while (!Stop)
            {
                try
                {
                    var DTO = _dataQueueAnalyze.Take();

                    CheckBP(DTO);
                    CheckBattery(DTO);
                    CheckPulse(DTO);
                    Thread.Sleep(0);
                }
                catch (InvalidOperationException)
                {
                    continue;
                }
                Thread.Sleep(0);
                
            }
        }


        void CheckBP(Analyze_DTO DTO)
        {
            //Take BP fra en kø fra analyzeDataQueue
            //Hvis den nye er 20% højere eller lavere end den gamle
            //ConcreteSubject.NotifyBP(1)
            if (DTO.AvgBP > OldAnalyzeDto.AvgBP*1.30 || DTO.AvgBP < OldAnalyzeDto.AvgBP*1.30)
            {
                alarm.NotifyBP(1);
            }
                    
            //Hvis den er mellem for høj/lav ConcreteSubject.NotifyBP(2)
            if (DTO.AvgBP > OldAnalyzeDto.AvgBP*1.20 || DTO.AvgBP < OldAnalyzeDto.AvgBP*1.20)
            {
                alarm.NotifyBP(2);
            }
            //Hvis den er lidt for høj/lav ConcreteSubject.NotifyBP(3)
            if (DTO.AvgBP > OldAnalyzeDto.AvgBP*1.10 || DTO.AvgBP < OldAnalyzeDto.AvgBP*1.10)
            {
                alarm.NotifyBP(3);
            }
        }

        void CheckBattery(Analyze_DTO DTO)
        {
            //Take fra en kø med batterispænding
            //Hvis den er meget lav ConcreteSubject.NotifyBattery(1)
            //Hvis den er mellem lav ConcreteSubject.NotifyBattery(2)
            //Hvis den er lidt lav ConcreteSubject.NotifyBatery(3)
               
            if (DTO.BatteryVoltageInPercent < 15)
            {
                alarm.NotifyBattery(1);
            }

            if (DTO.BatteryVoltageInPercent < 30)
            {
                alarm.NotifyBattery(1);
            }

            if (DTO.BatteryVoltageInPercent < 50)
            {
                alarm.NotifyBattery(1);
            }
        }

        void CheckPulse(Analyze_DTO DTO)
        {
            //Take fra en kø med Puls
            //Hvis den er meget for høj/lav ConcreteSubject.NotifyPulse(1)
            //Hvis den er mellem for høj/lav ConcreteSubject.NotifyPulse(2)
            //Hvis den er lidt for høj/lav ConcreteSubject.NotifyPulse(3)


            //Take BP fra en kø fra analyzeDataQueue
            //Hvis den nye er 20% højere eller lavere end den gamle
            //ConcreteSubject.NotifyPulse(1)
            if (DTO.Pulse > OldAnalyzeDto.Pulse*1.30 || DTO.Pulse < OldAnalyzeDto.Pulse*1.30)
            {
                alarm.NotifyPulse(1);
            }
                    
            //Hvis den er mellem for høj/lav ConcreteSubject.NotifyPulse(2)
            if (DTO.Pulse > OldAnalyzeDto.Pulse*1.20 || DTO.Pulse< OldAnalyzeDto.Pulse*1.20)
            {
                alarm.NotifyPulse(2);
            }
            //Hvis den er lidt for høj/lav ConcreteSubject.NotifyPulse(3)
            if (DTO.Pulse > OldAnalyzeDto.Pulse*1.10 || DTO.Pulse< OldAnalyzeDto.Pulse*1.10)
            {
                alarm.NotifyPulse(3);
            }
        }
    }
}