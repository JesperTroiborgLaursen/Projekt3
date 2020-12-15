using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Domain.DTOModels;
using Domain.Models;

namespace DataAccesLogic.Boundaries
{
    public class Broadcast
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
        
        public Broadcast(BlockingCollection<Broadcast_DTO> dataQueueBroadcast, BlockingCollection<MetaData_DTO> dataQueueMetaData,
            BlockingCollection<Alarm_DTO> dataQueueAlarmToBroadcast, BlockingCollection<Analyze_DTO> dataQueueAnalyzeToBroadcast)
        {
            _dataQueueBroadcast = dataQueueBroadcast;
            _dataQueueMetaData = dataQueueMetaData;
            _dataQueueAlarmToBroadcast = dataQueueAlarmToBroadcast;
            _dataQueueAnalyzeToBroadcast = dataQueueAnalyzeToBroadcast;
            BroadcastDto = new Broadcast_DTO();
            MetaDataDto = new MetaData_DTO();
            AlarmDto = new Alarm_DTO();
            AnalyzeDto = new Analyze_DTO();

            message = "";
        }

        public void Run()
        {
            while(!Stop)
            {
                while (!_dataQueueBroadcast.IsCompleted)
                {
                   message =
                        $"{AlarmToString()}{id}\r\n{MetaDataToString()}{date}\r\n{AnalyzeToString()}{SamplePackToString()}";

                   IPAddress broadcast = IPAddress.Parse(ip);
                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                    byte[] sendbuf = Encoding.ASCII.GetBytes(message);

                    IPEndPoint ep = new IPEndPoint(broadcast, PORT);

                    s.SendTo(sendbuf, ep);

                    Thread.Sleep(1000);
                }
            }

        }

        public string SamplePackToString()
        {
            string result = "";
            try
            {
                if (_dataQueueBroadcast.Count >= 1)
                {
                    var container = _dataQueueBroadcast.Take();
                
                    BroadcastDto = container;
                }
                
                SamplePack samplePack = BroadcastDto.SamplePack;


                date = samplePack.Date;
                id = samplePack.ID;

                foreach (var VARIABLE in samplePack.SampleList)
                {
                    result = $"{result}\r\n{VARIABLE.Value}";
                }

                return result;
            }
            catch (InvalidOperationException)
            {
            }

            return result;
        }


        public string AnalyzeToString()
        {
            string result = "";
            try
            {
                if (_dataQueueAnalyzeToBroadcast.Count >= 1)
                {
                    var dto = _dataQueueAnalyzeToBroadcast.Take();
                    AnalyzeDto = dto;
                }
                
                
                result = $"{AnalyzeDto.Dia}\r\n{AnalyzeDto.Sys}\r\n{(int)AnalyzeDto.AvgBP}\r\n{AnalyzeDto.Pulse}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }


        public string AlarmToString()
        {
            string result ="";
            try
            {
                
                if (_dataQueueAlarmToBroadcast.Count >= 1)
                {
                    var dto = _dataQueueAlarmToBroadcast.Take();
                    AlarmDto = dto;
                }

                result = $"{AlarmDto.BpAlarm}\r\n{AlarmDto.PulseAlarm}\r\n{AlarmDto.BatteryAlarm}\r\n";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }


        public string MetaDataToString()
        {
            string result ="";
            try
            {
                

                if (_dataQueueMetaData.Count >= 1)
                {
                    var dto = _dataQueueMetaData.Take();
                    MetaDataDto = dto;
                }

                result = $"{MetaDataDto.Gender}\r\n{MetaDataDto.Age}\r\n";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }

    }
}