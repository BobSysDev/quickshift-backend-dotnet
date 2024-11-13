using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class ShiftInMemoryRepository : IShiftRepository
{
    private List<Shift> shifts = new List<Shift>();
    
    public Task<Shift> AddAsync(Shift shift)
    {
        if (shifts.Any())
        {
            var maxId = shifts.Max(s => s.Id);
            shift.Id = maxId + 1;
        }
        else
        {
            shift.Id = 1;
        }
        shifts.Add(shift);
        return Task.FromResult(shift);
    }

    public Task UpdateAsync(Shift shift)
    {
        Shift? exisitingShift = shifts.SingleOrDefault(p => p.Id == shift.Id);

        if (exisitingShift is null) throw new InvalidOperationException($"Shift with ID {shift.Id} not found");

        shifts.Remove(exisitingShift);
        shifts.Add(shift);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(long id)
    {
        Shift? shiftToRemove = shifts.SingleOrDefault(p => p.Id == id);

        if (shiftToRemove is null) throw new InvalidOperationException($"Shift with ID {id} not found");

        shifts.Remove(shiftToRemove);
        return Task.CompletedTask;
    }

    public IQueryable<Shift> GetManyAsync()
    {
        return shifts.AsQueryable();
    }

    public Task<Shift> GetSingleAsync(long id)
    {
        var shift = shifts.FirstOrDefault(c => c.Id == id);
        if (shift is null) throw new InvalidOperationException($"Shift with ID {id} not found");
        return Task.FromResult(shift);
    }
}