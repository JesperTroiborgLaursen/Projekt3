using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;//Tilføjes
using InterfacesCore;


namespace PresentationLogicCoreRPI.Boundaries

{
    public class SimpelCtrlRPIUI : iPresentationLogic
    {
        private iBusinessLogic currentBL;
        public SimpelCtrlRPIUI(iBusinessLogic mybl)
        {
            this.currentBL = mybl;
           
        }
        public void startUpGUI()
        {
            currentBL.doAnAlogrithm();
        }
    }
    public class CtrlWPFGUI : iPresentationLogic
    {
        private iBusinessLogic currentBL;
        public CtrlWPFGUI(iBusinessLogic mybl)
        {
            this.currentBL = mybl;
        }

        [STAThread]  //Tilføjes 
        public void startUpGUI()
        {
            //Application.EnableVisualStyles(); //Tilføjes
            //Application.SetCompatibleTextRenderingDefault(false); //Tiføjes
            //Application.DoEvents();// Tilføjes
            //Application.Run(new FormRunDAQ(currentBL)); //Tilføjes !!Dobbelt Dependency Injection!!
            //                                            //currentBL.doAnAlogrithm(); Kaldet er nu flyttet/delegeret til Windows Form.

        }
    }

    public class AnotherGUI : iPresentationLogic
    {
        public void startUpGUI()
        {
            throw new NotImplementedException();
        }
    }
}
