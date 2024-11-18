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

        // GET: api/Car
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Car>>> Get(string status)
        {
            return await _context.Cars.Where(c=>c.Status==status).ToListAsync();
        }

        
    }
}
