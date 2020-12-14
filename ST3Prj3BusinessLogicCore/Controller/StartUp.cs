using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using DataAccesLogic.Boundaries;
using DataAccesLogic.Drivers;
using Domain.DTOModels;
using MathNet.Numerics;

namespace BusinessLogic.Controller
{
    public class StartUp
    {
       private BlockingCollection<Measure_DTO> _dataQueueMeasure;
        private BlockingCollection<Adjustments_DTO> _dataQueueAdjustments;
        private BlockingCollection<LCD_DTO> _dataQueueLCD;
        private BlockingCollection<MetaData_DTO> _dataQueueMetadata;
        private ButtonObserver _button1Observer;
        private ButtonObserver _button2Observer;
        private ButtonObserver _button3Observer;
        private ButtonObserver _button4Observer;
        private List<int> ageLs;
        private List<string> genderLs;
        private MetaData_DTO dto;


        private ManualResetEvent _calibrationEventMeasure;

        
        //private bool stop=false;

        //public bool Stop
        //{
        //    get { return stop; }
        //    set { stop = value; }
        //}

        public StartUp(BlockingCollection<Measure_DTO> dataQueueMeasure, BlockingCollection<Adjustments_DTO> dataQueueAdjustments,
            BlockingCollection<MetaData_DTO> dataQueueMetaData, ButtonObserver buttonObserver1, ButtonObserver buttonObserver2,
            ButtonObserver buttonObserver3, ButtonObserver buttonObserver4, ManualResetEvent calibrationEventMeasure,
            BlockingCollection<LCD_DTO> dataQueueLcd)
        {
            
            _dataQueueMeasure = dataQueueMeasure;
            _dataQueueAdjustments = dataQueueAdjustments;
            _dataQueueMetadata = dataQueueMetaData;
            _button1Observer = buttonObserver1;
            _button2Observer = buttonObserver2;
            _button3Observer = buttonObserver3;
            _button4Observer = buttonObserver4;
            _calibrationEventMeasure = calibrationEventMeasure;
            _dataQueueLCD = dataQueueLcd;

            dto = new MetaData_DTO();

            ageLs = new List<int>();

            genderLs = new List<string>() {"Male", "Female"};

            for (int i = 0; i < 111; i++)
            {
                ageLs.Add(i);
            }

        }
        public void Run()
        {
            _dataQueueLCD.Add(new LCD_DTO()
            {
                Message =
                    "Welcome. Please press Prepare t0 proceed."
            });
            Debug.WriteLine("Welcome. Please press Prepare to proceed.");
            
            while (!_button1Observer.IsPressed)
            {
                Thread.Sleep(1);
            }

            Thread.Sleep(50);

            PerformMetadata();
            PerformZeroPointAdj();

            _dataQueueMetadata.Add(dto);

        }

        void PerformMetadata()
        {
            _dataQueueLCD.Add(new LCD_DTO()
            {
                Message =
                    "Please select gender and press Prepare to continue"
            });
            Debug.WriteLine("Please select gender and press Prepare to continue");

            Thread.Sleep(1000);

            ChooseGender();
            
            _dataQueueLCD.Add(new LCD_DTO()
            {
                Message =
                    "Your choice have been saved"
            });
            Debug.WriteLine("Your choice have been saved");

            Thread.Sleep(1000);

            _dataQueueLCD.Add(new LCD_DTO()
            {
                Message =
                    "Please use the arrows to select age and press prepare to confirm"
            });
            Debug.WriteLine("Please use the arrows to select age and press prepare to confirm");

            Thread.Sleep(1000);

            ChooseAge();

            _dataQueueLCD.Add(new LCD_DTO()
            {
                Message =
                    "Your choice have been saved"
            });
            Debug.WriteLine("Your choice have been saved");

            Thread.Sleep(1000);
        }
        double MeasureZeroPoint()
        {
            List<int> zeroPointMeasures = new List<int>();
            ClearQueueMeasure(_dataQueueMeasure);
            for (int i = 0; i < 3;)
            {
                _calibrationEventMeasure.Reset();

                while (_dataQueueMeasure.Count == 0)
                {
                    Thread.Sleep(10);
                }
                _calibrationEventMeasure.Set();

                if (_dataQueueMeasure.Count >= 1)
                {
                    var container = _dataQueueMeasure.Take();
                    //Gemme de tre containers for første testtryk i liste
                    foreach (var sample in container.SamplePack.SampleList)
                    {
                        zeroPointMeasures.Add(sample.Value);
                    }
                    ClearQueueMeasure(_dataQueueMeasure);
                    i++;
                }
                else
                {
                    ClearQueueMeasure(_dataQueueMeasure);
                }
            }
            
            return zeroPointMeasures.Average();
        }



        public void ClearQueueMeasure(BlockingCollection<Measure_DTO> blockingCollection)
        {
            while (blockingCollection.Count > 0)
            {
                blockingCollection.TryTake(out _);
            }
        }

        void PerformZeroPointAdj()
        {
            Start:
            
            _dataQueueLCD.Add(new LCD_DTO()
            {
                Message =
                    "Please position the PVC tap as shown in the manual and press Prepare to initialize the device"
            });
            Debug.WriteLine("Please position the PVC tap as shown in the manual and press Prepare to initialize the device");
            
            while (!_button1Observer.IsPressed)
            {
                Thread.Sleep(0);
            }
     
                
            _dataQueueLCD.Add(new LCD_DTO()
            {
                Message =
                    "Measuring...        Please don't move PVC tap.."
            });
            Debug.WriteLine("Measuring...        Please don't move PVC tap..");
            //if (MeasureZeroPoint() < 650 || MeasureZeroPoint() > 800)//Lowest and highest recorded airpressure in mmHg soucre:
            //                                                         //https://sciencing.com/understand-barometric-pressure-readings-5397464.html
            //{

            //    _dataQueueLCD.Add(new LCD_DTO()
            //    {
            //        Message =
            //            "The measured pressure was not as expected. Please make sure the position of the PVC tap is correct, and try again."
            //    });

            //    Debug.WriteLine("The measured pressure was not as expected. Please make sure the position of the PVC tap is correct, and try again.");

            //    goto Start;
            //}

            Adjustments_DTO DTO = new Adjustments_DTO();
            //DTO.ZeroPoint = MeasureZeroPoint();
            DTO.ZeroPoint = -1.5;
            _dataQueueAdjustments.Add(DTO);
        
            _dataQueueLCD.Add(new LCD_DTO()
            {
                Message =
                    "The device have been initialized with pressure 750mmHg. Press Start to start measuring"
            });
            Debug.WriteLine("The device have been initialized. Adjust PVC tap back to position 3 and press Start to start measuring");
            Thread.Sleep(1000);
        }

        void ChooseAge()
        {
            int i = 0;
            _dataQueueLCD.Add(new LCD_DTO()
            {
                Message =$"Age: {ageLs[i]}"

            });
            Debug.WriteLine($"Age: {ageLs[i]}");
            while(!_button1Observer.IsPressed)
            {
                if (_button2Observer.IsPressed)
                {
                    if (i==110)
                    {
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                    
                    _dataQueueLCD.Add(new LCD_DTO()
                    {
                        Message =$"Age: {ageLs[i]}"

                    });
                    Debug.WriteLine($"Age: {ageLs[i]}");
                }

                if (_button3Observer.IsPressed)
                {
                    if (i==0)
                    {
                        i = 110;
                    }
                    else
                    {
                        i--;
                    }
                    
                    _dataQueueLCD.Add(new LCD_DTO()
                    {
                        Message =$"Age: {ageLs[i]}"

                    });
                    Debug.WriteLine($"Age: {ageLs[i]}");
                }
            }

            dto.Age = i;
        }


        void ChooseGender()
        {
            int i = 0;
            _dataQueueLCD.Add(new LCD_DTO()
            {
                Message =$"Gender: {genderLs[i]}"

            });
            Debug.WriteLine($"Gender: {genderLs[i]}");

            while(!_button1Observer.IsPressed)
            {
                

                if (_button2Observer.IsPressed)
                {
                    if (i==0)
                    {
                        i = 1;
                    }
                    else
                    {
                        i--;
                    }
                    _dataQueueLCD.Add(new LCD_DTO()
                    {
                        Message =$"Gender: {genderLs[i]}"

                    });
                    Debug.WriteLine($"Gender: {genderLs[i]}");
                }

                if (_button3Observer.IsPressed)
                {
                    if (i==1)
                    {
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                    _dataQueueLCD.Add(new LCD_DTO()
                    {
                        Message =$"Gender: {genderLs[i]}"

                    });
                    Debug.WriteLine($"Gender: {genderLs[i]}");
                }

            }

            
            dto.Gender = i;

        }

    }

}