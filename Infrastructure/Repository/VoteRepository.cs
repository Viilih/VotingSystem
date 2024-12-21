using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class VoteRepository :  IVoteRepository
{
    private readonly ApplicationDbContext _context;

    public VoteRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task ProcessVote(int candidateId)
    {
        var candidate = await _context.Candidates
            .FirstOrDefaultAsync(c => c.Id == candidateId);
        
        if (candidate == null)
        {
            throw new InvalidOperationException($"Candidate with ID {candidateId} not found");
        }

        var vote = new Vote
        {
            CandidateId = candidateId,
            VotedAt = DateTime.UtcNow
        };
        Console.WriteLine($"Vim ate o repositorio {candidate} {vote}");
        _context.Votes.Add(vote);
        await _context.SaveChangesAsync();
    }
}