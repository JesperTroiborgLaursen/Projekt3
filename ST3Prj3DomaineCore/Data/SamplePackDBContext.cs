using System.Collections.Generic;
using DomaineCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DomaineCore.Data
{
    public class SamplePackDBContext : DbContext
    {

        public SamplePackDBContext(DbContextOptions<SamplePackDBContext> options) : base(options)
        {
        }
        public DbSet<SamplePack> SamplePacks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=SamplePacks.db");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SamplePack>()
                .Property(e => e.ID)
                .HasColumnType("int32");
            builder.Entity<SamplePack>()
                .Property(e => e.Date)
                .HasColumnType("DATETIME");
            builder.Entity<SamplePack>()
                .Property(e => e.SampleList)
                .HasColumnType("varbinary(max)");
        }

        
    }
}