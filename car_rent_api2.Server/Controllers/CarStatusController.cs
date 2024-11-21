using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace car_rent_api2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarStatusController : ControllerBase
    {
        private readonly CarRentDbContext _context;

        public CarStatusController(CarRentDbContext context)
        {
            _context = context;
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Car>>> Get(string status)
        {
            return await _context.Cars.Where(c=>c.Status==status).ToListAsync();
        }

        [HttpGet("currentstatuses")]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            return new List<string>() { "rented", "available", "norented", "in_repair"};
            //return await _context.Cars.Select(c => c.Status).Distinct().ToListAsync();
        }
    }
}
