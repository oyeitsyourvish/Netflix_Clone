using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetflixClone.Models;

namespace NetflixClone.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<MovieCategory> MovieCategories { get; set; } = null!;
        public DbSet<UserMovie> UserMovies { get; set; } = null!;


        //we are going to explicitly tell EF Core how MovieCategory works.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Movie Category

            modelBuilder.Entity<MovieCategory>()
                .HasKey(mc => new { mc.MovieId, mc.CategoryId });

            modelBuilder.Entity<MovieCategory>()
                .HasOne(mc => mc.Movie)
                .WithMany(m => m.MovieCategories)
                .HasForeignKey(mc => mc.MovieId);

            modelBuilder.Entity<MovieCategory>()
                .HasOne(mc => mc.Category)
                .WithMany(c => c.MovieCategories)
                .HasForeignKey(mc => mc.CategoryId);

            // User - Movie

            modelBuilder.Entity<UserMovie>()
                .HasKey(um => new
                {
                    um.UserId,
                    um.MovieId
                });

            modelBuilder.Entity<UserMovie>()
                .HasOne(um => um.User)
                .WithMany(u => u.UserMovies)
                .HasForeignKey(um => um.UserId);

            modelBuilder.Entity<UserMovie>()
                .HasOne(um => um.Movie)
                .WithMany(m => m.UserMovies)
                .HasForeignKey(um => um.MovieId);


            // Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action" },
                new Category { Id = 2, Name = "Comedy" },
                new Category { Id = 3, Name = "Drama" },
                new Category { Id = 4, Name = "Sci-Fi" },
                new Category { Id = 5, Name = "Thriller" }
            );
        }
    }
}
