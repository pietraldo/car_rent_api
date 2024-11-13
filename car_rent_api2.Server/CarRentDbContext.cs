using Microsoft.EntityFrameworkCore;


namespace car_rent_api2.Server
{
    public class CarRentDbContext: DbContext
    {
        public CarRentDbContext(DbContextOptions<CarRentDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
    }
}
