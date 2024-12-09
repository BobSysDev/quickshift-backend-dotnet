using Entities;
namespace RepositoryContracts;


public interface IShiftSwitchReplyRepository 
{
    Task<ShiftSwitchReply> AddAsync(ShiftSwitchReply shiftSwitchReply);
    Task<ShiftSwitchReply> UpdateAsync(ShiftSwitchReply shiftSwitchReply);
    Task DeleteAsync(long id);
    IQueryable<ShiftSwitchReply> GetManyAsync();
    Task<ShiftSwitchReply> GetSingleAsync(long id);
    Task<bool> SetTargetAcceptedAsync(long id, bool accepted);
    Task<bool> SetOriginAcceptedAsync(long id, bool accepted);
}