using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using BusinessLogicCore.Controller;
using DataAccessLogicCore.Boundaries;
using DomaineCore.Data;
using DomaineCore.Models;

namespace TestConsoleProject
{
    class Program
    {
        private const int PORT = 11000;
        private const string ip = "192.168.43.255"; //Standard networking broadcast IP  

        static void Main(string[] args)
        {
            //BroadcastLogic broadcastLogic = new BroadcastLogic();

            //BPLogic bpLogic = new BPLogic();

            //broadcastLogic.BroadcastSamplePack(bpLogic.ReadAdc());
            //while (true)
            //{
            //    Console.WriteLine("Ready to broadcast");
            //    BroadcastMessage(Console.ReadLine());
            //}

            //void BroadcastMessage(String message)
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


                //db.Add(new SamplePack() {Date = DateTime.Now, ID = 1});
                //Console.WriteLine("samplepack created");
            


        }
    }
}
