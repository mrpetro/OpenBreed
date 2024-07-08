using Microsoft.EntityFrameworkCore;

namespace OpenBreed.Database.EFCore
{
    public partial class OpenBreedDbContext : DbContext
    {
        public OpenBreedDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=OpenBreedTemplate.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingDataSources(modelBuilder);
            OnModelCreatingTileAtlases(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private partial void OnModelCreatingDataSources(ModelBuilder modelBuilder);
        private partial void OnModelCreatingTileAtlases(ModelBuilder modelBuilder);
    }
}