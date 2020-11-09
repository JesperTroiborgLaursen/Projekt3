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
using Thread = System.Threading.Thread;


namespace RPITest
{
    class Program
    {
        //Working test project. Possible to read ADC, pack it to SamplePack and broadcast with UDP and receive on the listener. 
        //Tested with Marcs RPI on Jespers Laptop, with HUAWEIP10 hotspot and Marcs computer for receiving with the project PRJ3UDPLIstener on nov. 4th 2020.

        private static Measure m;
        private static Broadcast b;
        private static LCDProducer lcdProducer;
        private static WriteToLCD writeToLcd;
        private static Thread mThread;
        private static Thread bThread;
        private static Thread lcdProducerThread;
        private static Thread writeToLcdThread;
        private static BlockingCollection<Broadcast_DTO> dataQueueBroadcast;
        private static BlockingCollection<LCD_DTO> dataQueueLCD;
        private static BlockingCollection<Measure_DTO> dataQueueMeasure;

        static void Main(string[] args)
        {
            dataQueueBroadcast = new BlockingCollection<Broadcast_DTO>();
            dataQueueLCD = new BlockingCollection<LCD_DTO>();
            dataQueueMeasure = new BlockingCollection<Measure_DTO>();

            m = new Measure(dataQueueBroadcast, dataQueueMeasure);
            b = new Broadcast(dataQueueBroadcast);

            lcdProducer = new LCDProducer(dataQueueLCD, dataQueueMeasure);
            writeToLcd = new WriteToLCD(dataQueueLCD);

            mThread = new Thread(m.Run);
            bThread = new Thread(b.Run);
            lcdProducerThread = new Thread(lcdProducer.Run);
            writeToLcdThread = new Thread(writeToLcd.Run);

            mThread.Start();
            bThread.Start();
            lcdProducerThread.Start();
            writeToLcdThread.Start();

        }

    }
}
