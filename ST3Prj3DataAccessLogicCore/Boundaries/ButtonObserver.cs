using System.Security.AccessControl;
using System.Threading;
using DataAccesLogic.Drivers;
using Interfaces;

namespace DataAccesLogic.Boundaries
{
    public class ButtonObserver : IButtonObserver
    {
        private bool isPressed = false;

        public bool startCal { get; set; }

        //public AutoResetEvent ready { get; private set; }

        public bool IsPressed
        {
            get { return isPressed; }
            set { isPressed = value; }
        }

        public ButtonObserver(Button button)
        {
            button.Attach(this);
            //ready = new AutoResetEvent(false);
        }
        public void Update()
        {
            if (IsPressed)
            {
                IsPressed = false;
                //ready.Set();
            }
            else
            {
                IsPressed = true;
                //ready.Set();
            }
        }

        public void UpdateCalibration()
        {
            startCal = true;
        }
    }
}