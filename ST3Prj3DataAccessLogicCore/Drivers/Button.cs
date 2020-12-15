using System.Device.Gpio;
using Interfaces;

namespace DataAccesLogic.Drivers
{
    public class Button : ButtonAbstract
    {
        public GpioController Controller { get; set; }
        public int buttonNumber { get; private set; }
        public Button(int buttonNumber)
        {
            switch (buttonNumber)
            {
                case 1:
                    Controller = new GpioController();
                    Controller.OpenPin(17, PinMode.Input);
                    this.buttonNumber = 17;
                    //Tested 23,24,25
                break;
                case 2:
                    Controller = new GpioController();
                    Controller.OpenPin(22, PinMode.Input);
                    this.buttonNumber = 22;
                    break;
                case 3:
                    Controller = new GpioController();
                    Controller.OpenPin(27, PinMode.Input);
                    this.buttonNumber = 27;
                    break;
                case 4:
                    Controller = new GpioController();
                    Controller.OpenPin(4, PinMode.Input);
                    this.buttonNumber = 4;
                    break;
            }
        }

        public bool IsPressed()
        {
            bool result = false;
            if (Controller.Read(buttonNumber) == PinValue.High)
            {
                result = false;
            }
            else if(Controller.Read(buttonNumber) == PinValue.Low)
            {
                result = true;
            }

            return result;
        }
    }
}