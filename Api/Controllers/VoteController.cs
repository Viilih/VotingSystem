using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/vote")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _voteService;

        public VoteController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpPost("publish-vote")]
        public async Task<IActionResult> PublishVote([FromBody] VoteRequest voteRequest)
        {
            
            if (voteRequest == null || voteRequest.CandidateId <= 0)
                return BadRequest("Invalid vote request.");

            await _voteService.PublishVoteAsync(voteRequest);

            return Ok("Your vote was computed");
        }
    }
}
