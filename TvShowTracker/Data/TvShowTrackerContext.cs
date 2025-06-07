using Microsoft.EntityFrameworkCore;
using TvShowTracker.Models;

namespace TvShowTracker.Data
{
    public class TvShowTrackerContext : DbContext
    {
        public TvShowTrackerContext(DbContextOptions<TvShowTrackerContext> options) : base(options) { }

        public DbSet<Show> Shows { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Show>()
                .HasMany(t => t.Genres)
                .WithMany(g => g.Shows);

            modelBuilder.Entity<Show>()
                .HasMany(t => t.Actors)
                .WithMany(a => a.Shows);

            
            modelBuilder.Entity<User>()
                .HasMany(u => u.Favorites)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserFavorites"));
        }
    }
}
