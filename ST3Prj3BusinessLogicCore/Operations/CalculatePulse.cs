using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Domain.DTOModels;

namespace BusinessLogic.Operations
{
    public class CalculatePulse
    {
        private int threshold = 90; //mmHg
        private List<int> lsSamples;
        private bool Thresholdreached1 = false;
        private bool Thresholdreached2 = false;
        private bool Thresholdreached3 = false;

        #region Properties

        public int highestSample4 { get; private  set; }

        public int highestSampleIndex4 { get;private  set; }

        public bool Thresholdreached4 { get;private  set; }

        public bool ThresholdDone3 { get;private  set; }

        public int highestSample3 { get;private  set; }

        public int highestSampleIndex3 { get;private  set; }

        public bool ThresholdDone2 { get;private  set; }

        public int highestSample2 { get;private  set; }

        public int highestSampleIndex2 { get;private  set; }

        public bool ThresholdDone1 { get;private  set; }

        public int highestSample6 { get;private  set; }

        public int highestSampleIndex9 { get;private  set; }

        public int highestSample9 { get;private  set; }

        public bool Thresholdreached9 { get;private  set; }

        public bool ThresholdDone8 { get;private  set; }

        public int highestSampleIndex8 { get;private  set; }

        public int highestSample8 { get;private  set; }

        public bool Thresholdreached8 { get;private  set; }

        public bool ThresholdDone7 { get;private  set; }

        public int highestSampleIndex7 { get;private  set; }

        public int highestSample7 { get;private  set; }

        public bool Thresholdreached7 { get;private  set; }

        public bool ThresholdDone6 { get;private  set; }

        public bool Thresholdreached6 { get;private  set; }

        public bool ThresholdDone5 { get;private  set; }

        public int highestSampleIndex6 { get;private  set; }

        public int highestSampleIndex5 { get;private  set; }

        public int highestSample5 { get;private  set; }

        public bool Thresholdreached5 { get;private  set; }

        public bool ThresholdDone4 { get;private  set; }

        #endregion

        private List<int> HeartBeatIndexList;


        public CalculatePulse()
        {
           HeartBeatIndexList = new List<int>();

            //10, 50, 80, 100, 140
            //var 40
            //Var 30
            //var 20
            //var 40
        }

        public List<int> FindPulseBeat(List<int> threeSecData)
        {
            lsSamples = threeSecData;
            
            //Sammenligner med Threshold, alle værdier derover, kan potentielt være pulsslag.
            int j = 0;
            int highestSampleIndex1 = 0;
            int highestSample1 = 0;

            foreach (var value in lsSamples)
            {
                if (value >= threshold)
                {
                    Thresholdreached1 = true;
                    if (value >= lsSamples[j])
                    {
                        highestSample1 = value;
                        highestSampleIndex1 = lsSamples.IndexOf(value);
                        HeartBeatIndexList.Add(highestSampleIndex1);
                    }
                   
                }

                if (Thresholdreached1 == true && value < threshold)
                {
                    ThresholdDone1 = true;
                }

                if (ThresholdDone1 == true && value > threshold)
                {
                    Thresholdreached2 = true;
                    if (value > lsSamples[j])
                    {
                        highestSample2 = value;
                        highestSampleIndex2 = lsSamples.IndexOf(value);
                        HeartBeatIndexList.Add(highestSampleIndex2);
                    }
                }

                if (Thresholdreached2 == true && value < threshold)
                {
                    ThresholdDone2 = true;
                }

                if (ThresholdDone2 == true && value > threshold)
                {
                    Thresholdreached3 = true;
                    if (value > lsSamples[j])
                    {
                        highestSample3 = value;
                        highestSampleIndex3 = lsSamples.IndexOf(value);
                        HeartBeatIndexList.Add(highestSampleIndex3);
                    }
                }

                if (Thresholdreached3 == true && value < threshold)
                {
                    ThresholdDone3 = true;
                }

                if (ThresholdDone3 == true && value > threshold)
                {
                    Thresholdreached4 = true;
                    if (value > lsSamples[j])
                    {
                        highestSample4 = value;
                        highestSampleIndex4 = lsSamples.IndexOf(value);
                        HeartBeatIndexList.Add(highestSampleIndex4);
                    }
                }

                if (Thresholdreached4 == true && value < threshold)
                {
                    ThresholdDone4 = true;
                }

                if (ThresholdDone4 == true && value > threshold)
                {
                    Thresholdreached5 = true;
                    if (value > lsSamples[j])
                    {
                        highestSample5 = value;
                        highestSampleIndex5 = lsSamples.IndexOf(value);
                        HeartBeatIndexList.Add(highestSampleIndex6);
                    }
                }

                if (Thresholdreached5 == true && value < threshold)
                {
                    ThresholdDone5 = true;
                }

                if (ThresholdDone5 == true && value > threshold)
                {
                    Thresholdreached6 = true;
                    if (value > lsSamples[j])
                    {
                        highestSample6 = value;
                        highestSampleIndex6 = lsSamples.IndexOf(value);
                        HeartBeatIndexList.Add(highestSampleIndex6);
                    }
                }

                if (Thresholdreached6 == true && value < threshold)
                {
                    ThresholdDone6 = true;
                }

                if (ThresholdDone6 == true && value > threshold)
                {
                    Thresholdreached7 = true;
                    if (value > lsSamples[j])
                    {
                        highestSample7 = value;
                        highestSampleIndex7 = lsSamples.IndexOf(value);
                        HeartBeatIndexList.Add(highestSampleIndex7);
                    }
                }

                if (Thresholdreached7 == true && value < threshold)
                {
                    ThresholdDone7 = true;
                }

                if (ThresholdDone7 == true && value > threshold)
                {
                    Thresholdreached8 = true;
                    if (value > lsSamples[j])
                    {
                        highestSample8 = value;
                        highestSampleIndex8 = lsSamples.IndexOf(value);
                        HeartBeatIndexList.Add(highestSampleIndex8);
                    }
                }

                if (Thresholdreached8 == true && value < threshold)
                {
                    ThresholdDone8 = true;
                }

                if (ThresholdDone8 == true && value > threshold)
                {
                    Thresholdreached9 = true;
                    if (value > lsSamples[j])
                    {
                        highestSample9 = value;
                        highestSampleIndex9 = lsSamples.IndexOf(value);
                        HeartBeatIndexList.Add(highestSampleIndex9);
                    }
                }

                j++;
            }

            return HeartBeatIndexList;
        }


        public int CalcPulse(List<int> threeSecData)
        {
            var indexLs = FindPulseBeat(threeSecData);
            var diffLs = new List<int>();
            
            for (int i = 0; i < indexLs.Count-1; i++)
            {
                diffLs.Add(indexLs[i+1] - indexLs[i]);
            }

            if (indexLs.Count != 0)
            {
                return (int)diffLs.Average();
            }

            return new int();

        }

        
    }
}
