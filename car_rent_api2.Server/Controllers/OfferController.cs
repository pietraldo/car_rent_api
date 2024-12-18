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
    public class OfferController : ControllerBase
    {
        private readonly CarRentDbContext _context;
        private readonly IOfferManager _offerManager;

        public OfferController(CarRentDbContext context, IOfferManager offerManager)
        {
            _context = context;
            _offerManager = offerManager;
        }

        // GET: api/Offer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<List<Offer>>>> Get(DateTime startDate, 
            DateTime endDate, string brand="", string model="", string clientId="")
        {
            if (startDate >= endDate)
            {
                return BadRequest("Invalid date range. Start date must be earlier than end date.");
            }

            var carsQuery = _context.Cars
                .Where(c => c.Status == CarStatus.Available)
                .Include(c => c.Details)
                .Include(c => c.Services)
                .Include(c=>c.Location).AsQueryable();

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

            List<Offer> offers = new List<Offer>();
            foreach (var car in cars)
            {
                Offer offer = new Offer
                {
                    Car = car,
                    ClientId = clientId,
                    Price = car.Price,
                    StartDate = startDate,
                    EndDate = endDate
                };
                offers.Add(offer);
            }   
            foreach(var offer in offers)
            {
                offer.Id = _offerManager.AddOffer(offer);
            }

            return Ok(offers);
        }

        [HttpPost("createClient")]
        public async Task<ActionResult<string>> CreateClient([FromBody] Client client)
        {
            if (client == null || string.IsNullOrWhiteSpace(client.Id))
            {
                return BadRequest("Invalid client data");
            }

            if (await _context.Clients.AnyAsync(c => c.Id == client.Id))
            {
                return BadRequest("Client already exists");
            }

            _context.Clients.Add(client);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving client: {ex.Message}");
            }

            return Ok(client.Id);
        }

        [HttpGet("checkClient/{id}")]
        public async Task<ActionResult<string>> CheckClient(string id)
        {
            if (id == "")
            {
                return BadRequest("Client id must be provided");
            }

            if (await _context.Clients.AnyAsync(c => c.Id == id))
            {
                return Ok("Client found");
            }
            else
            {
                return NotFound("Client not found");
            }
        }



        [HttpGet("rentCar/{offerId}/{clientId}")]
        public async Task<ActionResult<string>> Get(string offerId, string clientId)
        {
            if(!Guid.TryParse(offerId, out Guid guid))
            {
                return BadRequest("Invalid offer id");
            }

            Offer? offer = _offerManager.GetOffer(guid);
            if(offer == null)
            {
                return BadRequest("Offer not found or expired");
            }

            if (offer.ClientId != "" && offer.ClientId!=clientId)
            {
                return BadRequest("You must be logged in");
            }

            // check if we have user with clientId in database
            if (!await _context.Clients.AnyAsync(c => c.Id == clientId)) 
            {
                return BadRequest("Client not found");
            }


           

            Rent rent = new Rent
            {
                CarId = offer.Car.Id,
                ClientId = clientId,
                StartDate = offer.StartDate,
                EndDate = offer.EndDate,
                Price = offer.Price,
                Status = RentStatus.Reserved,
                Notes = "",
                LinkToPhotos = ""
            };
            _context.Rents.Add(rent);
            await _context.SaveChangesAsync();

            return Ok("car rent succesfull");
        }
        
        [HttpGet("offers")]
        public ActionResult<IEnumerable> GetAllOffers()
        {
            return Ok(_offerManager.GetAllOffers());
        }        
        
        [HttpGet("rents")]
        public ActionResult<IEnumerable<Rent>> GetAllRents()
        {
            return Ok(_context.Rents);
        }
    }
}
