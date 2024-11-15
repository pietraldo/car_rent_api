using Microsoft.EntityFrameworkCore;


namespace car_rent_api2.Server
{
    public class CarRentDbContext: DbContext
    {
        public CarRentDbContext(DbContextOptions<CarRentDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<CarDetail> CarDetails { get; set; }
        public DbSet<CarService> CarServices { get; set; }
    }
}
