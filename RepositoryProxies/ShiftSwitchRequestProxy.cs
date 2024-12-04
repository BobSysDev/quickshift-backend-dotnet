using GrpcClient;
using InMemoryRepositories;
using RepositoryContracts;
using ShiftSwitchRequest = Entities.ShiftSwitchRequest;

namespace RepositoryProxies;

public class ShiftSwitchRequestProxy : IShiftSwitchRequestRepository
{

    private IShiftSwitchRequestRepository _shiftSwitchSwitchRequestCachingRepository { get; set; }
    private IShiftSwitchRequestRepository _shiftSwitchSwitchRequestStorageRepository { get; set; }
    private DateTime _lastCacheUpdate { get; set; }

    public ShiftSwitchRequestProxy()
    {
        _shiftSwitchSwitchRequestCachingRepository = new ShiftSwitchSwitchRequestInMemoryRepository();
        _shiftSwitchSwitchRequestStorageRepository = new ShiftSwitchSwitchRequestGrpcRepository();

        List<ShiftSwitchRequest> shiftSwitchRequests =
            _shiftSwitchSwitchRequestStorageRepository.GetManyAsync().ToList();
        shiftSwitchRequests.ForEach(shiftSwitchRequests => _shiftSwitchSwitchRequestCachingRepository.AddAsync(shiftSwitchRequests));

        _lastCacheUpdate = DateTime.Now;
    }
    
    public async Task<ShiftSwitchRequest> AddAsync(ShiftSwitchRequest shiftSwitchRequest)
    {
        ShiftSwitchRequest addedShiftSwitchRequest =
            await _shiftSwitchSwitchRequestStorageRepository.AddAsync(shiftSwitchRequest);
        await _shiftSwitchSwitchRequestCachingRepository.AddAsync(shiftSwitchRequest);
        return addedShiftSwitchRequest;
    }

    public async Task<ShiftSwitchRequest> UpdateAsync(ShiftSwitchRequest shiftSwitchRequest)
    {
        await _shiftSwitchSwitchRequestCachingRepository.UpdateAsync(shiftSwitchRequest);
        await _shiftSwitchSwitchRequestStorageRepository.UpdateAsync(shiftSwitchRequest);
        return shiftSwitchRequest;
    }

    public async Task DeleteAsync(long id)
    {
        await _shiftSwitchSwitchRequestCachingRepository.DeleteAsync(id);
        await _shiftSwitchSwitchRequestStorageRepository.DeleteAsync(id);
    }

    public IQueryable<ShiftSwitchRequest> GetManyAsync()
    {
        RefreshCache();
        return _shiftSwitchSwitchRequestCachingRepository.GetManyAsync();
    }

    public async Task<ShiftSwitchRequest> GetSingleAsync(long id)
    { 
        RefreshCache();
        return await _shiftSwitchSwitchRequestCachingRepository.GetSingleAsync(id);
    }

    public async Task<bool> IsRequestInRepository(long id)
    {
        RefreshCache();
        return await _shiftSwitchSwitchRequestCachingRepository.IsRequestInRepository(id);
    }

    public async Task<List<ShiftSwitchRequest>> GetByEmployeeAsync(long employeeId)
    {
        RefreshCache();
        return await _shiftSwitchSwitchRequestCachingRepository.GetByEmployeeAsync(employeeId);
    }

    public async Task<List<ShiftSwitchRequest>> GetByShiftAsync(long shiftId)
    {
        RefreshCache();
        return await _shiftSwitchSwitchRequestCachingRepository.GetByShiftAsync(shiftId);
    }
    
    private void RefreshCache()
    {
        List<ShiftSwitchRequest> shiftSwitchRequests = _shiftSwitchSwitchRequestStorageRepository.GetManyAsync().ToList();
        _shiftSwitchSwitchRequestCachingRepository = new ShiftSwitchSwitchRequestInMemoryRepository();
        foreach (var shiftSwitchRequest in shiftSwitchRequests)
        { 
            _shiftSwitchSwitchRequestCachingRepository.AddAsync(shiftSwitchRequest);
        }
        _lastCacheUpdate = DateTime.Now;
    }
}