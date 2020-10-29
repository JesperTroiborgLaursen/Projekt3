using System;
using System.Linq;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using PresentationLogicCoreRPI.Boundaries;
using BusinessLogicCore.Controller; //Her findes den konkrete implementation af BL
using DataAccessLogicCore.Boundaries;
using DomaineCore.Services;
using DomaineCore.Data; //Her findes den konkrete implementation af DAL
using InterfacesCore; //Her er kontrakter/interfaces defineret


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
        private static SamplePackDbContextFactory factory;

        static void Main(string[] args)
        {
            // Configuration/opsætning
            //var builder = new ConfigurationBuilder(); flyttet til factory
            //var configuration = builder.Build();
            _ = new Program();

            var SPDBcontext = factory.CreateContext("DefaultConnection");

            SPDBcontext.Database.EnsureCreated();

            SPDBcontext.Database.CanConnectAsync(); //oprette forbindelse testes her


            //// Database impl. flyttet til ContextFactory

            //SPDBcontext.SamplePacks.Add(); Tilføj samplepacks til db her


            //var emneriDBCount = SPDBcontext.SamplePacks.Count(); //brug den her i debugger til at se om der ligger filer i DB? Den bruges ikke til andet lige her.


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
            factory = new SamplePackDbContextFactory();

        }
    }
}
