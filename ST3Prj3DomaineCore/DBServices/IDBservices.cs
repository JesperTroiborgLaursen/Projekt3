using System.Collections.Generic;
using Domain.Models;

namespace Domain.DBServices
{
    public interface IDBservices
    {
        //List<SamplePack> GetAllSamplePacks();
        void AddSamplePack(SamplePack samplePack);
    }
}
