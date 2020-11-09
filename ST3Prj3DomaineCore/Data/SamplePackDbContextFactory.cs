using System.Collections.Generic;
using DomaineCore.Models;
using DomaineCore.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;


namespace DomaineCore.Data

{
    public class SamplePackDbContextFactory
    {
        public SamplePackDBContext CreateContext(string connectionString)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<SamplePackDBContext>()
                .UseSqlite(configuration.GetConnectionString("DefaultConnection")); //oprettet appsettingsfil.json med default con.: Data Source=SamplePacks.db.

            var services = new ServiceCollection();
            services.AddSingleton(configuration);
            services.AddSingleton(optionsBuilder.Options);
            services.AddSingleton<IDBservices, DBservices>();
            services.AddDbContextPool<SamplePackDBContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
            services.BuildServiceProvider();

           
            //db oprettes som context her
            var SPDBcontext = new SamplePackDBContext(optionsBuilder.Options);
            SPDBcontext.Database.EnsureCreated();

            return SPDBcontext;

            //gamle Database implementation herunder:
            //var optionBuilder = new DbContextOptionsBuilder<SamplePackDBContext>();
            //optionBuilder.UseSqlite(connectionString);

            //var context = new SamplePackDBContext(optionBuilder.Options);
            //context.Database.EnsureCreated();
            //fra tidligere implementation jesper og marc 28.10


        }

    }
}