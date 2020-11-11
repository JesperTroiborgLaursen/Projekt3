using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DomaineCore.Data
{
    public class SamplePackDBContext : DbContext
    {
        private readonly SamplePackDBContext context;

        public SamplePackDBContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        //public SamplePackDBContext(DbContextOptionsBuilder options)
        //{
        //    options.UseSqlite("Data Source=SamplePacks.db");
        //}

        public DbSet<SamplePack> SamplePacks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=SamplePacks.db");

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<SamplePack>().HasData(GetAllSamplePacks());

            //builder.Entity<Sample>().HasNoKey();
            base.OnModelCreating(builder);
            

            //builder.Entity<SamplePack>()
            //    .Property(e => e.ID).IsRequired()
            //    .HasColumnType("int32");
            //builder.Entity<SamplePack>()
            //    .Property(e => e.Date).IsRequired()
            //    .HasColumnType("DATETIME");
            //builder.Entity<SamplePack>()
            //    .Property(e => e.SampleList).IsRequired()
            //    .HasColumnType("BLOB");

            //base.OnModelCreating(builder);
        }

        public List<SamplePack> GetAllSamplePacks()
        {
            return new List<SamplePack>()
            {
                new SamplePack() {Date = DateTime.Today, ID = 1, SampleList = new List<Sample>()
                {
                    new Sample(){SamplePackID = 1,Value = 2},
                    new Sample(){Value = 2,SamplePackID = 1}
                }},
                //new SamplePack() {Date = DateTime.Today, ID = 2, SampleList = new List<Sample>(){new Sample(){SamplePackID = 2, Value = 3}}},
                //new SamplePack() {Date = DateTime.Today, ID = 3, SampleList = new List<Sample>(){new Sample(){SamplePackID = 3, Value = 4}}},
                //new SamplePack() {Date = DateTime.Today, ID = 4, SampleList = new List<Sample>(){new Sample(){SamplePackID = 4,Value = 5}}}

            };

            
            //var resultingSamplePacks = context.SamplePacks
            //    .OrderBy(e => e.ID)
            //    .ToList();


            //return resultingSamplePacks;
        }


    }
}