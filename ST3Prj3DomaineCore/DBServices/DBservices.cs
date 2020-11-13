using Domain.Context;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.DBServices
{
    public class DBservices : IDBservices
    {
        private readonly IConfigurationRoot config;
        private readonly SamplePackDBContext context;
        private ServiceCollection services;

        public DBservices(IConfigurationRoot configurationRoot, SamplePackDBContext samplePackDbContext)
        {
            config = configurationRoot;
            context = samplePackDbContext;

        }
        
        public void AddSamplePack(SamplePack samplePack)
        {
            context.SamplePacks.Add(samplePack);
            context.SaveChanges();
        }
    }
}
