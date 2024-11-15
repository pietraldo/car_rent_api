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

        // GET: api/Car
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> Get()
        {
            return await _context.Cars.ToListAsync();
        }

        [HttpGet("cardetails")]
        public async Task<ActionResult<IEnumerable<CarDetail>>> GetCarDetails()
        {
            List<CarDetail> carDetails = new List<CarDetail>();
            carDetails.Add(new CarDetail { Id = 1, Description = "Description 1", Value = "Value 1" });
            carDetails.Add(new CarDetail { Id = 2, Description = "Description 2", Value = "Value 2" });
            return carDetails;
            //return await _context.CarDetails.ToListAsync();
        }

        [HttpGet("carservices")]
        public async Task<ActionResult<IEnumerable<CarService>>> GetCarServices()
        {
            List<CarService> carServices = new List<CarService>();
            carServices.Add(new CarService { Id = 1, Name = "service 1", Price=23, Description="description1"});
            carServices.Add(new CarService { Id = 2, Name = "service 2", Price = 24, Description = "description2" });
            carServices.Add(new CarService { Id = 3, Name = "service 3", Price = 25, Description = "description3" });
            return carServices;
            //return await _context.CarServices.ToListAsync();
        }

        [HttpGet("locations")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            List<Location> locations = new List<Location>();
            locations.Add(new Location { Id = 1, Latitude=54, Longitude=21, Address="Adfa"});
            locations.Add(new Location { Id = 2, Latitude = 55, Longitude = 22, Address = "Adfa2" });
            locations.Add(new Location { Id = 3, Latitude = 56, Longitude = 23, Address = "Adfa3" }); 
            return locations;
            //return await _context.Locations.ToListAsync();
        }   

        // GET: api/Car/5
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

        // POST: api/Car
        [HttpPost]
        public async Task<ActionResult<Car>> Post([FromBody] Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = car.Id }, car);
        }

        // PUT: api/Car/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Car/5
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
    }
}
