using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WebApiJWTAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromBody] ImageUploadModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Base64Image))
            {
                return BadRequest("No image data provided.");
            }

            try
            {
                // Decode base64 string to byte array
                byte[] imageBytes = Convert.FromBase64String(model.Base64Image);

                // Generate a unique filename for the image
                string imageName = $"{Guid.NewGuid()}.png";

                // Specify the directory where you want to save the image
                string imagePath = Path.Combine("wwwroot", "images", imageName);

                bool exists = System.IO.Directory.Exists(Path.Combine("wwwroot", "images"));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Path.Combine("wwwroot", "images"));

                // Save the image to the specified path
                await System.IO.File.WriteAllBytesAsync(imagePath, imageBytes);

                // Construct the URL for the saved image
                string imageUrl = $"{Request.Scheme}://{Request.Host}/images/{imageName}";

                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

    public class ImageUploadModel
    {
        public string Caption { get; set; }
        public string Base64Image { get; set; }
    }
}
