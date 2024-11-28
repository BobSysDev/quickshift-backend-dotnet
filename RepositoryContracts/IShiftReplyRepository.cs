using Entities;
namespace RepositoryContracts;


public interface IShiftReplyRepository 
{
    Task<ShiftSwitchReply> AddAsync(ShiftSwitchReply shiftSwitchReply);
    Task<ShiftSwitchReply> UpdateAsync(ShiftSwitchReply shiftSwitchReply);
    Task DeleteAsync(long id);
    IQueryable<ShiftSwitchReply> GetManyAsync();
    Task<ShiftSwitchReply> GetSingleAsync(long id);
    Task<bool> IsReplyInRepository(long id);
    Task<ShiftSwitchReply> SetTargetAcceptedAsync(long id, bool accepted);
    Task<ShiftSwitchReply> SetOriginAcceptedAsync(long id, bool accepted);
}