using System;
using System.Collections.Generic;
using System.Text;
using DomaineCore.Models;

namespace DomaineCore.Services
{
    public interface IDBservices
    {
        List<SamplePack> GetAllSamplePacks();
        void AddSamplePack(SamplePack samplePack);
    }
}
