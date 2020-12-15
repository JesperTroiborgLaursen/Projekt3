using System.Collections.Generic;
using Domain.Models;

namespace Interfaces
{
    public interface IDatabase
    {
        //Interface to easy impl. HL7 FHIR or other database
        public void SaveSamplePack(SamplePack samplePack);
        public void DeleteSamplePack(int samplePackID);
        public SamplePack GetSamplePack(int samplePackID);
        public List<SamplePack> GetAllSamplePacks();
    }
}