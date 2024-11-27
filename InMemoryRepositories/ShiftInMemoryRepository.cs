using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class ShiftInMemoryRepository : IShiftRepository
{
    private List<Shift> shifts = new List<Shift>();
    
    public async Task<Shift> AddAsync(Shift shift)
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
        return shift;
    }

    
    public async Task<Shift> UpdateAsync(Shift shift)
    {
        Shift? exisitingShift = shifts.SingleOrDefault(p => p.Id == shift.Id);

        if (exisitingShift is null) throw new InvalidOperationException($"Shift with ID {shift.Id} not found");

        shifts.Remove(exisitingShift);
        shifts.Add(shift);
        return shift;
    }
    
    public async Task DeleteAsync(long id)
    {
        Shift? shiftToRemove = shifts.FirstOrDefault(p => p.Id == id);

        if (shiftToRemove is null) throw new InvalidOperationException($"Shift with ID {id} not found");

        shifts.Remove(shiftToRemove);
    }

    public IQueryable<Shift> GetManyAsync()
    {
        return shifts.AsQueryable();
    }

    public async Task<Shift> GetSingleAsync(long id)
    {
        var shift = shifts.FirstOrDefault(c => c.Id == id);
        if (shift is null) throw new InvalidOperationException($"Shift with ID '{id}' not found");
        return shift;
    }

    public Task<bool> IsShiftInRepository(long id)
    {
        var exists = shifts.Any(shift => shift.Id == id);
        return Task.FromResult(exists);
    }

    public async Task<Shift> AssignEmployeeToShift(long shiftId, long employeeId)
    {
        var shift = shifts.SingleOrDefault(s => s.Id == shiftId);
        if (shift == null)
        {
            throw new InvalidOperationException($"Shift with ID {shiftId} not found!");
        }

        // shift.EmployeeId = employeeId;
        if (shift.EmployeeId == null)
        {
            shift.EmployeeId = employeeId;
        }
        else if (shift.EmployeeId == employeeId)
        {
            throw new InvalidOperationException($"Shift with ID {shiftId} already has this employee with ID {employeeId}!");
        }
        else if (shift.EmployeeId != null)
        {
            throw new InvalidOperationException($"Shift with ID {shiftId} already has assigned employee with ID {shift.EmployeeId}!");
        }

        return shift;
    }

    public async Task<Shift> UnassignEmployeeToShift(long shiftId)
    {
        var shift = shifts.SingleOrDefault(s => s.Id == shiftId);
        if (shift == null)
        {
            throw new InvalidOperationException($"Shift with ID {shiftId} not found!");
        }

        shift.EmployeeId = null;

        return shift;
    }
}