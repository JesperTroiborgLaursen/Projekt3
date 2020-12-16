namespace Interfaces
{
    public interface IAlarmObserver
    {
        public int priority { get; set; }
        public void UpdateBattery(int priority);
        public void UpdateBP(int priority);
        public void UpdatePulse(int priority);
    }
}
