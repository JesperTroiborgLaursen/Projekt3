using System;
using System.Threading;
using Interfaces;
using Button = DataAccesLogic.Drivers.Button;

namespace Presentation
{
    public class UserInterface
    {
        private bool stop = true;

        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }


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
             
                if (button1.IsPressed())
                {
                    button1.Notify();
                    while (button1.IsPressed())
                    {
                        Thread.Sleep(0);
                    }

                    button1.Notify();
                }

                if (button2.IsPressed())
                {
                    button2.Notify();
                    int i = 0;
                    bool startcal = false;
                    while (button2.IsPressed() && !startcal)
                    {
                        i = 0;
                        while (button3.IsPressed() && button2.IsPressed())
                        {
                            if (button3.IsPressed() && button2.IsPressed())
                            {
                                i++;
                            }

                            Thread.Sleep(0);

                            if (i > 500)
                            {
                                button3.NotifyCalibration();
                                startcal = true;
                            }
                        }
                    }

                    button2.Notify();
                }

                if (button3.IsPressed())
                {
                    if (button3.IsPressed())
                    {
                        button3.Notify();
                        int i = 0;
                        bool startcal = false;
                        while (button3.IsPressed() && !startcal)
                        {
                            i = 0;
                            while (button3.IsPressed() && button2.IsPressed())
                            {
                                if (button3.IsPressed() && button2.IsPressed())
                                {
                                    i++;
                                }

                                Thread.Sleep(0);

                                if (i > 500)
                                {
                                    button3.NotifyCalibration();
                                    startcal = true;
                                }
                            }
                        }

                        button3.Notify();
                    }

                    Thread.Sleep(0);
                }

                
                if (button4.IsPressed())
                {
                    button4.Notify();
                    while (button4.IsPressed())
                    {
                        Thread.Sleep(0);
                    }

                    button4.Notify();
                }

                Thread.Sleep(0);
            }
        }

    }
}