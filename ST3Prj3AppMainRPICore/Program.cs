using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationLogicCoreRPI.Boundaries;
using BusinessLogicCore.Controller; //Her findes den konkrete implementation af BL
using DataAccessLogicCore.Boundaries; //Her findes den konkrete implementation af DAL
using InterfacesCore; //Her er kontrakter/interfaces defineret

namespace AppMainCore
{
    class Program
    {

        //Brug Interface referencer  
        //Alle objekter der er globalt brug og brugt mellem Logiklag opsættes her
        //Her kan overvejes en global systemdatamodel som så injectes i den forskellige lag
        private iPresentationLogic icurrentGUIPL;     
        private iBusinessLogic icurrentBL;       
        private iDataAccessLogic icurrentDAL;

        static void Main(string[] args)
        {
            _ = new Program();
        }

        public Program()
        {
                       
            //Opsætning af referencer til implementationer af interfaces 
            icurrentDAL = new CtrlDataAccessLogic(); 
            icurrentBL = new CtrlBusinessLogic(icurrentDAL);
            icurrentGUIPL = new  SimpelCtrlRPIUI(icurrentBL);
            //Eller omstil til en anden for UI (User Interface)
            //icurrentGUIPL = new AnotherGUI(icurrentBL); 
            icurrentGUIPL.startUpGUI();//Trin start applikation

        }
    }
}
