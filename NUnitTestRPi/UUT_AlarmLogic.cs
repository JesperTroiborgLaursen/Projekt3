using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Threading;
using BusinessLogic.Controller;
using DataAccesLogic.Drivers;
using DataAccessLogic.Boundaries;
using Domain.DTOModels;
using FluentAssertions;
using FluentAssertions.Common;
using Interfaces;

namespace NUnitTestRPi
{
    [TestFixture]
    public class UUT_AlarmLogic
    {
        public BlockingCollection<Analyze_DTO> _dataQueueAnalyze { get; set; }
        private BlockingCollection<Alarm_DTO> _dataQueueAlarmToBroadcast;

        public Analyze_DTO OldAnalyzeDto { get; set; }
        public Analyze_DTO NewAnalyzeDto { get; set; }
        public AlarmConcreteSubject alarm { get; set; }
        private Alarm_DTO dtoToBroadcast;
        private AlarmLogic uut;
        private IAlarmObserver fakeSomo;

        [SetUp]
        public void Setup()
        {
            //AnalyzeLogicqueue
            _dataQueueAnalyze = new BlockingCollection<Analyze_DTO>();
            _dataQueueAlarmToBroadcast = new BlockingCollection<Alarm_DTO>();


            OldAnalyzeDto = new Analyze_DTO()
            {
                AvgBP = 90,
                BatteryVoltageInPercent = 80,
                Dia = 110,
                Sys = 80,
                Pulse = 100
            };

            NewAnalyzeDto = new Analyze_DTO()
            {
                AvgBP = 90,
                BatteryVoltageInPercent = 80,
                Dia = 110,
                Sys = 80,
                Pulse = 100
            };

            _dataQueueAnalyze.Add(NewAnalyzeDto);
            
            //alarm = new FakeAlarmConcreteSubject();
            dtoToBroadcast = new Alarm_DTO(){BpAlarm = 0, PulseAlarm = 0, BatteryAlarm = 0};
            uut = new AlarmLogic(_dataQueueAnalyze, _dataQueueAlarmToBroadcast);
            fakeSomo = new FakeSomo();
            uut.alarm.somo = fakeSomo;
            
        }

        [Test]
        public void CheckBP_BPDropsMoreThan30Perc_AlarmReturn1()
        {
            OldAnalyzeDto.AvgBP = 40;
            uut.OldAnalyzeDto = OldAnalyzeDto;
            uut.CheckBP(_dataQueueAnalyze.Take());
            uut.dtoToBroadcast.BpAlarm.Should().Be(1);
            uut.alarm.somo.priority.Should().Be(1);

        }
        [Test]
        public void CheckBP_BPDropsMoreThan20PercLessThan30perc_AlarmReturn2()
        {
            OldAnalyzeDto.AvgBP = 68;
            uut.OldAnalyzeDto = OldAnalyzeDto;
            uut.CheckBP(_dataQueueAnalyze.Take());
            uut.dtoToBroadcast.BpAlarm.Should().Be(2);
            uut.alarm.somo.priority.Should().Be(2);

        }

        [Test]
        public void CheckBP_BPDropsMoreThan10PercLessThan20perc_AlarmReturn2()
        {
            OldAnalyzeDto.AvgBP = 75;
            uut.OldAnalyzeDto = OldAnalyzeDto;
            uut.CheckBP(_dataQueueAnalyze.Take());
            uut.dtoToBroadcast.BpAlarm.Should().Be(3);
            uut.alarm.somo.priority.Should().Be(3);
        }

        [Test]
        public void CheckBP_BPDropsLessThan10perc_AlarmReturn0()
        {
            OldAnalyzeDto.AvgBP = 85;
            uut.OldAnalyzeDto = OldAnalyzeDto;
            uut.dtoToBroadcast.BpAlarm.Should().Be(0);
            uut.alarm.somo.priority.Should().Be(0);
        }
    }

    public class FakeSomo : IAlarmObserver
    {
        public int priority { get; set; }
        public void UpdateBattery(int priority)
        {
            this.priority = priority;
        }

        public void UpdateBP(int priority)
        {
            this.priority = priority;
        }

        public void UpdatePulse(int priority)
        {
            this.priority = priority;
        }
    }

    internal class FakeAlarmLogic : AlarmLogic
    {
        public FakeAlarmLogic(BlockingCollection<Analyze_DTO> dataQueueAnalyze, BlockingCollection<Alarm_DTO> dataQueueAlarmToBroadcast) : base(dataQueueAnalyze, dataQueueAlarmToBroadcast)
        {
        }

        public int CheckBP(Analyze_DTO DTO)
        {
            int priority = 0;
            //Take BP fra en kø fra analyzeDataQueue
            //Hvis den nye er 20% højere eller lavere end den gamle
            if (DTO.AvgBP > OldAnalyzeDto.AvgBP*1.30 || DTO.AvgBP < OldAnalyzeDto.AvgBP*1.30)
            {
                alarm.NotifyBP(1);
                dtoToBroadcast.BpAlarm = 1;
                priority = 1;
            }
                    
            //Hvis den er mellem for høj/lav ConcreteSubject.NotifyBP(2)
            else if (DTO.AvgBP > OldAnalyzeDto.AvgBP*1.20 || DTO.AvgBP < OldAnalyzeDto.AvgBP*1.20)
            {
                alarm.NotifyBP(2);
                dtoToBroadcast.BpAlarm = 2;
                priority = 2;
            }
            //Hvis den er lidt for høj/lav ConcreteSubject.NotifyBP(3)
            else if (DTO.AvgBP > OldAnalyzeDto.AvgBP*1.10 || DTO.AvgBP < OldAnalyzeDto.AvgBP*1.10)
            {
                alarm.NotifyBP(3);
                dtoToBroadcast.BpAlarm = 3;
                priority = 3;
            }
            else
            {
                dtoToBroadcast.BpAlarm = 0;  
                priority = 0;
            }

            return priority;
        }

        void CheckBattery(Analyze_DTO DTO)
        {
            //Take fra en kø med batterispænding
            //Hvis den er meget lav 
            if (DTO.BatteryVoltageInPercent < 15)
            {
                alarm.NotifyBattery(1);
                dtoToBroadcast.BatteryAlarm = 1;
            }
            //Hvis den er mellem lav
            else if (DTO.BatteryVoltageInPercent < 30)
            {
                alarm.NotifyBattery(2);
                dtoToBroadcast.BatteryAlarm = 2;
            }
            //Hvis den er lidt lav
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


            //Take pulse fra en kø fra analyzeDataQueue
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

    //public class FakeAlarmConcreteSubject : AlarmConcreteSubject
    //{
    //    public int FakeNotifyBattery(int priority)
    //    {
    //        return priority;
    //    }
    //    public int FakeNotifyBP(int priority)
    //    {
    //        return priority;
    //    }

    //    public int FakeNotifyPulse(int priority)
    //    {
    //        return priority;
    //    }
    //}
}