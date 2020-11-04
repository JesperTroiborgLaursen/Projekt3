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
        private static string ip = "192.168.43.109"; //Standard networking broadcast IP
        private BlockingCollection<DataContainer> _dataQueue;

        public Broadcast(BlockingCollection<DataContainer> dataQueue)
        {
            _dataQueue = dataQueue;
        }

        public void Run()
        {
            while (!_dataQueue.IsCompleted)
            {
                try
                {
                    var container = _dataQueue.Take();
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
        //public void BroadcastMessage(String message)
        //{
        //    IPAddress broadcast = IPAddress.Parse(ip);
        //    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        //    byte[] sendbuf = Encoding.ASCII.GetBytes(message);
        //    //byte[] sendbufhundred = new byte[100];
        //    //sendbuf.CopyTo(sendbufhundred,0);
        //    IPEndPoint ep = new IPEndPoint(broadcast, PORT);

        //    s.SendTo(sendbuf, ep);

        //    Console.WriteLine("Message sent to the broadcast address");
        //}
    }
}