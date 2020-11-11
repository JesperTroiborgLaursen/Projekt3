using System.Linq;
using Domain.Models;

namespace BusinessLogic.Operations
{
    public class BPAvg
    {
        public double Avg(SamplePack samplePack)
        {
            double result = 0;
            foreach (var sample in samplePack.SampleList)
            {
                result += sample.Value;
            }

            double resultAvg = result / samplePack.SampleList.Count();

            return resultAvg;
        }
    }
}