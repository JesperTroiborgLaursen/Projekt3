using RaspberryPiCore.LCD;

namespace DataAccesLogic.Drivers
{
    public class DisplayDriver : SerLCD
    {
        private SerLCD lcd;
        public DisplayDriver()
        {
            lcd = new SerLCD();
            lcd.lcdDisplay();
            lcd.lcdSetBackLight(238, 29, 203); //Makes color pink
            lcd.lcdSetContrast(0);
        }
    }
}