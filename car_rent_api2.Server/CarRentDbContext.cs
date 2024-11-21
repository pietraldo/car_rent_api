using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace car_rent_api2.Server
{
    public class CarRentDbContext : IdentityDbContext
    {
        public CarRentDbContext(DbContextOptions<CarRentDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<CarDetail> CarDetails { get; set; }
        public DbSet<CarService> CarServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring the many-to-many relationship between Car and CarDetails
            modelBuilder.Entity<Car>()
                .HasMany(c => c.Details)
                .WithMany()
                .UsingEntity(j => j.ToTable("CarCarDetails"));

            // Configuring the many-to-many relationship between Car and CarServices
            modelBuilder.Entity<Car>()
                .HasMany(c => c.Services)
                .WithMany()
                .UsingEntity(j => j.ToTable("CarCarServices"));

            modelBuilder.Entity<Car>()
            .HasOne(c => c.Location) // Car has one Location
            .WithMany()              // Location has many Cars
            .HasForeignKey(c => c.LocationId) // Foreign key in Car table
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
