using Application.DTO;
using Domain.Entities;

namespace Application.Interfaces;

public interface IVoteService
{
    Task PublishVoteAsync(VoteRequest vote);
}