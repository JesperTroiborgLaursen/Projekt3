using System;
using System.Threading;
using DataAccesLogic.Drivers;
using Interfaces;

namespace Presentation
{
    public class UserInterface
    {

        public Button button1 { get; set; }
        public Button button2 { get; set; }
        public Button button3 { get; set; }
        public Button button4 { get; set; }


        public UserInterface()
        {
            //Creating buttons
            button1 = new Button(1);
            button2 = new Button(2);
            button3 = new Button(3);
            button4 = new Button(4);


            //Should set up WriteToLCD also
        }


        public void Run()
        {
            while (true)
            {
                if(button1.IsPressed())
                {
                    button1.Notify();
                    while (button1.IsPressed())
                    {
                        Thread.Sleep(1);
                    }
                    button1.Notify();
                    //Notify observers
                    
                    //Apply observer pattern.............

                }
                
            }
        }
    }
}