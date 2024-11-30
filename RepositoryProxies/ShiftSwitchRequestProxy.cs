using Entities;
using RepositoryContracts;

namespace RepositoryProxies;

public class ShiftSwitchRequestProxy : IShiftRequestRepository
{
    public Task<ShiftSwitchRequest> AddAsync(ShiftSwitchRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ShiftSwitchRequest> UpdateAsync(ShiftSwitchRequest request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<ShiftSwitchRequest> GetManyAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ShiftSwitchRequest> GetSingleAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsRequestInRepository(long id)
    {
        throw new NotImplementedException();
    }

    public Task<List<ShiftSwitchRequest>> GetByEmployeeAsync(long employeeId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ShiftSwitchRequest>> GetByShiftAsync(long shiftId)
    {
        throw new NotImplementedException();
    }
}