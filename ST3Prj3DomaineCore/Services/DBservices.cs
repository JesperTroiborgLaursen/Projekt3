using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using DomaineCore.Data;
using Microsoft.Extensions.Configuration;

namespace DomaineCore.Services
{
    public class DBservices : IDBservices
    {
        private readonly IConfigurationRoot config;
        private readonly SamplePackDBContext context;

        public DBservices(IConfigurationRoot configurationRoot, SamplePackDBContext samplePackDbContext)
        {
            config = configurationRoot;
            context = samplePackDbContext;
        }


        public List<SamplePack> GetAllSamplePacks()
        {
            var resultingSamplePacks = context.SamplePacks
                .OrderBy(e => e.ID)
                .ToList();


            return resultingSamplePacks;
        }

        public void AddSamplePack(SamplePack samplePack)
        {
            context.SamplePacks.Add(samplePack);
            context.SaveChanges();
        }
    }
}
