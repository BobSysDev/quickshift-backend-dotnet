using System.Runtime.InteropServices.JavaScript;
using Entities;

namespace RepositoryContracts;

public interface IShiftRepository
{
    Task<Shift> AddAsync(Shift shift);
    Task UpdateAsync(Shift shift);
    Task DeleteAsync(long shift);
    IQueryable<Shift> GetManyAsync();
    Task<Shift> GetSingleAsync(long id);
    Task<Boolean> IsShiftInRepository(long id);
    Task<Shift> AssignEmployeeToShift(long shiftId, long employeeId);
    Task<Shift> UnassignEmployeeToShift(long shiftId, long employeeId);
}