using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class ShiftSwitchSwitchRequestInMemoryRepository : IShiftSwitchRequestRepository
{
    private readonly List<ShiftSwitchRequest> requests = new List<ShiftSwitchRequest>();
    
    public async Task<ShiftSwitchRequest> AddAsync(ShiftSwitchRequest request)
    {
        if (request.Id == 0)
        {
            request.Id = requests.Any() ? requests.Max(r => r.Id) + 1 : 1;
        }

        requests.Add(request);
        return request;
    }

    public async Task<ShiftSwitchRequest> UpdateAsync(ShiftSwitchRequest request)
    {
        var existingRequest = requests.SingleOrDefault(r => r.Id == request.Id);
        if (existingRequest is null) throw new InvalidOperationException($"Request with ID {request.Id} not found.");
        requests.Remove(existingRequest);
        requests.Add(request);
        return request;
    }


    public async Task DeleteAsync(long id)
    {
        var requestToRemove = requests.FirstOrDefault(r => r.Id == id);
        if (requestToRemove is null) throw new InvalidOperationException($"Request with ID {id} not found.");
        requests.Remove(requestToRemove);
        await Task.CompletedTask;
    }

    public IQueryable<ShiftSwitchRequest> GetManyAsync()
    {
        return requests.AsQueryable();
    }

    public async Task<ShiftSwitchRequest> GetSingleAsync(long id)
    {
        var request = requests.SingleOrDefault(r => r.Id == id);
        if (request is null) throw new InvalidOperationException($"Request with ID {id} not found.");
        return request;
    }

    public Task<bool> IsRequestInRepository(long id)
    {
        var exists = requests.Any(request => request.Id == id);
        return Task.FromResult(exists);
    }

    public async Task<List<ShiftSwitchRequest>> GetByEmployeeAsync(long employeeId)
    {
        var result = requests.Where(r => r.OriginEmployee != null && r.OriginEmployee.Id == employeeId).ToList();
        return result;
    }

    public async Task<List<ShiftSwitchRequest>> GetByShiftAsync(long shiftId)
    {
        var result = requests.Where(r => r.OriginShift != null && r.OriginShift.Id == shiftId).ToList();
        return result;
    }
}