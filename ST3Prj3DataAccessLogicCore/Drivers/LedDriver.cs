using System.Device.Gpio;

namespace DataAccessLogic.Drivers
{
    public class LedDriver
    {
        private GpioController gpioController;
        private int gpio;
        public LedDriver(GpioController gpioController, int gpio)
        {
            this.gpioController = gpioController; ;
            this.gpio = gpio;
            gpioController.OpenPin(gpio,PinMode.Output);
        }

        public void on()
        {
            gpioController.Write(gpio,PinValue.High);
        }
        public void off()
        {
            gpioController.Write(gpio, PinValue.Low);
        }
    }
}