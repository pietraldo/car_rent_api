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
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Ensure the "images" directory exists
            var imageDirectory = Path.Combine(_env.WebRootPath, "images");
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }
            Random r = new Random();
            int rInt = r.Next(0, int.MaxValue);
            

            // Create a unique filename (optional)
            var fileName = Path.GetFileName(file.FileName); // Use original file name, or generate a unique one
            string fileName2 = fileName.Split('.')[0] + rInt.ToString() + "." + fileName.Split('.')[1];
            var filePath = Path.Combine(imageDirectory, fileName2);

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { FilePath = $"api/images/{fileName2}" });
        }
    }

}
