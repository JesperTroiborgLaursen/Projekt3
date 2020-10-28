using System;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
//using PresentationLogicCoreRPI.Boundaries;
using BusinessLogicCore.Controller; //Her findes den konkrete implementation af BL
using DataAccessLogicCore.Boundaries;
using DataAccessLogicCore.Data; //Her findes den konkrete implementation af DAL
using InterfacesCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; //Her er kontrakter/interfaces defineret


namespace RPIMain
{
    class Program
    {
        //Brug Interface referencer  
        //Alle objekter der er globalt brug og brugt mellem Logiklag opsættes her
        //Her kan overvejes en global systemdatamodel som så injectes i den forskellige lag
        //private IPresentationLogic icurrentGUIPL;
        //private IBusinessLogic icurrentBL;
        //private IDataAccessLogic icurrentDAL;
        private static ServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();//Start a DI Container (Dependency Injection Container)
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();

            //_ = new Program();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IDataAccessLogic, CtrlDataAccessLogic>(); //Add an instance a iDataAccesLogic implementaion to the DI Container
            services.AddSingleton<IBusinessLogic, BPLogic>(); //Add anusing ST3Prj3DataAccessLogicCore.Data; instance a iBusinessLogiciBusinessLogic implementaion to the DI Container
            services.AddDbContext<SamplePackDbContext>(options =>
            {
                options.UseSqlite("Data Source = SamplePack.db");//This options may be smarter to set in DbContext
            });

            //services.AddSingleton<MainWindow>();
        }

        public Program()
        {

            //Opsætning af referencer til implementationer af interfaces 
            //icurrentDAL = new CtrlDataAccessLogic();
            //icurrentBL = new BPLogic(icurrentDAL);
            //icurrentGUIPL = new SimpelCtrlRPIUI(icurrentBL);
            //Eller omstil til en anden for UI (User Interface)
            //icurrentGUIPL = new AnotherGUI(icurrentBL); 
            //icurrentGUIPL.startUpGUI();//Trin start applikation

        }
    }
}
