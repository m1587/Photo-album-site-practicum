using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> GetPresignedUrl([FromQuery] string fileName, [FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID is required.");

            string userFolder = $"users/{userId}/"; // שמירת קבצים לכל משתמש בתיקיה נפרדת
            string fileKey = userFolder + fileName;

            var request = new GetPreSignedUrlRequest
            {
                BucketName = "login-user-bucket-testpnoren",
                Key = fileKey,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(5),
                //ContentType = "image/jpg" // או סוג הקובץ המתאים
            };

            string url = _s3Client.GetPreSignedURL(request);
            return Ok(new { url });
        }
        [HttpGet("list-files")]
        [Authorize]
        public async Task<IActionResult> ListFiles([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID is required.");

            string userFolder = $"users/{userId}/";

            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = "login-user-bucket-testpnoren",
                    Prefix = userFolder // מציג רק קבצים של המשתמש המחובר
                };

                var response = await _s3Client.ListObjectsV2Async(request);
                var fileNames = response.S3Objects.Select(obj => obj.Key.Replace(userFolder, "")).ToList();

                return Ok(fileNames);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בקבלת רשימת הקבצים: {ex.Message}");
            }
        }

        [HttpDelete("delete-file")]
        [Authorize]
        public async Task<IActionResult> DeleteFile([FromQuery] string fileName, [FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID is required.");

            string fileKey = $"users/{userId}/{fileName}";

            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = "login-user-bucket-testpnoren",
                    Key = fileKey
                };

                await _s3Client.DeleteObjectAsync(request);
                return Ok($"הקובץ {fileName} נמחק בהצלחה.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה במחיקת הקובץ: {ex.Message}");
            }
        }

        [HttpPut("rename-file")]
        [Authorize]
        public async Task<IActionResult> RenameFile([FromQuery] string oldFileName, [FromQuery] string newFileName, [FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID is required.");

            string oldFileKey = $"users/{userId}/{oldFileName}";
            string newFileKey = $"users/{userId}/{newFileName}";

            try
            {
                var copyRequest = new CopyObjectRequest
                {
                    SourceBucket = "login-user-bucket-testpnoren",
                    SourceKey = oldFileKey,
                    DestinationBucket = "login-user-bucket-testpnoren",
                    DestinationKey = newFileKey
                };

                await _s3Client.CopyObjectAsync(copyRequest);

                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = "login-user-bucket-testpnoren",
                    Key = oldFileKey
                };

                await _s3Client.DeleteObjectAsync(deleteRequest);
                Console.WriteLine($"File {oldFileName} renamed to {newFileName} successfully.");
                return Ok($"הקובץ {oldFileName} שונה ל-{newFileName} בהצלחה.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בשינוי שם הקובץ: {ex.Message}");
            }
        }

        [HttpPut("move-file")]
        [Authorize]
        public async Task<IActionResult> MoveFile([FromQuery] string fileName, [FromQuery] string newFolder, [FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            string oldKey = $"users/{userId}/{fileName}";
            string newKey = $"users/{userId}/{newFolder}/{fileName}";


            try
            {
                // בדוק אם הקובץ קיים לפני ההעתקה
                var headRequest = new GetObjectMetadataRequest
                {
                    BucketName = "login-user-bucket-testpnoren",
                    Key = oldKey
                };

                var headResponse = await _s3Client.GetObjectMetadataAsync(headRequest);

                var copyRequest = new CopyObjectRequest
                {
                    SourceBucket = "login-user-bucket-testpnoren",
                    SourceKey = oldKey,
                    DestinationBucket = "login-user-bucket-testpnoren",
                    DestinationKey = newKey,
                };

                await _s3Client.CopyObjectAsync(copyRequest);

                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = "login-user-bucket-testpnoren",
                    Key = oldKey,
                };

                await _s3Client.DeleteObjectAsync(deleteRequest);

                return Ok($"הקובץ {fileName} הועבר לתיקייה {newFolder} בהצלחה.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה בהעברת הקובץ: {ex.Message}");
                return StatusCode(500, $"שגיאה בהעברת הקובץ לתיקייה: {ex.Message}");
            }

        }
        [HttpPut("tag-file")]
        [Authorize]
        public async Task<IActionResult> TagFile([FromQuery] string fileName, [FromQuery] string tagKey, [FromQuery] string tagValue, [FromQuery] string userId)
        {
            try
            {
                var tagRequest = new PutObjectTaggingRequest
                {
                    BucketName = "login-user-bucket-testpnoren",
                    Key = $"users/{userId}/{fileName}",
                    Tagging = new Tagging
                    {
                        TagSet = new List<Tag>
                        {
                            new Tag { Key = tagKey, Value = tagValue }
                        }
                    }
                };

                await _s3Client.PutObjectTaggingAsync(tagRequest);
                return Ok($"תוית {tagKey}: {tagValue} נוספה לקובץ {fileName} בהצלחה.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בהוספת תיוג: {ex.Message}");
            }
        }


        //[HttpGet("search-files")]
        //public async Task<IActionResult> SearchFiles([FromQuery] string? tagKey, [FromQuery] string? tagValue, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        //{
        //    var request = new ListObjectsV2Request
        //    {
        //        BucketName = "login-user-bucket-testpnoren"
        //    };

        //    var response = await _s3Client.ListObjectsV2Async(request);
        //    var files = response.S3Objects;

        //    if (fromDate.HasValue)
        //        files = files.Where(f => f.LastModified >= fromDate.Value).ToList();

        //    if (toDate.HasValue)
        //        files = files.Where(f => f.LastModified <= toDate.Value).ToList();

        //    if (!string.IsNullOrEmpty(tagKey))
        //    {
        //        var filteredFiles = new List<S3Object>();
        //        foreach (var file in files)
        //        {
        //            var taggingResponse = await _s3Client.GetObjectTaggingAsync(new GetObjectTaggingRequest
        //            {
        //                BucketName = "login-user-bucket-testpnoren",
        //                Key = file.Key
        //            });

        //            if (taggingResponse.Tagging.Any(t => t.Key == tagKey && (tagValue == null || t.Value == tagValue)))
        //            {
        //                filteredFiles.Add(file);
        //            }
        //        }
        //        files = filteredFiles;
        //    }

        //    return Ok(files.Select(f => f.Key).ToList());
        //}


    }
}
