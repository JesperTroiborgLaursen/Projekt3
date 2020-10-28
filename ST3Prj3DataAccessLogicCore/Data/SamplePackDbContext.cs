using System;
using System.Collections.Generic;
using DomaineCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLogicCore.Data
{
    public class SamplePackDbContext : DbContext
    {
        #region Contructor
            public SamplePackDbContext(DbContextOptions<SamplePackDbContext> options) : base(options)
            {
                Database.EnsureCreated();
            }
            #endregion

            #region Public properties
            public DbSet<SamplePack> SamplePack { get; set; }
            #endregion

            #region Overridden method
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<SamplePack>().HasData(getSamplePacks());
                base.OnModelCreating(modelBuilder);
            }
            #endregion


            #region Private method
            private List<SamplePack> getSamplePacks()
            {
                return new List<SamplePack>
                {
                    new SamplePack(DateTime.Now, 0){SampleList = new List<Sample>()}
                };
            }
            #endregion
    }
}