using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace car_rent_api2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly CarRentDbContext _context;

        public OfferController(CarRentDbContext context)
        {
            _context = context;
        }

        // GET: api/Offer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get(DateTime startDate, DateTime endDate, string brand="", string model="")
        {
            if (startDate >= endDate)
            {
                return BadRequest("Invalid date range. Start date must be earlier than end date.");
            }

            var carsQuery = _context.Cars
                .Where(c => c.Status == "available")
                .Include(c => c.Details)
                .Include(c => c.Services).AsQueryable();

            // Filter by brand if specified
            if (!string.IsNullOrEmpty(brand))
            {
                carsQuery = carsQuery.Where(c => c.Brand == brand);
            }

            // Filter by model if specified
            if (!string.IsNullOrEmpty(model))
            {
                carsQuery = carsQuery.Where(c => c.Model == model);
            }

            
            var cars = await carsQuery
                .Where(c => !_context.Rents.Any(r =>
                    r.CarId == c.Id &&
                    (r.StartDate < endDate && r.EndDate > startDate))) 
                .ToListAsync();

            return Ok(cars);
        }

        
    }
}
