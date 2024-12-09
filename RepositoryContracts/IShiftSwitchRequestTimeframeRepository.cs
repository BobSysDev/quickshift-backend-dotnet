using Entities;

namespace RepositoryContracts;

public interface IShiftSwitchRequestTimeframeRepository
{
    Task<ShiftSwitchRequestTimeframe> AddAsync(ShiftSwitchRequestTimeframe timeframe);
    Task DeleteAsync(long id);
    Task<ShiftSwitchRequestTimeframe> GetSingleAsync(long id);
}
