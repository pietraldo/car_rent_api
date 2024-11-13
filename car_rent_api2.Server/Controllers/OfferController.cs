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
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            List<string> offers = new List<string>();
            offers.Add("Offer 1");
            offers.Add("Offer 2");
            string a = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
                offers.Add((string)de.Key);


            return offers;
        }

        
    }
}
