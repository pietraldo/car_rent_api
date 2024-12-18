using car_rent_api2.Server.Models;
using car_rent_api2.Server.Services;
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
    public class RentController : ControllerBase
    {
        private readonly CarRentDbContext _context;

        public RentController(CarRentDbContext context)
        {
            _context = context;
        }

        [HttpGet("getrents")]
        public async Task<ActionResult<IEnumerable<Rent>>> GetRents()
        {
            return await _context.Rents.Include(c=>c.Client).Include(c=>c.Car).ToListAsync();
        }

        [HttpPost("uploadnote")]
        public async Task<ActionResult> UploadNote(int id, string note, string linkToPhotos)
        {
            var rent = await _context.Rents.FirstOrDefaultAsync(r=>r.Id==id);
            if (rent == null)
            {
                return NotFound();
            }

            rent.Notes = note;
            rent.LinkToPhotos = linkToPhotos;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
