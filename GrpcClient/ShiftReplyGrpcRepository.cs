using DTOs;
using Entities;
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

    public async Task<ShiftSwitchReply> AddAsync(ShiftSwitchReply shiftSwitchReply)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Shift.ShiftClient(channel);
        var reply = await client.AddShiftSwitchReplyAsync(new ShiftSwitchReplyDTO
        {
           
        });
        return grpcShiftSwitchReplyObject(reply);
    }

    public Task<ShiftSwitchReply> UpdateAsync(ShiftSwitchReply shiftSwitchReply)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Shift.ShiftClient(channel);
    }

    public Task DeleteAsync(long id)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Shift.ShiftClient(channel);
    }

    public IQueryable<ShiftSwitchReply> GetManyAsync()
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Shift.ShiftClient(channel);
    }

    public Task<ShiftSwitchReply> GetSingleAsync(long id)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Shift.ShiftClient(channel);
    }

    public Task<bool> IsReplyInRepository(long id)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Shift.ShiftClient(channel);
    }

    public Task<ShiftSwitchReply> SetTargetAcceptedAsync(long id, bool accepted)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Shift.ShiftClient(channel);
    }

    public Task<ShiftSwitchReply> SetOriginAcceptedAsync(long id, bool accepted)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Shift.ShiftClient(channel);
    }
}