using Entities;
using RepositoryContracts;

namespace RepositoryProxies;

public class ShiftSwitchReplyProxy : IShiftSwitchReplyRepository
{
    public Task<ShiftSwitchReply> AddAsync(ShiftSwitchReply shiftSwitchReply)
    {
        throw new NotImplementedException();
    }

    public Task<ShiftSwitchReply> UpdateAsync(ShiftSwitchReply shiftSwitchReply)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<ShiftSwitchReply> GetManyAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ShiftSwitchReply> GetSingleAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsReplyInRepository(long id)
    {
        throw new NotImplementedException();
    }

    public Task<ShiftSwitchReply> SetTargetAcceptedAsync(long id, bool accepted)
    {
        throw new NotImplementedException();
    }

    public Task<ShiftSwitchReply> SetOriginAcceptedAsync(long id, bool accepted)
    {
        throw new NotImplementedException();
    }
}