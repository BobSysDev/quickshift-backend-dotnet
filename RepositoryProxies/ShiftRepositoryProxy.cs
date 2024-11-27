using GrpcClient;
using InMemoryRepositories;
using RepositoryContracts;
using Shift = Entities.Shift;

namespace RepositoryProxies;

public class ShiftRepositoryProxy : IShiftRepository
{
    private IShiftRepository _shiftCachingRepository { get; set; }
    private IShiftRepository _shiftStorageRepository { get; set; }
    private DateTime _lastCacheUpdate { get; set; }

    public ShiftRepositoryProxy()
    {
        _shiftCachingRepository = new ShiftInMemoryRepository();
        _shiftStorageRepository = new ShiftGrpcRepository();
        Console.Write("Trying to get shifts from repo");
        List<Shift> shifts = _shiftStorageRepository.GetManyAsync().ToList();
        Console.Write("Got them shifts");
        shifts.ForEach(shift => _shiftCachingRepository.AddAsync(shift));
        _lastCacheUpdate = DateTime.Now;
    }

    public async Task<Shift> AddAsync(Shift shift)
    {
        Shift addedShift = await _shiftStorageRepository.AddAsync(shift);
        await _shiftCachingRepository.AddAsync(shift);
        return addedShift;
    }

    public async Task<Shift> UpdateAsync(Shift shift)
    {
        await _shiftCachingRepository.UpdateAsync(shift);
        await _shiftStorageRepository.UpdateAsync(shift);
        return shift;
    }

    public async Task DeleteAsync(long shift)
    {
        await _shiftCachingRepository.DeleteAsync(shift);
        await _shiftStorageRepository.DeleteAsync(shift);
    }

    public IQueryable<Shift> GetManyAsync()
    {
        RefreshCache();
        return _shiftCachingRepository.GetManyAsync();
    }

    public async Task<Shift> GetSingleAsync(long id)
    {
        RefreshCache();
        return await _shiftCachingRepository.GetSingleAsync(id);
    }

    public async Task<bool> IsShiftInRepository(long id)
    {
        RefreshCache();
        return await _shiftCachingRepository.IsShiftInRepository(id);
    }

    public async Task<Shift> AssignEmployeeToShift(long shiftId, long employeeId)
    {
        await _shiftCachingRepository.AssignEmployeeToShift(shiftId, employeeId);
        await _shiftStorageRepository.AssignEmployeeToShift(shiftId, employeeId);
        return await _shiftCachingRepository.GetSingleAsync(shiftId);
    }

    public async Task<Shift> UnassignEmployeeToShift(long shiftId, long employeeId)
    {
        await _shiftCachingRepository.UnassignEmployeeToShift(shiftId, employeeId);
        await _shiftStorageRepository.UnassignEmployeeToShift(shiftId, employeeId);
        return await _shiftCachingRepository.GetSingleAsync(shiftId);
    }

    private async void RefreshCache()
    {
        if (_lastCacheUpdate.AddMinutes(2).CompareTo(DateTime.Now) > 0)
        {
            List<Shift> shifts =_shiftStorageRepository.GetManyAsync().ToList();
            shifts.ForEach(async shift =>
            {
                if (await _shiftCachingRepository.IsShiftInRepository(shift.Id))
                {
                    await _shiftCachingRepository.UpdateAsync(shift);
                }
                else
                {
                    await _shiftCachingRepository.AddAsync(shift);
                }
            });
            _lastCacheUpdate = DateTime.Now;
        }
    }
}