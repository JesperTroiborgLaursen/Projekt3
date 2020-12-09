using System.Collections.Generic;
using System.Linq;
using Domain.Models;

namespace BusinessLogic.Operations
{
    public class BPAvg
    {
        public double Avg(List<int> threeSecData)
        {
            double result = 0;
            foreach (var sample in threeSecData)
            {
                result += sample;
            }

            double resultAvg = result / threeSecData.Count();

            return resultAvg;
        }
    }
}