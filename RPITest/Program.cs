﻿using System.Collections.Concurrent;
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
        private static UserInterface ui;
        private static CalibrationLogic calibrationLogic;
        private static BatteryMeasureLogic batteryMeasureLogic;
        private static AnalyseLogic analyseLogic;
        private static AlarmLogic alarmLogic;
        private static StartUp startUp;

        private static Thread measureThread;
        private static Thread broadcastThread;
        private static Thread lcdProducerThread;
        private static Thread writeToLcdThread;
        private static Thread saveToLocalDb;
        private static Thread uiThread;
        private static Thread calibrationThread;
        private static Thread batteryMeasureThread;
        private static Thread analyzeLogicThread;
        private static Thread alarmThread;
        private static Thread startUpThread;

        private static BlockingCollection<Broadcast_DTO> dataQueueBroadcast;
        private static BlockingCollection<LCD_DTO> dataQueueLCD;
        private static BlockingCollection<Measure_DTO> dataQueueMeasure;
        private static BlockingCollection<LocalDB_DTO> dataQueueLocalDb;
        private static BlockingCollection<ADC_DTO> dataQueueAdc;
        private static BlockingCollection<Battery_DTO> dataQueueBattery;
        private static BlockingCollection<Analyze_DTO> dataQueueAnalyze;
        private static BlockingCollection<Analyze_DTO> dataQueueAnalyzeLCD;
        private static BlockingCollection<Adjustments_DTO> dataQueueAdjustments;

        private static ServiceCollection services;
        
        private static ButtonObserver buttonObserver1;
        private static ButtonObserver buttonObserver2;
        private static ButtonObserver buttonObserver3;
        private static ButtonObserver buttonObserver4;


        private static DisplayDriver lcd;
       

        public static ManualResetEvent calibrationEventLcd { get; set; }
        public static ManualResetEvent calibrationEventMeasure { get; set; }
        public static ManualResetEvent calibrationEventLocalDb { get; set; }
        public static ManualResetEvent calibrationJoinEvent;

        static void Main(string[] args)
        {

            //Setting Dependency Injection for DB context
            services = new ServiceCollection();
            services.AddDbContext<SamplePackDBContext>();


            //Creating display
            lcd = new DisplayDriver();
            //Creating UserInterface
            ui = new UserInterface();

            //Creating calibrationevent
            calibrationEventLcd = new ManualResetEvent(true);
            calibrationEventMeasure = new ManualResetEvent(true);
            calibrationEventLocalDb = new ManualResetEvent(true);
            calibrationJoinEvent = new ManualResetEvent(false);

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
            dataQueueAdc = new BlockingCollection<ADC_DTO>();
            dataQueueBattery = new BlockingCollection<Battery_DTO>();
            dataQueueAnalyze = new BlockingCollection<Analyze_DTO>();
            dataQueueAnalyzeLCD = new BlockingCollection<Analyze_DTO>();
            dataQueueAdjustments = new BlockingCollection<Adjustments_DTO>();


            
            //Creating objects
            measure = new Measure(dataQueueBroadcast, dataQueueMeasure, dataQueueLocalDb, dataQueueAdc,
                dataQueueAdjustments, calibrationEventMeasure);

            broadcast = new Broadcast(dataQueueBroadcast);

            lcdProducer = new LCDProducer(dataQueueLCD, dataQueueAnalyzeLCD, calibrationEventLcd);

            writeToLcd = new WriteToLCD(dataQueueLCD, calibrationEventLcd, lcd);

            localDb = new LocalDB(dataQueueLocalDb, calibrationEventLocalDb);

            batteryMeasureLogic = new BatteryMeasureLogic(dataQueueAdc,dataQueueBattery);

            alarmLogic = new AlarmLogic(dataQueueAnalyze);

            analyseLogic = new AnalyseLogic(dataQueueAnalyze,dataQueueMeasure, dataQueueBattery,dataQueueAnalyzeLCD);

            startUp = new StartUp(lcd, dataQueueMeasure, dataQueueAdjustments, 
                buttonObserver1, buttonObserver2,buttonObserver3,buttonObserver4, calibrationEventMeasure);

            calibrationLogic= new CalibrationLogic(buttonObserver1, buttonObserver2,buttonObserver3,buttonObserver4,
                dataQueueLCD, calibrationEventLcd,calibrationEventMeasure,calibrationEventLocalDb,
                calibrationJoinEvent, dataQueueMeasure, lcd, dataQueueAdjustments, measure.ConvertingFactor);

      


            //Creating threads for producers and consumers
            measureThread = new Thread(measure.Run);
            broadcastThread = new Thread(broadcast.Run);
            lcdProducerThread = new Thread(lcdProducer.Run);
            writeToLcdThread = new Thread(writeToLcd.Run);
            saveToLocalDb = new Thread(localDb.Run);
            uiThread = new Thread(ui.Run);
            calibrationThread = new Thread(calibrationLogic.Run);
            batteryMeasureThread = new Thread(batteryMeasureLogic.Run);
            alarmThread = new Thread(alarmLogic.Run);
            analyzeLogicThread = new Thread(analyseLogic.Run);
            startUpThread = new Thread(startUp.Run);



            //Setting background Threads
            uiThread.IsBackground = true;
            calibrationThread.IsBackground = true;
            

            //Starting UI and battery measure threads
            uiThread.Start();
            batteryMeasureThread.Start();

            //Starting startup thread, and waiting for it to be done
            startUpThread.Start();
            startUpThread.Join();

            //Starting calibration
            calibrationThread.Start();

            //Wait until start button is pressed and then measurement threads are started
            while (!buttonObserver2.IsPressed)
            {
                Thread.Sleep(0);
            }


            measureThread.Start();
            broadcastThread.Start();
            lcdProducerThread.Start();
            writeToLcdThread.Start();
            saveToLocalDb.Start();
            //analyzeLogicThread.Start();
            //alarmThread.Start();
            
            
            //As long as stop button hasnt been pressed, threads are kept going
            while (!buttonObserver4.IsPressed)
            {
                Thread.Sleep(0);
                while (calibrationJoinEvent.WaitOne())
                {
                    calibrationThread.Join();
                }
            }

            //Shutting down threads by completing queues
            dataQueueLocalDb.CompleteAdding();
            dataQueueLCD.CompleteAdding();
            dataQueueBroadcast.CompleteAdding();
            dataQueueMeasure.CompleteAdding();
            analyseLogic.Stop = true;
            alarmLogic.Stop = true;
            measure.Stop = true;


            ////Start again after stopped measure
            //while (true)
            //{
            //    while (!buttonObserver1.IsPressed)
            //    {
            //        Thread.Sleep(0);
            //    }

            //    dataQueueLocalDb.;
            //    dataQueueLCD.CompleteAdding();
            //    dataQueueBroadcast.CompleteAdding();
            //    dataQueueMeasure.CompleteAdding();

            //    calibrationThread.Start();
            //    measureThread.Start();
            //    broadcastThread.Start();
            //    lcdProducerThread.Start();
            //    writeToLcdThread.Start();
            //    saveToLocalDb.Start();

            //    //As long as stop button hasnt been pressed, threads are kept going
            //    while (!buttonObserver4.IsPressed)
            //    {
            //        Thread.Sleep(0);
            //    }

            //    //Shutting down threads by completing queues
            //    dataQueueLocalDb.CompleteAdding();
            //    dataQueueLCD.CompleteAdding();
            //    dataQueueBroadcast.CompleteAdding();
            //    dataQueueMeasure.CompleteAdding();
            //    measure.Stop = true;



            //}

            //uiThread.Join();
        }

        
    }
}
