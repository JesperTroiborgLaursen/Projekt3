using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Threading;
using BusinessLogic.Controller;
using DataAccessLogic.Boundaries;
using Domain.DTOModels;
using FluentAssertions;
using FluentAssertions.Common;

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
            
            alarm = new AlarmConcreteSubject();
            dtoToBroadcast = new Alarm_DTO(){BpAlarm = 0, PulseAlarm = 0, BatteryAlarm = 0};
            uut = new AlarmLogic(_dataQueueAnalyze, _dataQueueAlarmToBroadcast);
           



        }

        [Test]
        public void CheckBP_BPDropsMoreThan30perc_AlarmReturn1()
        {
            OldAnalyzeDto.AvgBP = 40;
            uut.OldAnalyzeDto = OldAnalyzeDto;

            uut.CheckBP(_dataQueueAnalyze.Take());
            uut.dtoToBroadcast.BpAlarm.Should().Be(1);
        }
    }

    //public class StubAlarmConcreteSubject : AlarmConcreteSubject
    //{
    //    public int NotifyBattery(int priority)
    //    {
    //        return priority;
    //    }
    //    public int NotifyBP(int priority)
    //    {
    //        return priority;
    //    }

    //    public int NotifyPulse(int priority)
    //    {
    //        return priority;
    //    }
    //}
}