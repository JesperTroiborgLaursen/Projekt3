using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public abstract class AlarmSubject
    {
        private List<IAlarmObserver> _alarmObservers = new List<IAlarmObserver>();

        public void Attach(IAlarmObserver observer)
        {
            _alarmObservers.Add(observer);
        }

        public void Detach(IAlarmObserver observer)
        {
            _alarmObservers.Remove(observer);
        }

        public void NotifyBattery(int priority)
        {
            foreach (var observer in _alarmObservers)
            {
                observer.UpdateBattery(priority);
            }
        }
        public void NotifyBP(int priority)
        {
            foreach (var observer in _alarmObservers)
            {
                observer.UpdateBP(priority);
            }
        }

        public void NotifyPulse(int priority)
        {
            foreach (var observer in _alarmObservers)
            {
                observer.UpdatePulse(priority);
            }
        }

    }
}
