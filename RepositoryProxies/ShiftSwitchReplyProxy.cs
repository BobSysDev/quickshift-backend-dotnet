using GrpcClient;
using InMemoryRepositories;
using RepositoryContracts;
using ShiftSwitchReply = Entities.ShiftSwitchReply;

namespace RepositoryProxies;

public class ShiftSwitchReplyProxy : IShiftSwitchReplyRepository
{

    private IShiftSwitchReplyRepository _shiftCachingRepository { get; set; }
    private IShiftSwitchReplyRepository _shiftStorageRepository { get; set; }
    private DateTime _lastCacheUpdate { get; set; }

    public ShiftSwitchReplyProxy()
    {
        _shiftCachingRepository = new ShiftSwitchReplyInMemoryRepository();
        _shiftStorageRepository = new ShiftSwitchReplyGrpcRepository();
        List<ShiftSwitchReply> shiftSwitchReplies = _shiftStorageRepository.GetManyAsync().ToList();
        shiftSwitchReplies.ForEach(shiftSwitchReplies => _shiftCachingRepository.AddAsync(shiftSwitchReplies));
        _lastCacheUpdate = DateTime.Today;
    }
    
    public async Task<ShiftSwitchReply> AddAsync(ShiftSwitchReply shiftSwitchReply)
    {
        ShiftSwitchReply addedShiftSwitchReply = await _shiftStorageRepository.AddAsync(shiftSwitchReply);
        await _shiftCachingRepository.AddAsync(shiftSwitchReply);
        return addedShiftSwitchReply;
    }

    public async Task<ShiftSwitchReply> UpdateAsync(ShiftSwitchReply shiftSwitchReply)
    {
        await _shiftCachingRepository.UpdateAsync(shiftSwitchReply);
        await _shiftStorageRepository.UpdateAsync(shiftSwitchReply);
        return shiftSwitchReply;
    }

    public async Task DeleteAsync(long id)
    {
        await _shiftCachingRepository.DeleteAsync(id);
        await _shiftStorageRepository.DeleteAsync(id);
    }

    public IQueryable<ShiftSwitchReply> GetManyAsync()
    {
        RefreshCache();
        return _shiftCachingRepository.GetManyAsync();
    }

    public async Task<ShiftSwitchReply> GetSingleAsync(long id)
    {
        RefreshCache();
        return await _shiftCachingRepository.GetSingleAsync(id);
    }

    public async Task<bool> IsReplyInRepository(long id)
    {
        RefreshCache();
        return await _shiftCachingRepository.IsReplyInRepository(id);
    }

    public async Task<ShiftSwitchReply> SetTargetAcceptedAsync(long id, bool accepted)
    {
        await _shiftCachingRepository.SetTargetAcceptedAsync(id, accepted);
        await _shiftStorageRepository.SetTargetAcceptedAsync(id, accepted);
        return await _shiftCachingRepository.GetSingleAsync(id);
    }

    public async Task<ShiftSwitchReply> SetOriginAcceptedAsync(long id, bool accepted)
    {
        
        await _shiftCachingRepository.SetOriginAcceptedAsync(id, accepted);
        await _shiftStorageRepository.SetOriginAcceptedAsync(id, accepted);
        return await _shiftCachingRepository.GetSingleAsync(id);
    }
    
    private async void RefreshCache()
    {
        if (_lastCacheUpdate.AddMinutes(2).CompareTo(DateTime.Now) > 0)
        {
            List<ShiftSwitchReply> shiftSwitchReplies = _shiftStorageRepository.GetManyAsync().ToList();
            _shiftCachingRepository = new ShiftSwitchReplyInMemoryRepository();
            foreach (var reply in shiftSwitchReplies)
            {
                await _shiftCachingRepository.AddAsync(reply);
            }
            _lastCacheUpdate = DateTime.Now;
        }
    }
}