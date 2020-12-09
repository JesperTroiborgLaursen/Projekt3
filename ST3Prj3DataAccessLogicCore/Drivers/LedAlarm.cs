using System;
using System.Collections.Generic;
using System.Text;
using Interfaces;
using RPI;

namespace DataAccessLogic.Drivers
{
    public class LedAlarm : IAlarmObserver
    {
        //To typer af alarm, visuel og sensuel

        //Led som alarm

        //Somo lyd som alarm
        public LedAlarm()
        {

        }

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
