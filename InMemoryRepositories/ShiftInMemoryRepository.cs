using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class ShiftInMemoryRepository : IShiftRepository
{
    private List<Shift> shifts = new List<Shift>();
    
    public async Task<Shift> AddAsync(Shift shift)
    {
      
        try
        {
            shifts.Add(shift);
            return shift;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the shift.", ex);
        }
    }

    
    public async Task<Shift> UpdateAsync(Shift shift)
    {
        try
        {
            Shift? existingShift = shifts.SingleOrDefault(p => p.Id == shift.Id);
            if (existingShift is null) throw new ArgumentException($"Shift with ID {shift.Id} not found!", nameof(shift.Id));

            shifts.Remove(existingShift);
            shifts.Add(shift);
            return shift;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the shift.", ex);
        }
    }
    
    public async Task DeleteAsync(long id)
    {
        try
        {
            Shift? shiftToRemove = shifts.FirstOrDefault(p => p.Id == id);
            if (shiftToRemove is null) throw new ArgumentException($"Shift with ID {id} not found", nameof(id));
            shifts.Remove(shiftToRemove);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the shift.", ex);
        }
    }

    public IQueryable<Shift> GetManyAsync()
    {
        return shifts.AsQueryable();
    }

    public async Task<Shift> GetSingleAsync(long id)
    {
        try
        {
            var shift = shifts.FirstOrDefault(c => c.Id == id);
            if (shift is null) throw new ArgumentException($"Shift with ID '{id}' not found", nameof(id));
            return shift;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the shift.", ex);
        }
    }

    public async Task<bool> IsShiftInRepository(long id)
    {
        try
        {
            var exists = shifts.Any(shift => shift.Id == id);
            return await Task.FromResult(exists);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while checking if the shift is in the repository.", ex);
        }
    }

    public async Task<Shift> AssignEmployeeToShift(long shiftId, long employeeId)
    {
        try
        {
            var shift = shifts.SingleOrDefault(s => s.Id == shiftId);
            if (shift == null)
            {
                throw new ArgumentException("No shift found with ID: " + shiftId, nameof(shiftId));
            }
            else if (shift.AssingnedEmployees.Contains(employeeId))
            {
                throw new InvalidOperationException($"Shift with ID {shiftId} has already assigned employee with ID {employeeId}.");
            }
            else
            {
                shifts.SingleOrDefault(s => s.Id == shiftId).AssingnedEmployees.Add(employeeId);
            }

            return shift;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while assigning the employee to the shift.", ex);
        }
    }
    

    public async Task<Shift> UnassignEmployeeToShift(long shiftId, long employeeId)
    {
        
        try
        {
            var shift = shifts.SingleOrDefault(s => s.Id == shiftId);
            if (shift == null)
            {
                throw new ArgumentException($"Shift with ID {shiftId} not found!");
            }
            if (shift.AssingnedEmployees.Contains(employeeId))
            {
                shift.AssingnedEmployees.Remove(employeeId);
                return shift;
            }
            throw new InvalidOperationException($"Shift with ID {shiftId} has not assigned employee with ID {employeeId}!");
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while unassigning the employee from the shift.", ex);
        }
    }

    public void UpdateChache(List<Shift> newShifts)
    {
        shifts = new List<Shift>(newShifts);
    }
}