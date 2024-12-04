

using DTOs;
using Grpc.Core;
using Grpc.Net.Client;
using RepositoryContracts;

namespace GrpcClient;

public class ShiftSwitchReplyGrpcRepository : IShiftSwitchReplyRepository
{
    private string _grpcAddress { get; set; }

    public ShiftSwitchReplyGrpcRepository()
    {
        _grpcAddress = "http://192.168.195.143:50051";
    }

    public async Task<Entities.ShiftSwitchReply> AddAsync(Entities.ShiftSwitchReply shiftSwitchReply)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new ShiftSwitchReply.ShiftSwitchReplyClient(channel);
        var reply = new ShiftSwitchReplyDTO
        {
           Id = shiftSwitchReply.Id,
           OriginShiftId = shiftSwitchReply.
        };
        
        
    }

    public Task<Entities.ShiftSwitchReply> UpdateAsync(Entities.ShiftSwitchReply shiftSwitchReply)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Entities.ShiftSwitchReply> GetManyAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Entities.ShiftSwitchReply> GetSingleAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsReplyInRepository(long id)
    {
        throw new NotImplementedException();
    }

    public Task<Entities.ShiftSwitchReply> SetTargetAcceptedAsync(long id, bool accepted)
    {
        throw new NotImplementedException();
    }

    public Task<Entities.ShiftSwitchReply> SetOriginAcceptedAsync(long id, bool accepted)
    {
        throw new NotImplementedException();
    }
}