using RaspberryPiCore.LCD;

namespace DataAccesLogic.Drivers
{
    public class DisplayDriver : SerLCD
    {
        public SerLCD lcd { get; private set; }
        public DisplayDriver()
        {
            lcd = new SerLCD();
            lcd.lcdDisplay();
            lcd.lcdSetBackLight(238, 29, 203); //Sets color
            lcd.lcdSetContrast(0);
        }
    }
}