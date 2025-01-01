using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace car_rent_api2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly CarRentDbContext _context;

        public LocationController(CarRentDbContext context)
        {
            _context = context;
        }

        [HttpGet("locations")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            return await _context.Locations.ToListAsync();
        }

        [HttpGet("search/{search}")]
        public async Task<ActionResult<IEnumerable<Location>>> SearchLocation(string search)
        {
            List<Location> locations = await _context.Locations
            .Where(l =>
                l.Name.ToLower().Contains(search.ToLower()) ||
                l.Address.ToLower().Contains(search.ToLower()))
            .ToListAsync();

            return Ok(locations);
        }
    }
}
