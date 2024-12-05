using Entities;

namespace RepositoryContracts;

public interface IShiftSwitchRepository
{
    Task<ShiftSwitchRequest> AddShiftSwitchRequestAsync(ShiftSwitchRequest request);
    Task<ShiftSwitchRequest> UpdateShiftSwitchRequestAsync(ShiftSwitchRequest request);
    Task DeleteShiftSwitchRequestAsync(long id);
    IQueryable<ShiftSwitchRequest> GetManyShiftSwitchRequestAsync();
    Task<ShiftSwitchRequest> GetSingleShiftSwitchRequestAsync(long id);
    Task<List<ShiftSwitchRequest>> GetManyShiftSwitchRequestsByEmployeeIdAsync(long employeeId);
    Task<List<ShiftSwitchRequest>> GetManyShiftSwitchRequestsByShiftIdAsync(long shiftId);
    
    Task<ShiftSwitchReply> AddShiftSwitchReplyAsync(ShiftSwitchReply reply, long requestId);
    Task<ShiftSwitchReply> UpdateShiftSwitchReplyAsync(ShiftSwitchReply reply);
    Task DeleteShiftSwitchReplyAsync(long id);
    Task<ShiftSwitchReply> GetSingleShiftSwitchReplyAsync(long id);
    Task<List<ShiftSwitchReply>> GetManyShiftSwitchRepliesByRequestIdAsync(long requestId);
    Task<List<ShiftSwitchReply>> GetManyShiftSwitchRepliesByTargetEmployeeAsync(long employeeId);
    Task<ShiftSwitchReply> SetShiftSwitchReplyTargetAcceptedAsync(long id, bool accepted);
    Task<ShiftSwitchReply> SetShiftSwitchReplyOriginAcceptedAsync(long id, bool accepted);
    Task<long> GetShiftSwitchRequestIdByShiftSwitchReplyId(long id);
    
    Task<ShiftSwitchRequestTimeframe> AddShiftSwitchRequestTimeframeAsync(ShiftSwitchRequestTimeframe timeframe, long requestId);
    Task DeleteShiftSwitchRequestTimeframeAsync(long id);
    Task<ShiftSwitchRequestTimeframe> GetShiftSwitchRequestTimeframeSingleAsync(long id);
    Task<List<ShiftSwitchRequestTimeframe>> GetManyShiftSwitchRequestTimeframesByRequestIdAsync(long requestId);
    Task<long> GetShiftSwitchRequestIdByShiftSwitchRequestTimeframeId(long id);
}