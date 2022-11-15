using Microsoft.EntityFrameworkCore;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface.Items.Assets;
using System;
using System.Linq;

namespace OpenBreed.Database.EFCore
{
    public partial class OpenBreedDbContext : DbContext
    {
        public DbSet<DbAsset> Assets { get; set; }

        public DbSet<AssetFormatParameter> FormatParameters { get; set; }

        private partial void OnModelCreatingAssets(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbAsset>()
                .ToTable("Assets");

            modelBuilder.Entity<AssetFormatParameter>()
                .ToTable("FormatParameters");
        }
    }
}