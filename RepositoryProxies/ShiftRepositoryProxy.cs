﻿using GrpcClient;
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
        List<Shift> shifts = _shiftStorageRepository.GetManyAsync().ToList();
        shifts.ForEach(shift => _shiftCachingRepository.AddAsync(shift));
        _lastCacheUpdate = DateTime.Now;
    }

    public async Task<Shift> AddAsync(Shift shift)
    {
        Shift addedShift = await _shiftStorageRepository.AddAsync(shift);
        _shiftCachingRepository.AddAsync(shift);
        return addedShift;
    }

    public async Task UpdateAsync(Shift shift)
    {
        await _shiftCachingRepository.UpdateAsync(shift);
        await _shiftStorageRepository.UpdateAsync(shift);
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

    public Task<Shift> AssignEmployeeToShift(long shiftId, long employeeId)
    {
        throw new NotImplementedException();
    }

    public Task<Shift> UnassignEmployeeToShift(long shiftId, long employeeId)
    {
        throw new NotImplementedException();
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