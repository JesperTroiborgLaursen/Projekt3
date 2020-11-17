using DataAccesLogic.Drivers;
using Interfaces;

namespace DataAccesLogic.Boundaries
{
    public class ButtonObserver : IButtonObserver
    {
        public ButtonObserver(Button button)
        {
            button.Attach(this);
        }
        public bool Update()
        {
            return true;
        }
    }
}