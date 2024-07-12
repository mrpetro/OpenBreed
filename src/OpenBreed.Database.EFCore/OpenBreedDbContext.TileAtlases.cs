using Microsoft.EntityFrameworkCore;
using OpenBreed.Database.EFCore.DbEntries;
using System;
using System.Linq;

namespace OpenBreed.Database.EFCore
{
    public partial class OpenBreedDbContext : DbContext
    {
        public DbSet<DbTileAtlas> TileAtlases { get; set; }
        public DbSet<DbTileAtlasFromBlk> TileAtlasesFromBlk { get; set; }
        public DbSet<DbTileAtlasFromImage> TileAtlasesFromImages { get; set; }
        public DbSet<TileAtlasType> TileAtlasTypes { get; set; }

        private partial void OnModelCreatingTileAtlases(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbTileAtlas>()
                .ToTable("TileAtlases");

            modelBuilder.Entity<DbTileAtlasFromBlk>()
                .ToTable("TileAtlasesFromBlk");

            modelBuilder.Entity<DbTileAtlasFromImage>()
                .ToTable("TileAtlasesFromImages");

            modelBuilder
                .Entity<DbDataSource>()
                .Property(e => e.TypeId)
                .HasConversion<int>();

            modelBuilder
                .Entity<DataSourceType>()
                .Property(e => e.Id)
                .HasConversion<int>();

            modelBuilder
                .Entity<TileAtlasType>().HasData(
                    Enum.GetValues(typeof(TileAtlasTypeEnum))
                        .Cast<TileAtlasTypeEnum>()
                        .Select(e => new TileAtlasType()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
                );
        }
    }
}