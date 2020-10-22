﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesCore;
using DomaineCore;

namespace BusinessLogicCore.Controller
{
    public class CalibrateLogic : iBusinessLogic
    {
        private IDataAccessLogic currentDal;
        /// <summary>
        /// In this constructor the DI instance may come from injection done in Main, the RPI version.
        /// Or comming from a DI Container, the WPF version 
        /// </summary>
        /// <param name="mydal"></param>
        public CalibrateLogic(IDataAccessLogic mydal) 
        {
            this.currentDal = mydal;
        }
        public void doAnAlogrithm()
        {
            int x = currentDal.getSomeData();
            x =x + 5;
            currentDal.saveSomeData(x);
        }

        

        int iBusinessLogic.DoAnAlogrithm()
        {
            int x = currentDal.getSomeData();
            x = x + 5;
            currentDal.saveSomeData(x);
            return x;
        }
    }
}
