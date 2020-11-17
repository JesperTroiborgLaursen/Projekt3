using System.Collections.Generic;

namespace Interfaces
{
    public class ButtonAbstract
    {
        private List<IButtonObserver> _observers = new List<IButtonObserver>();

        public void Attach(IButtonObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IButtonObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
        
    }
}