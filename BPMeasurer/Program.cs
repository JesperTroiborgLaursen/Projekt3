using System;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using RPI;


namespace BPMeasurer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            SerLCD lcd = new SerLCD();

            ADC1015 adc = new ADC1015();
            RPi rpi = new RPi();
            adc.readADC_Differential_2_3();

            ADC1115 adc2 = new



        }
    }
}
