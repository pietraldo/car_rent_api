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
            return await _context.CarDetails.ToListAsync();
        }

        [HttpGet("cardetails/{car_id}")]
        public async Task<ActionResult<IEnumerable<CarDetail>>> GetCarDetailsId(int car_id)
        {
            Car? car = await _context.Cars.Include(car => car.Details).FirstOrDefaultAsync(x => x.Id == car_id);

            return (car != null) ? Ok(car.Details) : Ok(new List<CarDetail>());
        }

        [HttpGet("cardetails/search/{search}")]
        public async Task<ActionResult<IEnumerable<CarDetail>>> GetCarDetailsId(string search)
        {
            var filteredCarDetails = await _context.CarDetails.Where(x => x.Description.Contains(search)).ToListAsync();
            return Ok(filteredCarDetails);
        }

        [HttpGet("carservices")]
        public async Task<ActionResult<IEnumerable<CarService>>> GetCarServices()
        {
            return await _context.CarServices.ToListAsync();
        }

        [HttpGet("carservices/{car_id}")]
        public async Task<ActionResult<CarService>> GetCarServiceById(int car_id)
        {
            Car? car = await _context.Cars.Include(c => c.Services).FirstOrDefaultAsync(x => x.Id == car_id);
            return (car != null) ? Ok(car.Services) : Ok(new List<CarService>());
        }

        [HttpGet("carservices/search/{search}")]
        public async Task<ActionResult<IEnumerable<CarService>>> SearchCarServices(string search)
        {

            var filteredServices = await _context.CarServices
                 .Where(x => x.Name.ToLower().Contains(search.ToLower()) ||
                             x.Description.ToLower().Contains(search.ToLower()))
                 .ToListAsync();

            return Ok(filteredServices);
        }


        [HttpGet("locations")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            return await _context.Locations.ToListAsync();
        }

        [HttpGet("location/{car_id}")]
        public async Task<ActionResult<Location>> GetLocationByCarId(int car_id)
        {
            Car? car = await _context.Cars.Include(car => car.Location).FirstOrDefaultAsync(x => x.Id == car_id);

            return (car != null) ? Ok(car.Location) : Ok();
        }

        [HttpGet("location/search/{search}")]
        public async Task<ActionResult<IEnumerable<Location>>> SearchLocation(string search)
        {
            List<Location> locations = await _context.Locations
            .Where(l =>
                l.Name.ToLower().Contains(search.ToLower()) ||
                l.Address.ToLower().Contains(search.ToLower()))
            .ToListAsync();

            return Ok(locations);
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

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = car.Id }, car);
        }


       
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }
          
            // Get existing car with all related entities
            var existingCar = await _context.Cars
                .Include(c => c.Location)
                .Include(c => c.Details)
                .Include(c => c.Services)
                .FirstOrDefaultAsync(c => c.Id == id);
            if(existingCar==null) return NotFound();
            
            existingCar.Brand = car.Brand;
            existingCar.Model = car.Model;
            existingCar.Year = car.Year;
            existingCar.Photo = car.Photo;
            existingCar.Price = car.Price;


            //seting location
            if (car.Location!=null && car.Location.Id==-1)
            {
                car.Location.Id = 0;
                _context.Locations.Add(car.Location);
                existingCar.Location = car.Location;
            }
            else if (car.Location != null)
            {
                var existingLocation = await _context.Locations.FindAsync(car.Location.Id);
                if (existingLocation != null)
                {
                    existingCar.Location = existingLocation;
                }
            }

            // removing details that are not in the new car
            var detailsToRemove = existingCar.Details
            .Where(d => !car.Details.Any(cd => cd.Id == d.Id && cd.Id != -1))
            .ToList();

            foreach (var detail in detailsToRemove)
            {
                _context.CarDetails.Remove(detail);
            }

            // addoing new and those that are not yet in existing car
            foreach (var detail in car.Details)
            {
                if (detail.Id == -1)
                {
                    detail.Id = 0;
                    _context.CarDetails.Add(detail);
                    existingCar.Details.Add(detail);
                }
                else if(!existingCar.Details.Exists(d => d.Id == detail.Id))
                {
                    existingCar.Details.Add(detail);
                }
            }


            // seting services

            // Removing services that are not in the new car
            var servicesToRemove = existingCar.Services
                .Where(s => !car.Services.Any(cs => cs.Id == s.Id && cs.Id != -1))
                .ToList();

            foreach (var service in servicesToRemove)
            {
                _context.CarServices.Remove(service);  
            }

            foreach (var service in car.Services)
            {
                if (service.Id == -1)  
                {
                    service.Id = 0;  
                    _context.CarServices.Add(service);  
                    existingCar.Services.Add(service);  
                }
                else if (!existingCar.Services.Exists(s => s.Id == service.Id))  
                {
                    existingCar.Services.Add(service);  
                }
            }

            await _context.SaveChangesAsync();
            
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
