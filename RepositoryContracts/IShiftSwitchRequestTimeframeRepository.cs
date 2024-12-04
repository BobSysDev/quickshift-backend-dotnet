using Entities;

namespace RepositoryContracts;

public interface IShiftSwitchRequestTimeframeRepository
{
    Task<ShiftSwitchRequestTimeframe> AddAsync(ShiftSwitchRequestTimeframe timeframe);
    Task<ShiftSwitchRequestTimeframe> UpdateAsync(ShiftSwitchRequestTimeframe timeframe);
    Task DeleteAsync(long id);
    IQueryable<ShiftSwitchRequestTimeframe> GetManyAsync();
    Task<ShiftSwitchRequestTimeframe> GetSingleAsync(long id);
    Task<bool> IsTimeframeInRepository(long id);
    Task<List<ShiftSwitchRequestTimeframe>> GetByTimeRangeAsync(DateTime start, DateTime end);
}
