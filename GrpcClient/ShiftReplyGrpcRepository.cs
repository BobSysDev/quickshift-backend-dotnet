

using Grpc.Core;
using Grpc.Net.Client;
using RepositoryContracts;

namespace GrpcClient;

public class ShiftReplyGrpcRepository : IShiftReplyRepository
{
    private string _grpcAddress { get; set; }

    public ShiftReplyGrpcRepository()
    {
        _grpcAddress = "http://192.168.140.143:50051";
    }

    public async Task<Entities.ShiftSwitchReply> AddAsync(Entities.ShiftSwitchReply shiftSwitchReply)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new ShiftSwitchReply.ShiftSwitchReplyClient(channel);

    }

    public Task<ShiftSwitchReply> UpdateAsync(ShiftSwitchReply shiftSwitchReply)
    {

    }

    public Task DeleteAsync(long id)
    {

    }

    public IQueryable<ShiftSwitchReply> GetManyAsync()
    {

    }

    public Task<ShiftSwitchReply> GetSingleAsync(long id)
    {

    }

    public Task<bool> IsReplyInRepository(long id)
    {

    }

    public Task<ShiftSwitchReply> SetTargetAcceptedAsync(long id, bool accepted)
    {

    }

    public Task<ShiftSwitchReply> SetOriginAcceptedAsync(long id, bool accepted)
    {
  
    }
}