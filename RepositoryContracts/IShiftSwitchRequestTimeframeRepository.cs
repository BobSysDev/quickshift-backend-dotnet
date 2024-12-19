using Entities;

namespace RepositoryContracts;

public interface IShiftSwitchRequestTimeframeRepository
{
    Task<ShiftSwitchRequestTimeframe> AddAsync(ShiftSwitchRequestTimeframe timeframe, long requestId);
    Task DeleteAsync(long id);
    Task<ShiftSwitchRequestTimeframe> GetSingleAsync(long id);
}
