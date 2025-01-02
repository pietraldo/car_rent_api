using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace car_rent_api2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarDetailsController : ControllerBase
    {
        private readonly CarRentDbContext _context;

        public CarDetailsController(CarRentDbContext context)
        {
            _context = context;
        }

        [HttpGet("cardetails")]
        public async Task<ActionResult<IEnumerable<CarDetail>>> GetCarDetails()
        {
            return await _context.CarDetails.ToListAsync();
        }

        [HttpGet("search/{search}")]
        public async Task<ActionResult<IEnumerable<CarDetail>>> GetCarDetailsId(string search)
        {
            var filteredCarDetails = await _context.CarDetails.Where(x => x.Description.Contains(search)).ToListAsync();
            return Ok(filteredCarDetails);
        }
    }
}
