using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public abstract class AlarmAbstract
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

        public void Notify()
        {
            foreach (var observer in _alarmObservers)
            {
                observer.Update();
            }
        }
    }
}
