using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

        private static Measure m;
        private static Broadcast b;
        private static Thread mThread;
        private static Thread bThread;
        private static BlockingCollection<DataContainer> dataQueue;

        static void Main(string[] args)
        {
            dataQueue = new BlockingCollection<DataContainer>();
            m = new Measure(dataQueue);
            b = new Broadcast(dataQueue);

            mThread = new Thread(m.Run);
            bThread = new Thread(b.Run);

            mThread.Start();
            bThread.Start();



            //while (true)
            //{
            //    samplePack = new SamplePack();
            //    samplePack = m.StartMeasurement();
            //    Console.WriteLine("Ready to broadcast");
            //    b.BroadcastMessage(samplePack.ToString());
            //}
        }

    }
}
