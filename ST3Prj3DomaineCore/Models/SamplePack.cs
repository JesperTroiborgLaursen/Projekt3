﻿using System;
using System.Collections.Generic;

namespace DomaineCore.Models
{
    public class SamplePack
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public List<Sample> SampleList;

        public SamplePack(DateTime date, int id)
        {
            ID = id;
            Date = date;
        }

        public SamplePack()
        {
            SampleList = new List<Sample>();
        }

        public override string ToString()
        {
            string result = $"{ID} \n {Date} \n";

            foreach (var VARIABLE in SampleList)
            {
                result = $"{result} \n{VARIABLE.Value.ToString()}\n";
            }

            return result;
        }
    }


}