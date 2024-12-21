using Domain.Entities;

namespace Domain.Interfaces;

public interface IVoteRepository
{
    Task ProcessVote(int candidateId);
}