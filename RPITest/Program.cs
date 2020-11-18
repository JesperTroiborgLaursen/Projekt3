using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using BusinessLogic.Controller;
using BussinessLogic.Controller;
using DataAccesLogic.Boundaries;
using DataAccesLogic.Drivers;
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
        private static Thread uiThread;
        private static Thread calibrationThread;
        private static BlockingCollection<Broadcast_DTO> dataQueueBroadcast;
        private static BlockingCollection<LCD_DTO> dataQueueLCD;
        private static BlockingCollection<Measure_DTO> dataQueueMeasure;
        private static BlockingCollection<LocalDB_DTO> dataQueueLocalDb;
        private static ServiceCollection services;
        private static UserInterface ui;
        private static ButtonObserver buttonObserver1;
        private static ButtonObserver buttonObserver2;
        private static ButtonObserver buttonObserver3;
        private static ButtonObserver buttonObserver4;
        private static CalibrationLogic calibrationLogic;

        static void Main(string[] args)
        {
            //Setting Dependency Injection for DB context
            services = new ServiceCollection();
            services.AddDbContext<SamplePackDBContext>();

            //Creating UserInterface
            ui = new UserInterface();

            //Create Observers
            buttonObserver1 = new ButtonObserver(ui.button1);
            buttonObserver2 = new ButtonObserver(ui.button2);
            buttonObserver3 = new ButtonObserver(ui.button3);
            buttonObserver4 = new ButtonObserver(ui.button4);

            //Creating dataQues
            dataQueueBroadcast = new BlockingCollection<Broadcast_DTO>();
            dataQueueLCD = new BlockingCollection<LCD_DTO>();
            dataQueueMeasure = new BlockingCollection<Measure_DTO>();
            dataQueueLocalDb = new BlockingCollection<LocalDB_DTO>();

            //Create Calibration
            calibrationLogic= new CalibrationLogic(buttonObserver1, buttonObserver2, buttonObserver3, buttonObserver4, dataQueueLCD);

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
            uiThread = new Thread(ui.Run);
            //uiThread.IsBackground = true;
            calibrationThread = new Thread(calibrationLogic.Calibrate);

            //Starting threads
            mThread.Start();
            bThread.Start();
            lcdProducerThread.Start();
            writeToLcdThread.Start();
            saveToLocalDb.Start();
            uiThread.Start();
            calibrationThread.Start();

            //uiThread.Join();
        }

    }
}
