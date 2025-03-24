using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Dl.Entities;
namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadFileController : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;
        public UploadFileController(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }
        [HttpGet("presigned-url")]
        [Authorize]
        public async Task<IActionResult> GetPresignedUrl([FromQuery] string fileName)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = "login-user-bucket-testpnoren",
                Key = fileName,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(5),
                ContentType = "image/jpeg"
            };

            string url = _s3Client.GetPreSignedURL(request);
            return Ok(new { url , });
        }

        [HttpGet("list-files")]
        [Authorize]
        public async Task<IActionResult> ListFiles()
        {           
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = "login-user-bucket-testpnoren",
                    
                };

                var response = await _s3Client.ListObjectsV2Async(request);
                var fileNames = response.S3Objects.Select(obj => obj.Key).ToList();

                return Ok(fileNames);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בקבלת רשימת הקבצים: {ex.Message}");
            }
        }

    }
}
