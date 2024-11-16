using Microsoft.AspNetCore.Mvc;

namespace car_rent_api2.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public ImagesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("{imageName}")]
        public IActionResult GetImage(string imageName)
        {
            var imagePath = Path.Combine(_env.WebRootPath, "images", imageName);

            if (System.IO.File.Exists(imagePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(imagePath);
                return File(fileBytes, "image/jpeg"); 
            }
            return NotFound();
        }
    }
}
