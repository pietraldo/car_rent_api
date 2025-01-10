using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace car_rent_api2.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly BlobServiceClient _blobServiceClient;

        public ImagesController(IWebHostEnvironment env, BlobServiceClient blobServiceClient)
        {
            _env = env;
            _blobServiceClient = blobServiceClient;
        }

        [HttpGet("{imageName}")]
        public IActionResult GetImage(string imageName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("images");
            BlobClient blobClient = containerClient.GetBlobClient(imageName);
            
            if (!blobClient.Exists())
            {
                return NotFound();
            }
            
            return File(blobClient.OpenRead(), blobClient.GetProperties().Value.ContentType);
        }
        
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            Random r = new Random();
            int rInt = r.Next(0, int.MaxValue);

            // Create a unique filename (optional)
            var fileName = Path.GetFileName(file.FileName); // Use original file name, or generate a unique one
            string fileName2 = fileName.Split('.')[0] + rInt.ToString() + "." + fileName.Split('.')[1];

            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("images");
            BlobClient blobClient = containerClient.GetBlobClient(fileName2);
            
            // upload file
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }
            
            return Ok(new { FilePath = $"api/images/{fileName2}" });
        }
    }

}
