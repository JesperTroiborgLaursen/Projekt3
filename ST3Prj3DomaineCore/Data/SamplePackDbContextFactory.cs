using System.Collections.Generic;
using DomaineCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DomaineCore.Data

{
    public static class SamplePackDbContextFactory
    {
        public static SamplePackDBContext Create(string connectionString)
        {
            var optionBuilder = new DbContextOptionsBuilder<SamplePackDBContext>();
            optionBuilder.UseSqlite(connectionString);

            var context = new SamplePackDBContext(optionBuilder.Options);
            context.Database.EnsureCreated();

            return context;

        }
    }
}