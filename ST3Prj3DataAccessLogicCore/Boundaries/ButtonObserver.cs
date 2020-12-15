using DataAccesLogic.Drivers;
using Interfaces;

namespace DataAccesLogic.Boundaries
{
    public class ButtonObserver : IButtonObserver
    {
        private bool isPressed = false;

        public bool startCal { get; set; }

        public bool IsPressed
        {
            get { return isPressed; }
            set { isPressed = value; }
        }

        public ButtonObserver(Button button)
        {
            button.Attach(this);
        }
        public void Update()
        {
            if (IsPressed)
            {
                IsPressed = false;
            }
            else
            {
                IsPressed = true;
            }
        }

        public void UpdateCalibration()
        {
            startCal = true;
        }
    }
}