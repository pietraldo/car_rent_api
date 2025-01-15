using car_rent_api2.Server.Models;
using car_rent_api2.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using car_rent_api2.Server.DTOs;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<List<Offer>>>> GetOffers(DateTime startDate, 
            DateTime endDate, string brand="", string model="", string clientId="")
        {             
            var cars = await FilterCars(brand, model, startDate, endDate);

            List<Offer> offers = new List<Offer>();
            foreach (var car in cars)
            {    
                Offer offer = new Offer(car, clientId, car.Price, startDate, endDate);
                offers.Add(offer);
                offer.Id = _offerManager.AddOffer(offer);
            }   
            return Ok(offers);
        }

        [HttpGet("rentCar/{offerId}/{clientId}")]
        public async Task<ActionResult<RentInfoForClient>> Get(string offerId, string clientId)
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

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
            if (client==null) 
            {
                return BadRequest("Client not found");
            }

            _context.Entry(offer.Car).State = EntityState.Unchanged;
            Rent rent = new (offer.Car, client, offer.StartDate, offer.EndDate, offer.Price, RentStatus.Reserved, "","");

            _context.Rents.Add(rent);
            await _context.SaveChangesAsync();

            RentInfoForClient rentInfo = new RentInfoForClient(rent);

            return Ok(rentInfo);
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
        
        [HttpGet("id/{id}")]
        public ActionResult<Offer> GetOfferById(Guid id)
        {
            var offer = _offerManager.GetOffer(id);
            
            if (offer == null)
            {
                return NotFound();
            }
            return Ok(new OfferToDisplay(offer));
        }

        private async Task<List<Car>> FilterCars(string brand, string model, DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate) return new List<Car>();
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

            // Filter by availability
            var cars = await carsQuery
                .Where(c => !_context.Rents.Any(r =>
                    r.Car.Id == c.Id &&
                    (r.StartDate < endDate && r.EndDate > startDate))) 
                .ToListAsync();

            return cars;
        }
    

    }
}
