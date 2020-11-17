using System.Device.Gpio;
using Interfaces;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace DataAccesLogic.Drivers
{
    public class Button : ButtonAbstract
    {
        public GpioController Controller { get; set; }
        public int buttonNumber { get; private set; }
        public Button(int buttonNumber)
        {
            this.buttonNumber = 20 + buttonNumber;
            switch (buttonNumber)
            {
                case 1:
                    Controller = new GpioController();
                    Controller.OpenPin(21, PinMode.Input);
                break;
                case 2:
                    Controller = new GpioController();
                    Controller.OpenPin(22, PinMode.Input);
                    break;
                case 3:
                    Controller = new GpioController();
                    Controller.OpenPin(23, PinMode.Input);
                    break;
                case 4:
                    Controller = new GpioController();
                    Controller.OpenPin(24, PinMode.Input);
                    break;
            }
        }

        public bool IsPressed()
        {
            return (bool)Controller.Read(buttonNumber);
        }

    }
}