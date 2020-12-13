using System.Collections.Concurrent;
using System.Threading;
using BusinessLogic.Controller;
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
        //private static LCDProducer lcdProducer;
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
        //private static Thread lcdProducerThread;
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
        //private static BlockingCollection<LCD_DTO> dataQueueStartUpLCD;
        //private static BlockingCollection<LCD_DTO> dataQueueCalibrationLCD;
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
        public static ManualResetEvent calibrationJoinEvent;
        


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

            //lcdProducer = new LCDProducer(dataQueueLCD, dataQueueAnalyzeLCD, calibrationEventLcd);

            writeToLcd = new WriteToLCD(dataQueueLCD, dataQueueAnalyzeLCD, calibrationEventLcd);

            localDb = new LocalDB(dataQueueLocalDb, calibrationEventLocalDb);

            batteryMeasureLogic = new BatteryMeasureLogic(dataQueueAdc,dataQueueBattery);

            alarmLogic = new AlarmLogic(dataQueueAlarm, dataQueueAlarmToBroadcast);

            analyzeLogic = new AnalyzeLogic(dataQueueAlarm,dataQueueAnalyze, dataQueueBattery,dataQueueAnalyzeLCD, dataQueueAnalyzeToBroadcast);

            startUp = new StartUp(dataQueueMeasure, dataQueueAdjustments, dataQueueMetaData,
                buttonObserver1, buttonObserver2,buttonObserver3,buttonObserver4, calibrationEventMeasure, dataQueueLCD);

            calibrationLogic= new CalibrationLogic(buttonObserver1, buttonObserver2,buttonObserver3,buttonObserver4,
                dataQueueLCD, calibrationEventLcd,calibrationEventMeasure,calibrationEventLocalDb,
                calibrationJoinEvent, dataQueueMeasure, dataQueueAdjustments, measure.ConvertingFactor);

      


            //Creating threads for producers and consumers
            measureThread = new Thread(measure.Run);
            broadcastThread = new Thread(broadcast.Run);
            //lcdProducerThread = new Thread(lcdProducer.Run);
            writeToLcdThread = new Thread(writeToLcd.Run);
            saveToLocalDb = new Thread(localDb.Run);
            uiThread = new Thread(ui.Run);
            calibrationThread = new Thread(calibrationLogic.Run);
            batteryMeasureThread = new Thread(batteryMeasureLogic.Run);
            alarmThread = new Thread(alarmLogic.Run);
            analyzeLogicThread = new Thread(analyzeLogic.Run);
            startUpThread = new Thread(startUp.Run);


            //Setting background Threads
            uiThread.IsBackground = true;
            batteryMeasureThread.IsBackground = true;
            writeToLcdThread.IsBackground = true;

            //For testing
            //SerLCD lcd = new SerLCD();
            //lcd.lcdDisplay();
            //lcd.lcdSetBackLight(238, 29, 203); //Makes color pink
            //lcd.lcdSetContrast(0);

            //while (true)
            //{
            //    lcd.lcdClear();
            //    lcd.lcdPrint("test");
            //    Thread.Sleep(500);
            //}



            ////Starting UI and battery measure threads
            uiThread.Start();
            batteryMeasureThread.Start();
            writeToLcdThread.Start();



            ////Starting startup and measure thread, and waiting for startUp to be done
            startUpThread.Start();
            measureThread.Start();
            startUpThread.Join();

            //Starting calibration thread which listens for button comb.
            //calibrationThread.Start();


            //Wait until start button is pressed and then measurement threads are started
            while (!buttonObserver2.IsPressed ) //&& calibrationJoinEvent.WaitOne() == true
            {
                Thread.Sleep(0);
            }

            //while (calibrationJoinEvent.WaitOne() == false)
            //{
            //    Thread.Sleep(0);
            //}


            broadcastThread.Start();
            //lcdProducerThread.Start();

            saveToLocalDb.Start();
            analyzeLogicThread.Start();
            alarmThread.Start();
            //writeToLcdThread.Join();


            //As long as stop button hasnt been pressed, threads are kept going
            while (!buttonObserver4.IsPressed)
            {
                Thread.Sleep(0);
                //while ()
                //{
                //    calibrationThread.Join();
                //}
            }

            //Shutting down threads by completing queues and setting stop props
            dataQueueLocalDb.CompleteAdding();
            dataQueueLCD.CompleteAdding();
            dataQueueBroadcast.CompleteAdding();
            dataQueueMeasure.CompleteAdding();
            analyzeLogic.Stop = true;
            alarmLogic.Stop = true;
            measure.Stop = true;

            //Havent been impl. fully
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
