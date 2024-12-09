using GrpcClient;
using InMemoryRepositories;
using RepositoryContracts;
using ShiftSwitchReply = Entities.ShiftSwitchReply;

namespace RepositoryProxies;

public class ShiftSwitchReplyProxy : IShiftSwitchReplyRepository
{

    private IShiftSwitchReplyRepository _ShiftSwitchReplyCachingRepository { get; set; }
    private IShiftSwitchReplyRepository _ShiftSwitchReplyShiftStorageRepository { get; set; }
    private DateTime _lastCacheUpdate { get; set; }

    public ShiftSwitchReplyProxy()
    {
        _ShiftSwitchReplyCachingRepository = new ShiftSwitchReplyInMemoryRepository();
        // _ShiftSwitchReplyShiftStorageRepository = new ShiftSwitchReplyGrpcRepository();
        List<ShiftSwitchReply> shiftSwitchReplies = _ShiftSwitchReplyShiftStorageRepository.GetManyAsync().ToList();
        shiftSwitchReplies.ForEach(shiftSwitchReplies => _ShiftSwitchReplyCachingRepository.AddAsync(shiftSwitchReplies));
        _lastCacheUpdate = DateTime.Today;
    }
    
    public async Task<ShiftSwitchReply> AddAsync(ShiftSwitchReply shiftSwitchReply)
    {
        ShiftSwitchReply addedShiftSwitchReply = await _ShiftSwitchReplyShiftStorageRepository.AddAsync(shiftSwitchReply);
        await _ShiftSwitchReplyCachingRepository.AddAsync(shiftSwitchReply);
        return addedShiftSwitchReply;
    }

    public async Task<ShiftSwitchReply> UpdateAsync(ShiftSwitchReply shiftSwitchReply)
    {
        await _ShiftSwitchReplyCachingRepository.UpdateAsync(shiftSwitchReply);
        await _ShiftSwitchReplyShiftStorageRepository.UpdateAsync(shiftSwitchReply);
        return shiftSwitchReply;
    }

    public async Task DeleteAsync(long id)
    {
        await _ShiftSwitchReplyCachingRepository.DeleteAsync(id);
        await _ShiftSwitchReplyShiftStorageRepository.DeleteAsync(id);
    }

    public IQueryable<ShiftSwitchReply> GetManyAsync()
    {
        RefreshCache();
        return _ShiftSwitchReplyCachingRepository.GetManyAsync();
    }

    public async Task<ShiftSwitchReply> GetSingleAsync(long id)
    {
        RefreshCache();
        return await _ShiftSwitchReplyCachingRepository.GetSingleAsync(id);
    }

    public async Task<bool> IsReplyInRepository(long id)
    {
        RefreshCache();
        return await _ShiftSwitchReplyCachingRepository.IsReplyInRepository(id);
    }

    public async Task<ShiftSwitchReply> SetTargetAcceptedAsync(long id, bool accepted)
    {
        await _ShiftSwitchReplyCachingRepository.SetTargetAcceptedAsync(id, accepted);
        await _ShiftSwitchReplyShiftStorageRepository.SetTargetAcceptedAsync(id, accepted);
        return await _ShiftSwitchReplyCachingRepository.GetSingleAsync(id);
    }

    public async Task<ShiftSwitchReply> SetOriginAcceptedAsync(long id, bool accepted)
    {
        
        await _ShiftSwitchReplyCachingRepository.SetOriginAcceptedAsync(id, accepted);
        await _ShiftSwitchReplyShiftStorageRepository.SetOriginAcceptedAsync(id, accepted);
        return await _ShiftSwitchReplyCachingRepository.GetSingleAsync(id);
    }
    
    private async void RefreshCache()
    {
        if (_lastCacheUpdate.AddMinutes(2).CompareTo(DateTime.Now) > 0)
        {
            List<ShiftSwitchReply> shiftSwitchReplies = _ShiftSwitchReplyShiftStorageRepository.GetManyAsync().ToList();
            _ShiftSwitchReplyCachingRepository = new ShiftSwitchReplyInMemoryRepository();
            foreach (var reply in shiftSwitchReplies)
            {
                await _ShiftSwitchReplyCachingRepository.AddAsync(reply);
            }
            _lastCacheUpdate = DateTime.Now;
        }
    }
}