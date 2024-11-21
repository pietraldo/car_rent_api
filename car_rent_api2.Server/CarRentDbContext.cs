using car_rent_api2.Server.Models;
using Microsoft.EntityFrameworkCore;


namespace car_rent_api2.Server
{
    public class CarRentDbContext : DbContext
    {
        public CarRentDbContext(DbContextOptions<CarRentDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<CarDetail> CarDetails { get; set; }
        public DbSet<CarService> CarServices { get; set; }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Rent> Rents { get; set; }

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

            modelBuilder.Entity<Rent>().HasOne(r=> r.Car).WithMany().HasForeignKey(r => r.CarId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Rent>().HasOne(r => r.Client).WithMany().HasForeignKey(r => r.ClientId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Rent>().HasOne(r => r.Employee).WithMany().HasForeignKey(r => r.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
