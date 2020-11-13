using System.Collections.Concurrent;
using System.Threading;
using BussinessLogic.Controller;
using DataAccesLogic.Boundaries;
using Domain.Context;
using Domain.DTOModels;
using Microsoft.Extensions.DependencyInjection;
using Presentation;


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
        private static ServiceCollection services;

        static void Main(string[] args)
        {
            //Setting DB context
            services = new ServiceCollection();
            SamplePackDBContext context = new SamplePackDBContext();
            services.AddDbContext<SamplePackDBContext>();

            //Creating dataQues
            dataQueueBroadcast = new BlockingCollection<Broadcast_DTO>();
            dataQueueLCD = new BlockingCollection<LCD_DTO>();
            dataQueueMeasure = new BlockingCollection<Measure_DTO>();

            //Creating producers and consumers
            m = new Measure(dataQueueBroadcast, dataQueueMeasure);
            b = new Broadcast(dataQueueBroadcast);

            lcdProducer = new LCDProducer(dataQueueLCD, dataQueueMeasure);
            writeToLcd = new WriteToLCD(dataQueueLCD);

            //Creating threads for producers and consumers
            mThread = new Thread(m.Run);
            bThread = new Thread(b.Run);
            lcdProducerThread = new Thread(lcdProducer.Run);
            writeToLcdThread = new Thread(writeToLcd.Run);

            //Starting threads
            mThread.Start();
            bThread.Start();
            lcdProducerThread.Start();
            writeToLcdThread.Start();

        }

    }
}
