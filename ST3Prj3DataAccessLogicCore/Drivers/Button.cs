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
            switch (buttonNumber)
            {
                case 1:
                    Controller = new GpioController();
                    Controller.OpenPin(9, PinMode.Input);
                    this.buttonNumber = 9;
                break;
                case 2:
                    Controller = new GpioController();
                    Controller.OpenPin(25, PinMode.Input);
                    this.buttonNumber = 25;
                    break;
                case 3:
                    Controller = new GpioController();
                    Controller.OpenPin(11, PinMode.Input);
                    this.buttonNumber = 11;
                    break;
                case 4:
                    Controller = new GpioController();
                    Controller.OpenPin(8, PinMode.Input);
                    this.buttonNumber = 8;
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