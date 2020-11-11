using System.Collections.Generic;
using Domain.Models;

namespace DomaineCore.Services
{
    public interface IDBservices
    {
        //List<SamplePack> GetAllSamplePacks();
        void AddSamplePack(SamplePack samplePack);
    }
}
