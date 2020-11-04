using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using DataAccessLogicCore.Boundaries;
using DomaineCore.Models;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;


namespace RPITest
{
    class Program
    {
        //Working test project. Possible to read ADC, pack it to SamplePack and broadcast with UDP and receive on the listener. 
        //Tested with Marcs RPI on Jespers Laptop, with HUAWEIP10 hotspot and Marcs computer for receiving with the project PRJ3UDPLIstener on nov. 4th 2020.

        private static ADC1015 adc;
        private static int PORT = 11000;
        private static string ip = "192.168.43.109"; //Standard networking broadcast IP
        private static SamplePack samplePack;
        private static List<Sample> ls;

        static void Main(string[] args)
        {

            adc = new ADC1015();

            ls = new List<Sample>();

            samplePack = new SamplePack();

            for (int i = 0; i < 50; i++)
            {
                ls.Add(new Sample() { Value = Convert.ToInt16(adc.readADC_SingleEnded(0))});
            }

            samplePack.SampleList = ls;
            samplePack.Date = DateTime.Now;
            samplePack.ID = 1;


            while (true)
            {
                Console.WriteLine("Ready to broadcast");
                BroadcastMessage(samplePack.ToString());
            }

            void BroadcastMessage(String message)
            {
                IPAddress broadcast = IPAddress.Parse(ip);
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                byte[] sendbuf = Encoding.ASCII.GetBytes(message);
                //byte[] sendbufhundred = new byte[100];
                //sendbuf.CopyTo(sendbufhundred,0);
                IPEndPoint ep = new IPEndPoint(broadcast, PORT);

                s.SendTo(sendbuf, ep);

                Console.WriteLine("Message sent to the broadcast address");
            }
        }
    }
}
