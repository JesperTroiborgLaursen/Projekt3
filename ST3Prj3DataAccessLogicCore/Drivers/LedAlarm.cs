using System;
using System.Collections.Generic;
using System.Text;
using Interfaces;
using RPI;

namespace DataAccessLogic.Drivers
{
    public class LedAlarm : IAlarmObserver
    {
        //Not implemented
        public LedAlarm()
        {
            //LedDriver led1 = new LedDriver();
            //LedDriver led2 = new LedDriver();
        }

        public int priority { get; set; }

        public void UpdateBattery(int priority)
        {
            //Impl. of LED flashing combination based on priority
            throw new NotImplementedException();
        }

        public void UpdateBP(int priority)
        {
            //Impl. of LED flashing combination based on priority
            throw new NotImplementedException();
        }

        public void UpdatePulse(int priority)
        {
            //Impl. of LED flashing combination based on priority
            throw new NotImplementedException();
        }
    }
}
