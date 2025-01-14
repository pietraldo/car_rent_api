using car_rent_api2.Server.Models;
using car_rent_api2.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using car_rent_api2.Server.DTOs;
using car_rent_api2.Server.Notifications;

namespace car_rent_api2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly CarRentDbContext _context;
        private readonly INotificationService _notificationService;

        public RentController(CarRentDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        [HttpGet("getrents")]
        public async Task<ActionResult<IEnumerable<Rent>>> GetRents(
            [FromQuery] int offset = 0,
            [FromQuery] string startDate = "",
            [FromQuery] string endDate = "",
            [FromQuery] string status = "",
            [FromQuery] string client = "",
            [FromQuery] string car = ""
            )
        {

            DateTime? startDateParsed = DateTime.TryParse(startDate, out DateTime date) ? date : null;
            DateTime? endDateParsed = DateTime.TryParse(endDate, out DateTime date2) ? date2 : null;


            var statusesParsed = string.IsNullOrEmpty(status)? new List<int>(): status.Split(',').Select(s => (int)Enum.Parse<RentStatus>(s.Trim())).ToList();

            const int limit = 10;
            var rents = await _context.Rents
                .Include(c => c.Client)
                .Include(c => c.Car)
                .Where(r => startDateParsed == null || r.StartDate >= startDateParsed)
                .Where(r => endDateParsed == null || r.EndDate <= endDateParsed)
                .Where(r => !statusesParsed.Any() || statusesParsed.Contains((int)r.Status))
                .Where(r => client == "" || r.Client.Name.ToLower().Contains(client.ToLower()))
                .Where(r => car == "" || r.Car.Brand.ToLower().Contains(car.ToLower()) || r.Car.Model.ToLower().Contains(car.ToLower()))
                .OrderByDescending(r => r.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return Ok(rents);
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

        [HttpPost("acceptReturn")]
        public async Task<ActionResult> AcceptReturn([FromBody] RentStatusRequest rentStatusRequest)
        {
            var rent = await _context.Rents.Include(c => c.Client).Include(c => c.Car).FirstOrDefaultAsync(r => r.Id == rentStatusRequest.RentId);
            if (rent == null)
            {
                return NotFound();
            }

            if (rent.Status != RentStatus.ReadyToReturn)
            {
                return BadRequest("Invalid rent status. Expected ReadyToReturn.");
            }

            rent.Status = RentStatus.Finished;
            rent.Car.Status = CarStatus.Available;

            await _context.SaveChangesAsync();

            _notificationService.Notify(rent);

            return Ok(new { success = true });
        }

        [HttpGet("readyToReturn/{rentId}")]
        public async Task<ActionResult> ReadyToReturn(int rentId)
        {
            var rent = await _context.Rents.Include(c => c.Car).FirstOrDefaultAsync(r => r.Id == rentId);
            if (rent == null)
            {
                return NotFound();
            }

            if (rent.Status != RentStatus.Active)
            {
                return BadRequest("Invalid rent status. Expected Active.");
            }

            rent.Status = RentStatus.ReadyToReturn;
            rent.Car.Status = CarStatus.Returned;

            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        [HttpPost("pickedUpByClient")]
        public async Task<ActionResult> PickedUpByClient([FromBody] RentStatusRequest rentStatusRequest)
        {
            var rent = await _context.Rents.Include(c => c.Car).FirstOrDefaultAsync(r => r.Id == rentStatusRequest.RentId);
            if (rent == null)
            {
                return NotFound();
            }

            if (rent.Status != RentStatus.Reserved)
            {
                return BadRequest("Invalid rent status. Expected Reserved.");
            }

            rent.Status = RentStatus.Active;
            rent.Car.Status = CarStatus.Rented;

            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        [HttpGet("getrent/{rentId}")]
        public async Task<ActionResult<RentInfoForClient>> GetRent(int rentId)
        {
            var rent = await _context.Rents.Include(c => c.Car).FirstOrDefaultAsync(r => r.Id == rentId);
            if (rent == null)
            {
                return NotFound();
            }

            return new RentInfoForClient(rent);
        }
    }
}
