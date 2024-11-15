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

        [HttpGet("cardetails/{id}")]
        public async Task<ActionResult<IEnumerable<CarDetail>>> GetCarDetailsId(int id)
        {
            List<CarDetail> carDetails = new List<CarDetail>();
            carDetails.Add(new CarDetail { Id = 1, Description = "dec id 1", Value = "Value 1" });
            carDetails.Add(new CarDetail { Id = 2, Description = "dec id 2", Value = "Value 2" });
            return carDetails;
            //return await _context.CarDetails.ToListAsync();
        }

        [HttpGet("cardetails/search/{search}")]
        public async Task<ActionResult<IEnumerable<CarDetail>>> GetCarDetailsId(string search)
        {
            List<CarDetail> carDetails = new List<CarDetail>
            {
                new CarDetail { Id = 1, Description = "dec id 1", Value = "Value 1" },
                new CarDetail { Id = 2, Description = "dec id 2", Value = "Value 2" },
                new CarDetail { Id = 3, Description = "dec id 2", Value = "Value 3" },
                new CarDetail { Id = 4, Description = "dec id 2", Value = "Value 4" },
                new CarDetail { Id = 5, Description = "dec id 1", Value = "Value 5" }
            };

            var filteredCarDetails = carDetails.Where(x => x.Description.Contains(search));
            return Ok(filteredCarDetails);
        }

        [HttpGet("carservices")]
        public async Task<ActionResult<IEnumerable<CarService>>> GetCarServices()
        {
            List<CarService> carServices = new List<CarService>();
            carServices.Add(new CarService { Id = 1, Name = "service 1", Price = 23, Description = "description1" });
            carServices.Add(new CarService { Id = 2, Name = "service 2", Price = 24, Description = "description2" });
            carServices.Add(new CarService { Id = 3, Name = "service 3", Price = 25, Description = "description3" });
            return carServices;
            //return await _context.CarServices.ToListAsync();
        }
        [HttpGet("carservices/{id}")]
        public async Task<ActionResult<CarService>> GetCarServiceById(int id)
        {
            List<CarService> carServices = new List<CarService>
            {
                new CarService { Id = 1, Name = "service 1", Price = 23, Description = "description1" },
                new CarService { Id = 2, Name = "service 2", Price = 24, Description = "description2" },
                new CarService { Id = 3, Name = "service 3", Price = 25, Description = "description3" }
            };

            //var carService = carServices.FirstOrDefault(x => x.Id == id);



            return Ok(carServices);
        }

        [HttpGet("carservices/search/{search}")]
        public async Task<ActionResult<IEnumerable<CarService>>> SearchCarServices(string search)
        {
            List<CarService> carServices = new List<CarService>
            {
                new CarService { Id = 1, Name = "service 1", Price = 23, Description = "description1" },
                new CarService { Id = 2, Name = "service 2", Price = 24, Description = "description2" },
                new CarService { Id = 3, Name = "service 3", Price = 25, Description = "description3" },
                new CarService { Id = 4, Name = "special service", Price = 30, Description = "special description" }
            };

            var filteredServices = carServices.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                                           x.Description.Contains(search, StringComparison.OrdinalIgnoreCase));

            return Ok(filteredServices);
        }


        [HttpGet("locations")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            List<Location> locations = new List<Location>();
            locations.Add(new Location { Id = 1, Latitude = 54, Longitude = 21, Address = "Adfa" });
            locations.Add(new Location { Id = 2, Latitude = 55, Longitude = 22, Address = "Adfa2" });
            locations.Add(new Location { Id = 3, Latitude = 56, Longitude = 23, Address = "Adfa3" });
            return locations;
            //return await _context.Locations.ToListAsync();
        }

        [HttpGet("location/{id}")]
        public async Task<ActionResult<Location>> GetLocationByCarId(int id)
        {
            // Simulated data
            var location = new Location
            {
                Id = id,
                Name = "Default Location",
                Address = "123 Main St",
                Latitude = 40.7128,
                Longitude = -74.0060
            };

            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        [HttpGet("location/search/{search}")]
        public async Task<ActionResult<IEnumerable<Location>>> SearchLocation(string search)
        {
            // Simulated data
            var locations = new List<Location>
            {
                new Location { Id = 1, Name = "Location 1", Address = "123 Main St", Latitude = 40.7128, Longitude = -74.0060 },
                new Location { Id = 2, Name = "Location 2", Address = "456 Elm St", Latitude = 34.0522, Longitude = -118.2437 },
                new Location { Id = 3, Name = "Special Location", Address = "789 Oak St", Latitude = 37.7749, Longitude = -122.4194 }
            };

            var filteredLocations = locations.Where(l =>
                l.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                l.Address.Contains(search, StringComparison.OrdinalIgnoreCase));

            return Ok(filteredLocations);
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
            // Handle CarDetails - Add new details if their Id is -1 (new)
            for (int i = 0; i < car.Details.Count; i++)
            {
                var detail = car.Details[i];
                if (detail.Id == -1)
                {
                    detail.Id = 0; // Reset Id to 0 (EF Core will generate a new Id)
                    _context.CarDetails.Add(detail); // Add new CarDetail
                }
                else
                {
                    // Optionally, check if the CarDetail already exists and attach it
                    var existingDetail = await _context.CarDetails.FindAsync(detail.Id);
                    if (existingDetail != null)
                    {
                        _context.Entry(existingDetail).State = EntityState.Unchanged; // Mark as unchanged
                        car.Details[i] = existingDetail; // Link the existing detail to the current car
                    }
                }
            }

            // Handle CarServices - Add new services if their Id is -1 (new)
            for (int i = 0; i < car.Services.Count; i++)
            {
                var service = car.Services[i];
                if (service.Id == -1)
                {
                    service.Id = 0; // Reset Id to 0 (EF Core will generate a new Id)
                    _context.CarServices.Add(service); // Add new CarService
                }
                else
                {
                    // Optionally, check if the CarService already exists and attach it
                    var existingService = await _context.CarServices.FindAsync(service.Id);
                    if (existingService != null)
                    {
                        _context.Entry(existingService).State = EntityState.Unchanged; // Mark as unchanged
                        car.Services[i] = existingService; // Link the existing service to the current car
                    }
                }
            }

            // Handle Location - Add new location if it has an Id of -1
            if (car.Location != null && car.Location.Id == -1)
            {
                car.Location.Id = 0; // Reset Id to 0 (EF Core will generate a new Id)
                _context.Locations.Add(car.Location); // Add new Location
            }
            else
            {
                // If Location exists (not new), we link the existing Location.
                var existingLocation = await _context.Locations.FindAsync(car.Location?.Id);
                if (existingLocation != null)
                {
                    car.Location = existingLocation; // Link existing Location
                }
            }

            // Add the Car object itself (with linked CarDetails, CarServices, Location)
            _context.Cars.Add(car);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return a response with the newly created car
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
