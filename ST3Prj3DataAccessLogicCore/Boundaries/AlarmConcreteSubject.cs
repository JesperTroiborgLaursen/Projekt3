using System;
using System.Collections.Generic;
using System.Text;
using DataAccesLogic.Drivers;
using DataAccessLogic.Drivers;
using Interfaces;

namespace DataAccessLogic.Boundaries
{
    public class AlarmConcreteSubject : AlarmSubject
    {
        public AlarmConcreteSubject()
        {
            SomoAlarm somo = new SomoAlarm();
            //LedAlarm led = new LedAlarm();
            Attach(somo);
            //Attach(led);
        }

        public void NotifyAll(int priorityBP, int priorityBattery, int priorityPulse)
        {
            NotifyBP(priorityBP);
            NotifyBattery(priorityBattery);
            NotifyPulse(priorityPulse);
        }
    }
}
