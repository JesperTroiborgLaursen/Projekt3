using System.Collections.Generic;
using BusinessLogic.Operations;
using FluentAssertions;
using NUnit.Framework;

namespace NUnitTestRPi
{
    [TestFixture]
    public class UUT_BpAvg
    {
        private BPAvg uut;
        private List<int> threeSecData;
        [SetUp]
        public void Setup()
        {
            
            threeSecData = new List<int>();
            uut = new BPAvg();
            for (int i = 0; i < 149; i++)
            {
                threeSecData.Add(i);
            }
        }

        [Test]
        public void Avg_CalculateAvgOfOneTo150_ResultIs74()
        {
            
            uut.Avg(threeSecData).Should().Be(74);
        }
    }
}