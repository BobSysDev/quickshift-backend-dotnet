using Entities;
using RepositoryContracts;

namespace RepositoryProxies;

public class ShiftSwitchTimeframeProxy : IShiftSwitchRequestTimeframeRepository
{
    public Task<ShiftSwitchRequestTimeframe> AddAsync(ShiftSwitchRequestTimeframe timeframe)
    {
        throw new NotImplementedException();
    }

    public Task<ShiftSwitchRequestTimeframe> UpdateAsync(ShiftSwitchRequestTimeframe timeframe)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<ShiftSwitchRequestTimeframe> GetManyAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ShiftSwitchRequestTimeframe> GetSingleAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsTimeframeInRepository(long id)
    {
        throw new NotImplementedException();
    }

    public Task<List<ShiftSwitchRequestTimeframe>> GetByTimeRangeAsync(DateTime start, DateTime end)
    {
        throw new NotImplementedException();
    }
}