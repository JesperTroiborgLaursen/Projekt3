using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class SamplePack
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }

        //public List<Sample> SampleList;
        public byte[] SampleList;

        public SamplePack(DateTime date, int id)
        {
            ID = id;
            Date = date;
            SampleList = new byte[100];
        }

        public SamplePack()
        {
            SampleList = new byte[100];
        }

        public override string ToString()
        {
            string result = $"{ID} \r\n {Date}";

            foreach (var VARIABLE in SampleList)
            {
                result = $"{result} \r\n{VARIABLE.ToString()}";
            }

            result += "END";
            return result;
        }
    }


}