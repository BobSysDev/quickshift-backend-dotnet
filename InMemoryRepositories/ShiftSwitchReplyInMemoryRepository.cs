
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class ShiftSwitchReplyInMemoryRepository : IShiftSwitchReplyRepository
{
    private List<ShiftSwitchReply> replies = new List<ShiftSwitchReply>();
    
    public async Task<ShiftSwitchReply> AddAsync(ShiftSwitchReply shiftSwitchReply)
    {
        if (shiftSwitchReply.Id == 0)
        {
            shiftSwitchReply.Id = replies.Any() ? replies.Max(r => r.Id) + 1 : 1;
        }
        replies.Add(shiftSwitchReply);
        return shiftSwitchReply;
    }


    public async Task<ShiftSwitchReply> UpdateAsync(ShiftSwitchReply shiftSwitchReply)
    {
        var existingReply = replies.SingleOrDefault(r => r.Id == shiftSwitchReply.Id);

        if (existingReply is null) throw new InvalidOperationException($"Reply with ID {shiftSwitchReply.Id} not found.");

        replies.Remove(existingReply);
        replies.Add(shiftSwitchReply);
        return shiftSwitchReply;
    }

    public async Task DeleteAsync(long id)
    {
        var replyToRemove = replies.SingleOrDefault(r => r.Id == id);
        if (replyToRemove is null) throw new InvalidOperationException($"Reply with ID {id} not found.");
        replies.Remove(replyToRemove);
    }

    public IQueryable<ShiftSwitchReply> GetManyAsync()
    {
        return replies.AsQueryable();
    }

    public async Task<ShiftSwitchReply> GetSingleAsync(long id)
    {
        var reply = replies.FirstOrDefault(r => r.Id == id);
        if (reply is null) throw new InvalidOperationException($"Reply with ID {id} not found.");
        return reply;
    }
    
    public async Task<bool> SetTargetAcceptedAsync(long id, bool accepted)
    {
        var reply = replies.SingleOrDefault(r => r.Id == id);
        if (reply is null) throw new InvalidOperationException($"Reply with ID {id} not found.");
        reply.TargetAccepted = accepted;
        return await Task.FromResult(true);
    }

    public async Task<bool> SetOriginAcceptedAsync(long id, bool accepted)
    {
        var reply = replies.SingleOrDefault(r => r.Id == id);
        if (reply is null) throw new InvalidOperationException($"Reply with ID {id} not found.");
        reply.OriginAccepted = accepted;
        return await Task.FromResult(true);
    }
}