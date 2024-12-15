using Application.DTO;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class VoteService : IVoteService
{
    private readonly IMessagingService _messagingService;

    public VoteService(IMessagingService messagingService)
    {
        _messagingService = messagingService;
    }

    public async Task PublishVoteAsync(VoteRequest vote)
    {
        if (vote == null)
            throw new ArgumentNullException(nameof(vote));
        await _messagingService.PublishMessageAsync("vote_queue", vote);
    }
}