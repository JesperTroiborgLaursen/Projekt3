using System;
using System.Collections.Generic;

namespace Domain.Models
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
            string result = $"{ID} \r\n {Date}";

            foreach (var VARIABLE in SampleList)
            {
                result = $"{result} \r\n{VARIABLE.Value.ToString()}";
            }

            result += "END";
            return result;
        }
    }


}