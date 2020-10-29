﻿using System;
using System.Collections.Generic;
using System.Text;
using DomaineCore.Models;
using System.Linq;
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


        public List<SamplePack> GetSamplePacks()
        {
            var samplePakker = context.SamplePacks
                .OrderBy(e => e.ID)
                .ToList();


            return samplePakker;
        }

        public void AddSamplePack(SamplePack samplePack)
        {
            context.SamplePacks.Add(samplePack);
            context.SaveChanges();
        }
    }
}