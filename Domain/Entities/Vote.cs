namespace Domain.Entities;

public class Vote
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public DateTime VotedAt { get; set; }
    
    public virtual Candidate? Candidate { get; set; }
}