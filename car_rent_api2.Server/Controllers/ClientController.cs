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
    public class ClientController : ControllerBase
    {
        private readonly CarRentDbContext _context;

        public ClientController(CarRentDbContext context)
        {
            _context = context;
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
    }
}
