using Entities;
using RepositoryContracts;
using System.Linq;

namespace InMemoryRepositories;

public class ShiftSwitchRequestTimeframeInMemoryRepository : IShiftSwitchRequestTimeframeRepository
{

    private readonly List<ShiftSwitchRequestTimeframe> timeframes = new List<ShiftSwitchRequestTimeframe>();

    public async Task<ShiftSwitchRequestTimeframe> AddAsync(ShiftSwitchRequestTimeframe timeframe)
    {
        if (timeframe.Id == 0)
        {
            timeframe.Id = timeframes.Any() ? timeframes.Max(t => t.Id) + 1 : 1;
        }

        timeframes.Add(timeframe);
        return timeframe;
    }

    public async Task<ShiftSwitchRequestTimeframe> UpdateAsync(ShiftSwitchRequestTimeframe timeframe)
    {
        var existingTimeframe = timeframes.SingleOrDefault(t => t.Id == timeframe.Id);
        if (existingTimeframe == null) throw new InvalidOperationException($"Timeframe with ID {timeframe.Id} not found.");
        timeframes.Remove(existingTimeframe);
        timeframes.Add(timeframe);
        return timeframe;
    }

    public async Task DeleteAsync(long id)
    {
        var timeframeToRemove = timeframes.SingleOrDefault(t => t.Id == id);
        if (timeframeToRemove == null) throw new InvalidOperationException($"Timeframe with ID {id} not found.");
        timeframes.Remove(timeframeToRemove);
    }

    public IQueryable<ShiftSwitchRequestTimeframe> GetManyAsync()
    {
        return timeframes.AsQueryable();
    }

    public async Task<ShiftSwitchRequestTimeframe> GetSingleAsync(long id)
    {
        var timeframe = timeframes.FirstOrDefault(t => t.Id == id);
        if (timeframe == null) throw new InvalidOperationException($"Timeframe with ID {id} not found.");
        return timeframe;
    }

    public async Task<bool> IsTimeframeInRepository(long id)
    {
        var exists = timeframes.Any(t => t.Id == id);
        return exists;
    }

    public async Task<List<ShiftSwitchRequestTimeframe>> GetByTimeRangeAsync(DateTime start, DateTime end)
    {
        var result = timeframes.Where(t => t.TimeFrameStart >= start && t.TimeFrameEnd <= end).ToList();
        return result;
    }
}
