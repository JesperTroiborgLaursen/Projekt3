using System.Collections.Generic;
using Domain.Models;

namespace Interfaces
{
    public interface IDatabase
    {
        public void SaveSamplePack(SamplePack samplePack);
        public void DeleteSamplePack(int samplePackID);
        public SamplePack GetSamplePack(int samplePackID);
        public List<SamplePack> GetAllSamplePacks();
    }
}