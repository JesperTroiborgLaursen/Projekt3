using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Context
{
    public class SamplePackDBContext : DbContext
    {

        public SamplePackDBContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        
        public DbSet<SamplePack> SamplePacks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=SamplePacks.db");

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<SamplePack>().HasData(GetAllSamplePacks());
            builder.Entity<Sample>().HasData(GetSampleList());

            base.OnModelCreating(builder);
        }

        public List<SamplePack> GetAllSamplePacks()
        {
            return new List<SamplePack>()
            {
                new SamplePack() {ID =1, Date = DateTime.Today}
            };
        }

        public List<Sample> GetSampleList()
        {
            return new List<Sample>()
            {
                new Sample() {SamplePackID = 1, Value = 2, ID = 1},
                new Sample() {SamplePackID = 1, Value = 2, ID = 2}
            };
        }

    }
}