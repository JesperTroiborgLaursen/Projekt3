using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;//Tilføjes
using InterfacesCore;


namespace PresentationLogicCoreRPI.Boundaries

{
    public class SimpelCtrlRPIUI : IPresentationLogic
    {
        private IBusinessLogic currentBL;
        public SimpelCtrlRPIUI(IBusinessLogic mybl)
        {
            this.currentBL = mybl;
           
        }
        public void startUpGUI()
        {
            currentBL.doAnAlogrithm();
        }
    }
}
