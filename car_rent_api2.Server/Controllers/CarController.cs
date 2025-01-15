using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace car_rent_api2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly CarRentDbContext _context;

        public CarController(CarRentDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> Cars()
        {
            return await _context.Cars.Include(c=>c.Location).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> Get(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        [HttpGet("{car_id}/cardetails")]
        public async Task<ActionResult<IEnumerable<CarDetail>>> GetCarDetails(int car_id)
        {
            Car? car = await _context.Cars.Include(car => car.Details).FirstOrDefaultAsync(x => x.Id == car_id);

            return (car != null) ? Ok(car.Details) : Ok(new List<CarDetail>());
        }

        [HttpGet("{car_id}/carservices")]
        public async Task<ActionResult<CarService>> GetCarServiceById(int car_id)
        {
            Car? car = await _context.Cars.Include(c => c.Services).FirstOrDefaultAsync(x => x.Id == car_id);
            return (car != null) ? Ok(car.Services) : Ok(new List<CarService>());
        }

        [HttpGet("{car_id}/location")]
        public async Task<ActionResult<Location>> GetLocationByCarId(int car_id)
        {
            Car? car = await _context.Cars.Include(car => car.Location).FirstOrDefaultAsync(x => x.Id == car_id);

            return (car != null) ? Ok(car.Location) : Ok();
        }



        [HttpPost("createCars")]
        public async Task<ActionResult<IEnumerable<Car>>> CreateCars([FromBody] List<Car> cars)
        {
            foreach (var car in cars)
            {
                await CreateCar(car);
            }
            await _context.SaveChangesAsync();
            return Ok(cars);
        }

        [HttpPost]
        public async Task<ActionResult<Car>> CreateCar([FromBody] Car car)
        {
            Car newCar = new Car
            {
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                Photo = car.Photo,
                Price = car.Price,
                Status = CarStatus.Available,
                Location = new Location(),
                Details = new List<CarDetail>(),
                Services = new List<CarService>()
            };

            // Create or link existing details
            for(int i=0; i< car.Details.Count; i++)
            {
                newCar.Details.Add(await CreateCarDetail(car.Details[i]));
            }

            // Create or link existing services
            for(int i=0; i< car.Services.Count; i++)
            {
                newCar.Services.Add(await CreateCarService(car.Services[i]));
            }

            // Create or link existing location
            newCar.Location = await CreateLocation(car.Location);


            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = car.Id }, car);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> EditCar(int id, [FromBody] Car car)
        {
            if (id != car.Id) return BadRequest();
            
            var existingCar = await _context.Cars
                .Include(c => c.Location)
                .Include(c => c.Details)
                .Include(c => c.Services)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (existingCar == null) return NotFound();

            existingCar.Brand = car.Brand;
            existingCar.Model = car.Model;
            existingCar.Year = car.Year;
            existingCar.Photo = car.Photo;
            existingCar.Price = car.Price;
            existingCar.Details = new List<CarDetail>();
            existingCar.Services = new List<CarService>();


            // Create or link existing details
            for (int i = 0; i < car.Details.Count; i++)
            {
                existingCar.Details.Add(await CreateCarDetail(car.Details[i]));
            }

            // Create or link existing services
            for (int i = 0; i < car.Services.Count; i++)
            {
                existingCar.Services.Add(await CreateCarService(car.Services[i]));
            }

            // Create or link existing location
            existingCar.Location = await CreateLocation(car.Location);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }

        private async Task<Location> CreateLocation(Location location)
        {
            location.Id = 0;
            var finded = await _context.Locations.FirstOrDefaultAsync(l => (
                l.Name == location.Name
                && l.Address == location.Address
                && l.Latitude == location.Latitude
                && l.Longitude == location.Longitude));

            if (finded != null) return finded;

            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();
            return location;
        }

        private async Task<CarDetail> CreateCarDetail(CarDetail detail)
        {
            detail.Id = 0;
            var finded = await _context.CarDetails.FirstOrDefaultAsync(d => (
            d.Description == detail.Description
            && d.Value == detail.Value));

            if (finded != null) return finded;

            await _context.CarDetails.AddAsync(detail);
            await _context.SaveChangesAsync();
            return detail;
        }

        private async Task<CarService> CreateCarService(CarService service)
        {
            service.Id = 0;
            var finded = await _context.CarServices.FirstOrDefaultAsync(s => (
                s.Name == service.Name
                && s.Description == service.Description
                && s.Price == service.Price));

            if (finded != null) return finded;

            await _context.CarServices.AddAsync(service);
            await _context.SaveChangesAsync();
            return service;
        }
    }
}
