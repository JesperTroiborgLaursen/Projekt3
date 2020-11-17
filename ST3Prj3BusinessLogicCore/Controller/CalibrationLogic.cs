using DataAccesLogic.Boundaries;
using DataAccesLogic.Drivers;
using Interfaces;

namespace BusinessLogic.Controller
{
    public class CalibrationLogic
    {
        private static ButtonObserver _button1;
        private static ButtonObserver _button2;
        private static ButtonObserver _button3;
        private static ButtonObserver _button4;


        public CalibrationLogic(ButtonObserver buttonObserver1, ButtonObserver buttonObserver2, ButtonObserver buttonObserver3, ButtonObserver buttonObserver4)
        {
            _button1 = buttonObserver1;
            _button2 = buttonObserver2;
            _button3 = buttonObserver3;
            _button4 = buttonObserver4;
        }

        public void Calibrate()
        {
            _button1.Update();
        }
    }
}