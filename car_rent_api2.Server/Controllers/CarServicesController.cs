using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace car_rent_api2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarServicesController : ControllerBase
    {
        private readonly CarRentDbContext _context;

        public CarServicesController(CarRentDbContext context)
        {
            _context = context;
        }

        [HttpGet("carservices")]
        public async Task<ActionResult<IEnumerable<CarService>>> GetCarServices()
        {
            return await _context.CarServices.ToListAsync();
        }

        [HttpGet("search/{search}")]
        public async Task<ActionResult<IEnumerable<CarService>>> SearchCarServices(string search)
        {

            var filteredServices = await _context.CarServices
                 .Where(x => x.Name.ToLower().Contains(search.ToLower()) ||
                             x.Description.ToLower().Contains(search.ToLower()))
                 .ToListAsync();

            return Ok(filteredServices);
        }
    }
}
