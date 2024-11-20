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
    }
    
    public Task<Shift> AddAsync(Shift shift)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Shift shift)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long shift)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Shift> GetManyAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Shift> GetSingleAsync(long id)
    {
        throw new NotImplementedException();
    }
}