using MemberShip.Domain;
using Microsoft.EntityFrameworkCore;

namespace MemberShip.Infrastructure
{
    public class AppContext : DbContext 
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Todos> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Todos>()
                .HasOne(t => t.User)
                .WithMany(u => u.Todos)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Todos>()
                .Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(250);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "user1@example.com",
                    Password = "AAmHUvRRDQLxYolb2TP+yOiOadcfTtc+AwajPQZ9JWpi1B7pAL4xz4dG9yg2p8Xe5w==",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    IsAdmin = false,
                    IsActive = true
                },
                new User
                {
                    Id = 2,
                    Email = "user2@example.com",
                    Password = "ACIWSmVkigttTZdMj+EHQj9oEKONLLA7MrYITne3uwpHwVljbe4pBfAs0nXcLSgyWg==",
                    DateOfBirth = new DateTime(1990, 1, 2),
                    IsAdmin = false,
                    IsActive = true
                },
                new User
                {
                    Id = 3,
                    Email = "user3@example.com",
                    Password = "ALRfQQPf+7OZnVYOUfJqNWjwRzcW3MYiTASn6/WkcuETnVCy7tyH47izVdvgn7Gomg==",
                    DateOfBirth = new DateTime(1990, 1, 3),
                    IsAdmin = false,
                    IsActive = true
                },
                new User
                {
                    Id = 4,
                    Email = "user4@example.com",
                    Password = "AOLs9j++/2NGvW6xOXJ6/J8C+zA6xJo6bdqPrGCKRkkXychlexQxSt51kspxjn+9cg==",
                    DateOfBirth = new DateTime(1990, 1, 4),
                    IsAdmin = false,
                    IsActive = true
                },
                new User
                {
                    Id = 5,
                    Email = "user5@example.com",
                    Password = "AGkrZWF3A+LcaNZ+MDaVavUDdp2xnM7SBjki0FXW3zYyS3mTv50O8zwJhii/toFtzA==",
                    DateOfBirth = new DateTime(1990, 1, 5),
                    IsAdmin = false,
                    IsActive = true
                },
                new User
                {
                    Id = 6,
                    Email = "user6@example.com",
                    Password = "AFJF1dZxF6g9gUe00IzKXL/q8gRFOgFhmpzyPmraTubjlUCXRiHKfq9/rete2406Sg==",
                    DateOfBirth = new DateTime(1990, 1, 6),
                    IsAdmin = false,
                    IsActive = true
                },
                new User
                {
                    Id = 7,
                    Email = "user7@example.com",
                    Password = "AAhMi6osv0mAob1F+D8Q1L3jK7GVcKkg9peetDP8BI0B4ionURaJ6lFbyojJ5mGyrg==",
                    DateOfBirth = new DateTime(1990, 1, 7),
                    IsAdmin = false,
                    IsActive = true
                },
                new User
                {
                    Id = 8,
                    Email = "user8@example.com",
                    Password = "AEcUAO2/vFuVyClGcRF2PNtod71GuwQFGHmYByKDBrBFjMCSwDWqNED8m40W0EfzEQ==",
                    DateOfBirth = new DateTime(1990, 1, 8),
                    IsAdmin = false,
                    IsActive = true
                },
                new User
                {
                    Id = 9,
                    Email = "user9@example.com",
                    Password = "AN8JsuvMPYpeldl0SJ8tWes2w+6E7q7Y6O09jsE6oxz+BC1WcQxTivd2YSPQidmk2g==",
                    DateOfBirth = new DateTime(1990, 1, 9),
                    IsAdmin = false,
                    IsActive = true
                },
                new User
                {
                    Id = 10,
                    Email = "user10@example.com",
                    Password = "AB8aoLjEKpWAVbLuV6IRadwmD7CCoqr7jBuS5cvezimwvkVQjh1CMleg54y9h0a+Wg==",
                    DateOfBirth = new DateTime(1990, 1, 10),
                    IsAdmin = false,
                    IsActive = true
                }
            );
        }


    }
}
