using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public static class ShiftUtilityMethods
{
    public static async Task UpdateEmployeeAfterChangingShifts(long employeeId, IShiftRepository shiftRepository, IEmployeeRepository employeeRepository)
    {
        List<Shift> allShifts = shiftRepository.GetManyAsync().ToList();
        List<Shift> empShifts = new List<Shift>();
        foreach (var shift in allShifts)
        {
            if (shift.AssingnedEmployees.Contains(employeeId))
            {
                empShifts.Add(shift);
            }
        }

        (await employeeRepository.GetSingleAsync(employeeId)).Shifts = empShifts;
    }
}