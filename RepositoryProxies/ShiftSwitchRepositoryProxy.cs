using GrpcClient;
using InMemoryRepositories;
using RepositoryContracts;
using Boolean = System.Boolean;
using ShiftSwitchReply = Entities.ShiftSwitchReply;
using ShiftSwitchRequest = Entities.ShiftSwitchRequest;
using ShiftSwitchRequestTimeframe = Entities.ShiftSwitchRequestTimeframe;

namespace RepositoryProxies;

public class ShiftSwitchRepositoryProxy : IShiftSwitchRepository
{
    private IShiftSwitchRepository _shiftSwitchCachingRepository{ get; set; }
    private IShiftSwitchRequestRepository _requestStorageRepository { get; set; }
    private IShiftSwitchReplyRepository _replyStorageRepository { get; set; }
    private IShiftSwitchRequestTimeframeRepository _timeframeStorageRepository { get; set; }
    private DateTime _lastChacheUpdate{ get; set; }

    public ShiftSwitchRepositoryProxy(IShiftRepository shiftRepository, IEmployeeRepository employeeRepository)
    {
        _replyStorageRepository = new ShiftSwitchReplyGrpcRepository(shiftRepository, employeeRepository);
        _requestStorageRepository = new ShiftSwitchSwitchRequestGrpcRepository(shiftRepository, employeeRepository);
        _timeframeStorageRepository = new ShiftSwitchRequestTimeframeInMemoryRepository();
        List<ShiftSwitchRequest> shifts = _shiftSwitchCachingRepository.GetManyShiftSwitchRequestAsync().ToList();
        _shiftSwitchCachingRepository= new ShiftSwitchInMemoryRepository();
        shifts.ForEach(request => _shiftSwitchCachingRepository.AddShiftSwitchRequestAsync(request));
        _lastChacheUpdate = DateTime.Now;
    }
    
    
    
    public async Task<ShiftSwitchRequest> AddShiftSwitchRequestAsync(ShiftSwitchRequest request)
    {
        ShiftSwitchRequest addedRequest = await _requestStorageRepository.AddAsync(request);
        return addedRequest;
    }

    public async Task<ShiftSwitchRequest> UpdateShiftSwitchRequestAsync(ShiftSwitchRequest request)
    {
        await _shiftSwitchCachingRepository.UpdateShiftSwitchRequestAsync(request);
        await _requestStorageRepository.UpdateAsync(request);
        return request;
    }

    public async Task DeleteShiftSwitchRequestAsync(long id)
    {
        await _shiftSwitchCachingRepository.DeleteShiftSwitchRequestAsync(id);
        await _requestStorageRepository.DeleteAsync(id);
    }

    public IQueryable<ShiftSwitchRequest> GetManyShiftSwitchRequestAsync()
    {
        RefreshCache();
        return _shiftSwitchCachingRepository.GetManyShiftSwitchRequestAsync();
    }

    public Task<ShiftSwitchRequest> GetSingleShiftSwitchRequestAsync(long id)
    {
        RefreshCache();
        return _shiftSwitchCachingRepository.GetSingleShiftSwitchRequestAsync(id);
    }

    public Task<List<ShiftSwitchRequest>> GetManyShiftSwitchRequestsByEmployeeIdAsync(long employeeId)
    {
        RefreshCache();
        return _shiftSwitchCachingRepository.GetManyShiftSwitchRequestsByEmployeeIdAsync(employeeId);
    }

    public Task<List<ShiftSwitchRequest>> GetManyShiftSwitchRequestsByShiftIdAsync(long shiftId)
    {
        RefreshCache();
        return _shiftSwitchCachingRepository.GetManyShiftSwitchRequestsByShiftIdAsync(shiftId);
    }

    public async Task<ShiftSwitchReply> AddShiftSwitchReplyAsync(ShiftSwitchReply reply, long requestId)
    {
        ShiftSwitchReply addedReply = await _replyStorageRepository.AddAsync(reply);
        await _shiftSwitchCachingRepository.AddShiftSwitchReplyAsync(reply, requestId);
        return addedReply;
    }

    public async Task<ShiftSwitchReply> UpdateShiftSwitchReplyAsync(ShiftSwitchReply reply)
    {
        await _shiftSwitchCachingRepository.UpdateShiftSwitchReplyAsync(reply);
        await _replyStorageRepository.UpdateAsync(reply);
        return reply;
    }

    public async Task DeleteShiftSwitchReplyAsync(long id)
    {
        await _shiftSwitchCachingRepository.DeleteShiftSwitchReplyAsync(id);
        await _replyStorageRepository.DeleteAsync(id);
    }

    public async Task<ShiftSwitchReply> GetSingleShiftSwitchReplyAsync(long id)
    {
        RefreshCache();
        return await _shiftSwitchCachingRepository.GetSingleShiftSwitchReplyAsync(id);
    }

    public Task<List<ShiftSwitchReply>> GetManyShiftSwitchRepliesByRequestIdAsync(long requestId)
    {
        RefreshCache();
        return _shiftSwitchCachingRepository.GetManyShiftSwitchRepliesByRequestIdAsync(requestId);
    }

    public Task<List<ShiftSwitchReply>> GetManyShiftSwitchRepliesByTargetEmployeeAsync(long employeeId)
    {
        RefreshCache();
        return _shiftSwitchCachingRepository.GetManyShiftSwitchRepliesByTargetEmployeeAsync(employeeId);
    }

    public async Task<ShiftSwitchReply> SetShiftSwitchReplyTargetAcceptedAsync(long id, bool accepted)
    {
        await _shiftSwitchCachingRepository.SetShiftSwitchReplyTargetAcceptedAsync(id, accepted);
        await _replyStorageRepository.SetTargetAcceptedAsync(id, accepted);
        return await GetSingleShiftSwitchReplyAsync(id);
    }

    public async Task<ShiftSwitchReply> SetShiftSwitchReplyOriginAcceptedAsync(long id, bool accepted)
    {
        await _shiftSwitchCachingRepository.SetShiftSwitchReplyOriginAcceptedAsync(id, accepted);
        await _replyStorageRepository.SetTargetAcceptedAsync(id, accepted);
        return await GetSingleShiftSwitchReplyAsync(id);
    }

    public Task<long> GetShiftSwitchRequestIdByShiftSwitchReplyId(long id)
    {
        RefreshCache();
        return _shiftSwitchCachingRepository.GetShiftSwitchRequestIdByShiftSwitchReplyId(id);
    }

    public async Task<ShiftSwitchRequestTimeframe> AddShiftSwitchRequestTimeframeAsync(ShiftSwitchRequestTimeframe timeframe, long requestId)
    {
        ShiftSwitchRequestTimeframe addedRequestTimeframe =
            await _timeframeStorageRepository.AddAsync(timeframe);
        return addedRequestTimeframe;
    }
    

    public async Task DeleteShiftSwitchRequestTimeframeAsync(long id)
    {
        await _shiftSwitchCachingRepository.DeleteShiftSwitchRequestTimeframeAsync(id);
        await _requestStorageRepository.DeleteAsync(id);
    }

    public async Task<ShiftSwitchRequestTimeframe> GetShiftSwitchRequestTimeframeSingleAsync(long id)
    {
        RefreshCache();
        return await _shiftSwitchCachingRepository.GetShiftSwitchRequestTimeframeSingleAsync(id);
    }

    public async Task<List<ShiftSwitchRequestTimeframe>> GetManyShiftSwitchRequestTimeframesByRequestIdAsync(long requestId)
    {
        RefreshCache();
        return await _shiftSwitchCachingRepository.GetManyShiftSwitchRequestTimeframesByRequestIdAsync(requestId);
    }

    public async Task<long> GetShiftSwitchRequestIdByShiftSwitchRequestTimeframeId(long id)
    {
        RefreshCache();
        return await _shiftSwitchCachingRepository.GetShiftSwitchRequestIdByShiftSwitchRequestTimeframeId(id);
    }
    
    private void RefreshCache()
    {
        if (_lastChacheUpdate.AddMinutes(2).CompareTo(DateTime.Now) > 0)
        {
            List<ShiftSwitchRequest> shifts = _shiftSwitchCachingRepository.GetManyShiftSwitchRequestAsync().ToList();
            _shiftSwitchCachingRepository= new ShiftSwitchInMemoryRepository();
            shifts.ForEach(request => _shiftSwitchCachingRepository.AddShiftSwitchRequestAsync(request));
            _lastChacheUpdate = DateTime.Now;
        }
    }
}