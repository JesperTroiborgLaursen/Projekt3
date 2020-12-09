using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLogic.Drivers;
using Interfaces;

namespace DataAccessLogic.Boundaries
{
    public class AlarmConcreteSubject : AlarmSubject
    {
        public AlarmConcreteSubject()
        {
            
        }

        public void NotifyAll(int priorityBP, int priorityBattery, int priorityPulse)
        {
            NotifyBP(priorityBP);
            NotifyBattery(priorityBattery);
            NotifyPulse(priorityPulse);
        }
    }
}
