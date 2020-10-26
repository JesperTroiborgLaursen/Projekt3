using System;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
//using PresentationLogicCoreRPI.Boundaries;
using BusinessLogicCore.Controller; //Her findes den konkrete implementation af BL
using DataAccessLogicCore.Boundaries; //Her findes den konkrete implementation af DAL
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

        static void Main(string[] args)
        {
            _ = new Program();
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
