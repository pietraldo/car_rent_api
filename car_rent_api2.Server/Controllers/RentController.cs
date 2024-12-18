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
        public class EditNoteRequest
        {
            public int Id { get; set; }
            public string Note { get; set; }
            public string LinkToPhotos { get; set; }
        }

        [HttpPost("editnote")]
        public async Task<ActionResult> EditNote([FromBody] EditNoteRequest request)
        {
            var rent = await _context.Rents.FirstOrDefaultAsync(r => r.Id == request.Id);
            if (rent == null)
            {
                return NotFound();
            }

            rent.Notes = request.Note;
            rent.LinkToPhotos = request.LinkToPhotos;

            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        public class RentStatusRequest
        {
            public int RentId { get; set; }
        }

        private void sendEmailWithBill()
        {
            // Send email logic here
        }

        [HttpPost("acceptReturn")]
        public async Task<ActionResult> AcceptReturn([FromBody] RentStatusRequest rentStatusRequest)
        {
            var rent = await _context.Rents.FirstOrDefaultAsync(r => r.Id == rentStatusRequest.RentId);
            if (rent == null)
            {
                return NotFound();
            }

            if(rent.Status != RentStatus.ReadyToReturn)
            {
                return BadRequest("Invalid rent status. Expected ReadyToReturn.");
            }

            rent.Status = RentStatus.Finished;

            await _context.SaveChangesAsync();

            sendEmailWithBill();

            return Ok(new { success = true });
        }

        [HttpPost("readyToReturn")]
        public async Task<ActionResult> ReadyToReturn([FromBody] RentStatusRequest rentStatusRequest)
        {
            var rent = await _context.Rents.FirstOrDefaultAsync(r => r.Id == rentStatusRequest.RentId);
            if (rent == null)
            {
                return NotFound();
            }

            if (rent.Status != RentStatus.Active)
            {
                return BadRequest("Invalid rent status. Expected Active.");
            }

            rent.Status = RentStatus.ReadyToReturn;

            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        [HttpPost("pickedUpByClient")]
        public async Task<ActionResult> PickedUpByClient([FromBody] RentStatusRequest rentStatusRequest)
        {
            var rent = await _context.Rents.FirstOrDefaultAsync(r => r.Id == rentStatusRequest.RentId);
            if (rent == null)
            {
                return NotFound();
            }

            if (rent.Status != RentStatus.Reserved)
            {
                return BadRequest("Invalid rent status. Expected Reserved.");
            }

            rent.Status = RentStatus.Active;

            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }
    }
}
