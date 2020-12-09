using RaspberryPiCore.LCD;

namespace DataAccesLogic.Drivers
{
    public class DisplayDriver : SerLCD
    {
        //public SerLCD lcd;
        public SerLCD lcd { get; private set; }
        public DisplayDriver()
        {
            lcd = new SerLCD();
            lcd.lcdDisplay();
            lcd.lcdSetBackLight(238, 29, 203); //Makes color pink
            lcd.lcdSetContrast(0);
        }
    }
}