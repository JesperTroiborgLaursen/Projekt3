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

        }

        public int priority { get; set; }

        public void UpdateBattery(int priority)
        {
            throw new NotImplementedException();
        }

        public void UpdateBP(int priority)
        {
            throw new NotImplementedException();
        }

        public void UpdatePulse(int priority)
        {
            throw new NotImplementedException();
        }
    }
}
