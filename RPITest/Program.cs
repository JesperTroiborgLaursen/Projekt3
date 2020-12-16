using System.Collections.Concurrent;
using System.Threading;
using BusinessLogic.Controller;
using DataAccesLogic.Boundaries;
using Domain.Context;
using Domain.DTOModels;
using Microsoft.Extensions.DependencyInjection;
using Presentation;


namespace RPIMain
{
    class Program
    {
        private static Measure measure;
        private static Broadcast broadcast;
        private static WriteToLCD writeToLcd;
        private static LocalDB localDb;
        private static UserInterface ui;
        private static CalibrationLogic calibrationLogic;
        private static BatteryMeasureLogic batteryMeasureLogic;
        private static AnalyzeLogic analyzeLogic;
        private static AlarmLogic alarmLogic;
        private static StartUp startUp;

        private static Thread measureThread;
        private static Thread broadcastThread;
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
        private static BlockingCollection<Analyze_DTO> dataQueueAlarm;
        private static BlockingCollection<Analyze_DTO> dataQueueAnalyzeLCD;
        private static BlockingCollection<Measure_DTO> dataQueueAnalyze;
        private static BlockingCollection<Adjustments_DTO> dataQueueAdjustments;
        private static BlockingCollection<MetaData_DTO> dataQueueMetaData;
        private static BlockingCollection<Alarm_DTO> dataQueueAlarmToBroadcast;
        private static BlockingCollection<Analyze_DTO> dataQueueAnalyzeToBroadcast;

        private static ServiceCollection services;
        
        private static ButtonObserver buttonObserver1;
        private static ButtonObserver buttonObserver2;
        private static ButtonObserver buttonObserver3;
        private static ButtonObserver buttonObserver4;

        public static ManualResetEvent calibrationEventLcd { get; set; }
        public static ManualResetEvent calibrationEventMeasure { get; set; }
        public static ManualResetEvent calibrationEventLocalDb { get; set; }



        static void Main(string[] args)
        {
            //Setting Dependency Injection for DB context
            services = new ServiceCollection();
            services.AddDbContext<SamplePackDBContext>();

            //Creating UserInterface
            ui = new UserInterface();

            //Creating calibrationevent
            calibrationEventLcd = new ManualResetEvent(true);
            calibrationEventMeasure = new ManualResetEvent(true);
            calibrationEventLocalDb = new ManualResetEvent(true);

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
            dataQueueAlarm = new BlockingCollection<Analyze_DTO>();
            dataQueueAnalyze = new BlockingCollection<Measure_DTO>();
            dataQueueAnalyzeLCD = new BlockingCollection<Analyze_DTO>();
            dataQueueAdjustments = new BlockingCollection<Adjustments_DTO>();
            dataQueueMetaData = new BlockingCollection<MetaData_DTO>();
            dataQueueAnalyzeToBroadcast = new BlockingCollection<Analyze_DTO>();
            dataQueueAlarmToBroadcast = new BlockingCollection<Alarm_DTO>();

            //Creating objects
            measure = new Measure(dataQueueBroadcast, dataQueueMeasure, dataQueueLocalDb, dataQueueAdc,
                dataQueueAdjustments, dataQueueAnalyze, calibrationEventMeasure);

            broadcast = new Broadcast(dataQueueBroadcast, dataQueueMetaData, dataQueueAlarmToBroadcast, dataQueueAnalyzeToBroadcast);

            writeToLcd = new WriteToLCD(dataQueueLCD, dataQueueAnalyzeLCD, calibrationEventLcd);

            localDb = new LocalDB(dataQueueLocalDb);

            batteryMeasureLogic = new BatteryMeasureLogic(dataQueueAdc,dataQueueBattery);

            alarmLogic = new AlarmLogic(dataQueueAlarm, dataQueueAlarmToBroadcast);

            analyzeLogic = new AnalyzeLogic(dataQueueAlarm,dataQueueAnalyze, dataQueueBattery,dataQueueAnalyzeLCD, dataQueueAnalyzeToBroadcast);

            startUp = new StartUp(dataQueueMeasure, dataQueueAdjustments, dataQueueMetaData,
                buttonObserver1, buttonObserver2, buttonObserver3, buttonObserver4, calibrationEventMeasure, dataQueueLCD);

            calibrationLogic= new CalibrationLogic(buttonObserver1, buttonObserver2,buttonObserver3,buttonObserver4,
                dataQueueLCD, calibrationEventLcd, calibrationEventMeasure,
                dataQueueMeasure, dataQueueAdjustments, measure.ConvertingFactor);

            //Creating threads for producers and consumers
            measureThread = new Thread(measure.Run);
            broadcastThread = new Thread(broadcast.Run);
            writeToLcdThread = new Thread(writeToLcd.Run);
            saveToLocalDb = new Thread(localDb.Run);
            uiThread = new Thread(ui.Run);
            calibrationThread = new Thread(calibrationLogic.Run);
            batteryMeasureThread = new Thread(batteryMeasureLogic.Run);
            alarmThread = new Thread(alarmLogic.Run);
            analyzeLogicThread = new Thread(analyzeLogic.Run);
            startUpThread = new Thread(startUp.Run);


            ///Test det her før slet!
            ///
            ///
            ///
            ///NB
            ///
            ///NB
            ///
            /// NB
            ////Setting background Threads
            //uiThread.IsBackground = true;
            //batteryMeasureThread.IsBackground = true;
            //writeToLcdThread.IsBackground = true;
            //alarmThread.IsBackground = true;
            
            //Starting UI and battery measure threads
            uiThread.Start();
            batteryMeasureThread.Start();
            writeToLcdThread.Start();

            //Starting startup and measure thread, and waiting for startUp to be done. Securing with reset event,
            //that measure don't start measuring before needed
            startUpThread.Start();
            calibrationEventMeasure.Set();
            measureThread.Start();
            startUpThread.Join();

            //Starting calibration thread which listens for button comb.
            calibrationThread.Start();
            
            //Wait until start button is pressed and then measurement threads are started. If Calibration has been started, wait for it to be done
            while (!buttonObserver2.IsPressed )
            {
                while (buttonObserver3.startCal)
                {
                    Thread.Sleep(0);
                }
                Thread.Sleep(0);
            }

            calibrationEventMeasure.Reset();
            broadcastThread.Start();
            saveToLocalDb.Start();
            analyzeLogicThread.Start();
            alarmThread.Start(); 
            


            //As long as stop button hasnt been pressed, threads are kept going
            while (!buttonObserver4.IsPressed)
            {
                Thread.Sleep(0);
                
            }

            //Shutting down threads and completing queues
            dataQueueLocalDb.CompleteAdding();
            dataQueueLCD.CompleteAdding();
            dataQueueBroadcast.CompleteAdding();
            dataQueueMeasure.CompleteAdding();
            dataQueueAdc.CompleteAdding();
            dataQueueBattery.CompleteAdding();
            dataQueueAlarm.CompleteAdding();
            dataQueueAnalyze.CompleteAdding();
            dataQueueAnalyzeLCD.CompleteAdding();
            dataQueueAdjustments.CompleteAdding();
            dataQueueMetaData.CompleteAdding();
            dataQueueAnalyzeToBroadcast.CompleteAdding();
            dataQueueAlarmToBroadcast.CompleteAdding();

            measure.Stop = true;
            broadcast.Stop = true;
            writeToLcd.Stop = true;
            localDb.Stop = true;
            batteryMeasureLogic.Stop = true;
            alarmLogic.Stop = true;
            analyzeLogic.Stop = true;
            calibrationLogic.Stop = true;



            //Start again after stopped measure -> havent been fully impl.
            //CompleteAdding() shouldn't be called on queues before completely shutting down program.

            //Wait until start button is pressed and then measurement threads are started again. If Calibration has been started, wait for it to be done
            //while (!buttonObserver2.IsPressed )
            //{
            //    while (buttonObserver3.startCal)
            //    {
            //        Thread.Sleep(0);
            //    }
            //    Thread.Sleep(0);
            //}


            //measure.Stop = false;
            //broadcast.Stop = false;
            //writeToLcd.Stop = false;
            //localDb.Stop = false;
            //batteryMeasureLogic.Stop = false;
            //alarmLogic.Stop = false;
            //analyzeLogic.Stop = false;
            //calibrationLogic.Stop = false;



            ////As long as stop button hasnt been pressed, threads are kept going
            //while (!buttonObserver4.IsPressed)
            //{
            //    Thread.Sleep(0);

            //}

            ////Shutting down threads and completing queues
            //dataQueueLocalDb.CompleteAdding();
            //dataQueueLCD.CompleteAdding();
            //dataQueueBroadcast.CompleteAdding();
            //dataQueueMeasure.CompleteAdding();
            //dataQueueAdc.CompleteAdding();
            //dataQueueBattery.CompleteAdding();
            //dataQueueAlarm.CompleteAdding();
            //dataQueueAnalyze.CompleteAdding();
            //dataQueueAnalyzeLCD.CompleteAdding();
            //dataQueueAdjustments.CompleteAdding();
            //dataQueueMetaData.CompleteAdding();
            //dataQueueAnalyzeToBroadcast.CompleteAdding();
            //dataQueueAlarmToBroadcast.CompleteAdding();

            //measure.Stop = true;
            //broadcast.Stop = true;
            //writeToLcd.Stop = true;
            //localDb.Stop = true;
            //batteryMeasureLogic.Stop = true;
            //alarmLogic.Stop = true;
            //analyzeLogic.Stop = true;
            //calibrationLogic.Stop = true;
        }

        
    }
}
