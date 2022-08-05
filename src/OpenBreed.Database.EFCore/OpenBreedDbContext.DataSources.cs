using Microsoft.EntityFrameworkCore;
using OpenBreed.Database.EFCore.DbEntries;
using System;
using System.Linq;

namespace OpenBreed.Database.EFCore
{
    public partial class OpenBreedDbContext : DbContext
    {
        public DbSet<DbDataSource> DataSources { get; set; }
        public DbSet<DbEpfArchiveFileDataSource> EpfArchiveFileDataSources { get; set; }
        public DbSet<DbFileDataSource> FileDataSources { get; set; }
        public DbSet<DataSourceType> DataSourceTypes { get; set; }

        private partial void OnModelCreatingDataSources(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbDataSource>()
                .ToTable("DataSources");

            modelBuilder.Entity<DbEpfArchiveFileDataSource>()
                .ToTable("EpfArchiveFileDataSources");

            modelBuilder.Entity<DbFileDataSource>()
                .ToTable("FileDataSources");

            modelBuilder
                .Entity<DbDataSource>()
                .Property(e => e.TypeId)
                .HasConversion<int>();

            modelBuilder
                .Entity<DataSourceType>()
                .Property(e => e.Id)
                .HasConversion<int>();

            modelBuilder
                .Entity<DataSourceType>().HasData(
                    Enum.GetValues(typeof(DataSourceTypeEnum))
                        .Cast<DataSourceTypeEnum>()
                        .Select(e => new DataSourceType()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
                );
        }
    }
}