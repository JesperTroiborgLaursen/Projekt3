using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLogic.Drivers;
using Interfaces;

namespace DataAccessLogic.Boundaries
{
    public class AlarmObserver : IAlarmObserver
    {
        public AlarmObserver(Alarm alarm)
        {
            alarm.Attach(this);
        }
        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
