using Bl.DTOs;
using Bl.InterfaceServices;
using Dl.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _voteService;

        public VoteController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpGet("Count")]
        [Authorize]
        public async Task<IActionResult> GetVotesCount(int imageId)
        {
            Console.WriteLine($"GetVotesCount called with imageId: {imageId}");
            try
            {
                int count = await _voteService.GetVotesCountAsync(imageId);
                return Ok(new { imageId, voteCount = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
        [HttpPost]
        [Authorize] // לוודא שהמשתמש מחובר
        public async Task<IActionResult> AddVote([FromBody] VoteDto vote)
        {
            Console.WriteLine($"Received vote: UserId={vote?.UserId}, ImageId={vote?.ImageId}");
            try
            {
                if (vote == null)
                    return BadRequest(new { Message = "Invalid vote data" });
                var VoteData = new Vote
                {
                    UserId = vote.UserId,
                    ImageId =vote.ImageId,
                    CreatedAt = DateTime.Now
                };
                await _voteService.AddVoteAsync(VoteData);
                return Ok(new { Message = "Vote added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }

    }
}
