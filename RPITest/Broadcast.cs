using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using DomaineCore.Models;

namespace RPITest
{
    public class Broadcast
    {
        private static int PORT = 11000;
        private static string ip = "192.168.137.30"; //Standard networking broadcast IP
        private BlockingCollection<Broadcast_DTO> _dataQueueBroadcast;

        public Broadcast(BlockingCollection<Broadcast_DTO> dataQueueBroadcast)
        {
            _dataQueueBroadcast = dataQueueBroadcast;
        }

        public void Run()
        {
            while (!_dataQueueBroadcast.IsCompleted)
            {
                try
                {
                    var container = _dataQueueBroadcast.Take();
                    SamplePack samplePack = container.SamplePack;
                    IPAddress broadcast = IPAddress.Parse(ip);
                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                    byte[] sendbuf = Encoding.ASCII.GetBytes(samplePack.ToString());

                    IPEndPoint ep = new IPEndPoint(broadcast, PORT);

                    s.SendTo(sendbuf, ep);

                }
                catch (InvalidOperationException)
                {
                    continue;
                }

                
            }

        }
    }
}