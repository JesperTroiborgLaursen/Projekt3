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

        private static Measure measure;
        private static Broadcast broadcast;
        private static LCDProducer lcdProducer;
        private static WriteToLCD writeToLcd;
        private static LocalDB localDb;
        private static Thread mThread;
        private static Thread bThread;
        private static Thread lcdProducerThread;
        private static Thread writeToLcdThread;
        private static Thread saveToLocalDb;
        private static BlockingCollection<Broadcast_DTO> dataQueueBroadcast;
        private static BlockingCollection<LCD_DTO> dataQueueLCD;
        private static BlockingCollection<Measure_DTO> dataQueueMeasure;
        private static BlockingCollection<LocalDB_DTO> dataQueueLocalDb;
        private static ServiceCollection services;

        static void Main(string[] args)
        {
            //Setting Dependency Injection for DB context
            services = new ServiceCollection();
            services.AddDbContext<SamplePackDBContext>();

            //Creating dataQues
            dataQueueBroadcast = new BlockingCollection<Broadcast_DTO>();
            dataQueueLCD = new BlockingCollection<LCD_DTO>();
            dataQueueMeasure = new BlockingCollection<Measure_DTO>();
            dataQueueLocalDb = new BlockingCollection<LocalDB_DTO>();

            //Creating producers and consumers
            measure = new Measure(dataQueueBroadcast, dataQueueMeasure, dataQueueLocalDb);
            broadcast = new Broadcast(dataQueueBroadcast);

            lcdProducer = new LCDProducer(dataQueueLCD, dataQueueMeasure);
            writeToLcd = new WriteToLCD(dataQueueLCD);

            localDb = new LocalDB(dataQueueLocalDb);

            //Creating threads for producers and consumers
            mThread = new Thread(measure.Run);
            bThread = new Thread(broadcast.Run);
            lcdProducerThread = new Thread(lcdProducer.Run);
            writeToLcdThread = new Thread(writeToLcd.Run);
            saveToLocalDb = new Thread(localDb.Run);

            //Starting threads
            mThread.Start();
            bThread.Start();
            lcdProducerThread.Start();
            writeToLcdThread.Start();
            saveToLocalDb.Start();

        }

    }
}
