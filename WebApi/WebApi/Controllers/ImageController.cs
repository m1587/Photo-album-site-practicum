using Bl.DTOs;
using Bl.InterfaceServices;
using Bl.Services;
using Dl.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using Image = Dl.Entities.Image;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            this._imageService = imageService;
        }
        [HttpGet("Name")]
        [Authorize]
        public async Task<IActionResult> GetImagesByNameAsync([FromQuery] string imageName)
        {
            Console.WriteLine($"📌 קיבלתי קריאה עם imageName: {imageName}");
            try
            {
                var image = await _imageService.GetImagesByNameAsync(imageName);
                return image != null ? Ok(image) : NotFound(new { Message = "Image not found" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("eror");
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
        //[HttpGet("challenge/{id}")]
        //[Authorize]
        //public async Task<IActionResult> GetImagesByChallengeIdAsync([FromBody] int challengeId)
        //{
        //    try
        //    {
        //        var image = await _imageService.GetImagesByChallengeIdAsync(challengeId);
        //        return image != null ? Ok(image) : NotFound(new { Message = "User not found" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
        //    }
        //}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetImagesByIdAsync(int id)
        {
            try
            {
                var image = await _imageService.GetImagesByIdAsync(id);
                return image != null ? Ok(image) : NotFound(new { Message = "Image not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddImageAsync([FromBody] ImageDto image)
        {
            try
            {
                if (image == null || string.IsNullOrEmpty(image.ImageURL))
                {
                    return BadRequest(new { Message = "Invalid image data" });
                }
                var imageData = new Image
                {
                    UserId = image.UserId,
                    ImageURL = image.ImageURL,
                    Caption = image.Caption,
                    UploadedAt = DateTime.Now
                };
                await _imageService.AddImageAsync(imageData);
                Console.WriteLine($"Created image ID: {imageData.ID}");
                return Ok(new { Message = "Image added successfully", Image = imageData });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddImageAsync: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
    }
}
