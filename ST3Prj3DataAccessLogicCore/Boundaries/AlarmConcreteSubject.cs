using DataAccesLogic.Drivers;
using Interfaces;

namespace DataAccessLogic.Boundaries
{
    public class AlarmConcreteSubject : AlarmSubject
    {
        public IAlarmObserver somo { get; set; }
        public AlarmConcreteSubject()
        {
            somo = new SomoAlarm();
            //LedAlarm led = new LedAlarm(); // Not impl.
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
