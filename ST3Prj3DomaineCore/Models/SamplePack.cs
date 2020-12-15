using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class SamplePack
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public List<Sample> SampleList { get; set; }

        public SamplePack(DateTime date, int id)
        {
            ID = id;
            Date = date;
            SampleList = new List<Sample>();
        }
        
        public SamplePack()
        {
            SampleList = new List<Sample>();
        }

    }


}