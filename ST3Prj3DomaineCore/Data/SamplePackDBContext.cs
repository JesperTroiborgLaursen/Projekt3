using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DomaineCore.Data
{
    public class SamplePackDBContext : DbContext
    {

        public SamplePackDBContext(DbContextOptions<SamplePackDBContext> options) : base(options)
        {
        }
        public DbSet<SamplePack> SamplePacks { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options) erstattet af kode i factory
        //    => options.UseSqlite("Data Source=SamplePacks.db");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            

            builder.Entity<SamplePack>()
                .Property(e => e.ID).IsRequired()
                .HasColumnType("int32");
            builder.Entity<SamplePack>()
                .Property(e => e.Date).IsRequired()
                .HasColumnType("DATETIME");
            builder.Entity<SamplePack>()
                .Property(e => e.SampleList).IsRequired()
                .HasColumnType("BLOB");

            base.OnModelCreating(builder);
        }

        
    }
}