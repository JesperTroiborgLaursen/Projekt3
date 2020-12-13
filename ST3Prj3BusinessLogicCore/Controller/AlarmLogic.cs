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
        private BlockingCollection<Alarm_DTO> _dataQueueAlarmToBroadcast;

        public Analyze_DTO OldAnalyzeDto { get; set; }
        public AlarmConcreteSubject alarm { get; set; }
        private Alarm_DTO dtoToBroadcast;

        private bool stop=false;
        

        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }

        public AlarmLogic(BlockingCollection<Analyze_DTO> dataQueueAnalyze,
            BlockingCollection<Alarm_DTO> dataQueueAlarmToBroadcast)
        {
            //AnalyzeLogicqueue
            _dataQueueAnalyze = dataQueueAnalyze;
            _dataQueueAlarmToBroadcast = dataQueueAlarmToBroadcast;
            OldAnalyzeDto = new Analyze_DTO();
            alarm = new AlarmConcreteSubject();
            dtoToBroadcast = new Alarm_DTO(){BpAlarm = 0, PulseAlarm = 0, BatteryAlarm = 0};
            
        }

        public void Run()
        {
            while (!Stop)
            {

                while (_dataQueueAnalyze.Count == 0)
                {
                    Thread.Sleep(0);
                }
                try
                {
                    var dtoFromAnalyze = _dataQueueAnalyze.Take();
                    

                    CheckBP(dtoFromAnalyze);
                    CheckBattery(dtoFromAnalyze);
                    CheckPulse(dtoFromAnalyze);

                    _dataQueueAlarmToBroadcast.Add(dtoToBroadcast);
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
                dtoToBroadcast.BpAlarm = 1;
            }
                    
            //Hvis den er mellem for høj/lav ConcreteSubject.NotifyBP(2)
            else if (DTO.AvgBP > OldAnalyzeDto.AvgBP*1.20 || DTO.AvgBP < OldAnalyzeDto.AvgBP*1.20)
            {
                alarm.NotifyBP(2);
                dtoToBroadcast.BpAlarm = 2;
            }
            //Hvis den er lidt for høj/lav ConcreteSubject.NotifyBP(3)
            else if (DTO.AvgBP > OldAnalyzeDto.AvgBP*1.10 || DTO.AvgBP < OldAnalyzeDto.AvgBP*1.10)
            {
                alarm.NotifyBP(3);
                dtoToBroadcast.BpAlarm = 3;
            }
            else
            {
                dtoToBroadcast.BpAlarm = 0;   
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
                dtoToBroadcast.BatteryAlarm = 1;
            }

            else if (DTO.BatteryVoltageInPercent < 30)
            {
                alarm.NotifyBattery(2);
                dtoToBroadcast.BatteryAlarm = 2;
            }

            else if (DTO.BatteryVoltageInPercent < 50)
            {
                alarm.NotifyBattery(1);
                dtoToBroadcast.BatteryAlarm = 3;
            }
            else
            {
                dtoToBroadcast.BatteryAlarm = 0;   
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
                dtoToBroadcast.PulseAlarm = 1;
            }
                    
            //Hvis den er mellem for høj/lav ConcreteSubject.NotifyPulse(2)
            else if (DTO.Pulse > OldAnalyzeDto.Pulse*1.20 || DTO.Pulse< OldAnalyzeDto.Pulse*1.20)
            {
                alarm.NotifyPulse(2);
                dtoToBroadcast.PulseAlarm = 2;
            }
            //Hvis den er lidt for høj/lav ConcreteSubject.NotifyPulse(3)
            else if (DTO.Pulse > OldAnalyzeDto.Pulse*1.10 || DTO.Pulse< OldAnalyzeDto.Pulse*1.10)
            {
                alarm.NotifyPulse(3);
                dtoToBroadcast.PulseAlarm = 3;
            }
            else
            {
                dtoToBroadcast.PulseAlarm = 0;   
            }
        }
    }
}