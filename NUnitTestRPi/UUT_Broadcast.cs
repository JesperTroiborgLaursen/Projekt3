using System;
using System.Collections.Concurrent;
using Domain.DTOModels;
using Domain.Models;
using NUnit.Framework;

namespace NUnitTestRPi
{
    [TestFixture]
    public class UUT_Broadcast
    {
        private static int PORT = 11000;
        private static string ip = "192.168.137.168"; //IP for monitor
        private BlockingCollection<Broadcast_DTO> _dataQueueBroadcast;
        private BlockingCollection<MetaData_DTO> _dataQueueMetaData;
        private BlockingCollection<Alarm_DTO> _dataQueueAlarmToBroadcast;
        private BlockingCollection<Analyze_DTO> _dataQueueAnalyzeToBroadcast;
        private string message;
        private DateTime date;
        private int id;
        private bool stop = false;

        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }
        public Broadcast_DTO BroadcastDto{ get; set; }
        public MetaData_DTO MetaDataDto { get; set; }
        public Alarm_DTO AlarmDto { get; set; }
        public Analyze_DTO AnalyzeDto { get; set; } 
        [SetUp]
        public void Setup()
        {
         
            SamplePack sp = new SamplePack();
            for (int i = 0; i < 49; i++)
            {
                sp.SampleList.Add( new Sample(){ID = i, SamplePackID = 1, Value = (ushort)i});
            }
            BroadcastDto = new Broadcast_DTO()
            {
                //SamplePack = 
            };
            MetaDataDto = new MetaData_DTO();
            AlarmDto = new Alarm_DTO();
            AnalyzeDto = new Analyze_DTO();

            message = "";
        }

        [Test]
        public void Avg_CalculateAvgOfOneTo150_ResultIs74()
        {
            
            
        }
    }
}