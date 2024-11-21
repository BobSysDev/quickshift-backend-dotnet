using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class ShiftInMemoryRepository : IShiftRepository
{
    private List<Shift> shifts = new List<Shift>();
    
    public Task<Shift> AddAsync(Shift shift)
    {
        if (shift.Id == 0)
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
        }
        shifts.Add(shift);
        return Task.FromResult(shift);
    }

    
    public Task<Shift> UpdateAsync(Shift shift)
    {
        Shift? exisitingShift = shifts.SingleOrDefault(p => p.Id == shift.Id);

        if (exisitingShift is null) throw new InvalidOperationException($"Shift with ID {shift.Id} not found");

        shifts.Remove(exisitingShift);
        shifts.Add(shift);
        return Task.FromResult(shift);
    }
    
    public Task DeleteAsync(long id)
    {
        Shift? shiftToRemove = shifts.FirstOrDefault(p => p.Id == id);

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
        if (shift is null) throw new InvalidOperationException($"Shift with ID '{id}' not found");
        return Task.FromResult(shift);
    }

    public Task<bool> IsShiftInRepository(long id)
    {
        var exists = shifts.Any(shift => shift.Id == id);
        return Task.FromResult(exists);
    }

    public Task<Shift> AssignEmployeeToShift(long shiftId, long employeeId)
    {
        var shift = shifts.SingleOrDefault(s => s.Id == shiftId);
        if (shift == null)
        {
            throw new InvalidOperationException($"Shift with ID {shiftId} not found");
        }

        if (shift.AssignedEmployees == null)
        {
            shift.AssignedEmployees = new List<long>();
        }

        if (!shift.AssignedEmployees.Contains(employeeId))
        {
            shift.AssignedEmployees.Add(employeeId);
        }

        return Task.FromResult(shift);
    }

    public Task<Shift> UnassignEmployeeToShift(long shiftId, long employeeId)
    {
        var shift = shifts.SingleOrDefault(s => s.Id == shiftId);
        if (shift == null)
        {
            throw new InvalidOperationException($"Shift with ID {shiftId} not found");
        }

        if (shift.AssignedEmployees != null && shift.AssignedEmployees.Contains(employeeId))
        {
            shift.AssignedEmployees.Remove(employeeId);
        }

        return Task.FromResult(shift);
    }
}