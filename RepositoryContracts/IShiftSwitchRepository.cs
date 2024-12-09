using Entities;

namespace RepositoryContracts;


public interface IShiftSwitchRepository
{
    Task<ShiftSwitchRequest> AddShiftSwitchRequestAsync(ShiftSwitchRequest request);
    Task<ShiftSwitchRequest> UpdatShiftSwitchRequesteAsync(ShiftSwitchRequest request);
    Task DeleteShiftSwitchRequestAsync(long id);
    IQueryable<ShiftSwitchRequest> GetManyShiftSwitchRequestAsync();
    Task<ShiftSwitchRequest> GetSingleShiftSwitchRequestAsync(long id);
    Task<List<ShiftSwitchRequest>> GetShiftSwitchRequestByEmployeeAsync(long employeeId);
    Task<List<ShiftSwitchRequest>> GetShiftSwitchRequestByShiftAsync(long shiftId);
    
    Task<ShiftSwitchReply> AddShiftSwitchReplyAsync(ShiftSwitchReply reply, long requestId);
    Task<ShiftSwitchReply> UpdateShiftSwitchReplyAsync(ShiftSwitchReply reply);
    Task DeleteShiftSwitchReplyAsync(long id);
    Task<ShiftSwitchReply> GetSinglShiftSwitchReplyeAsync(long id);
    Task<List<ShiftSwitchReply>> GetShiftSwitchReplyByRequestIdAsync(long requestId);
    Task<List<ShiftSwitchReply>> GetShiftSwitchReplyByTargetEmployeeAsync(long employeeId);
    Task<ShiftSwitchReply> SetShiftSwitchReplyTargetAcceptedAsync(long id, bool accepted);
    Task<ShiftSwitchReply> SetShiftSwitchReplyOriginAcceptedAsync(long id, bool accepted);
    
    Task<ShiftSwitchRequestTimeframe> AddShiftSwitchRequestTimeframeAsync(ShiftSwitchRequestTimeframe timeframe, long requestId);
    Task DeletShiftSwitchRequestTimeframeAsync(long id);
    Task<ShiftSwitchRequestTimeframe> GetShiftSwitchRequestTimeframeSingleAsync(long id);
    Task<List<ShiftSwitchRequestTimeframe>> GetShiftSwitchRequestTimeframeByRequestIdAsync(long requestId);
}