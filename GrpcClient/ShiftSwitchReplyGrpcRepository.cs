

using DTOs;
using Grpc.Core;
using Grpc.Net.Client;
using RepositoryContracts;

namespace GrpcClient;

public class ShiftSwitchReplyGrpcRepository : IShiftSwitchReplyRepository
{
    private string _grpcAddress { get; set; }
    private readonly IShiftRepository _shiftRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public ShiftSwitchReplyGrpcRepository(IShiftRepository shiftRepository, IEmployeeRepository employeeRepository)
    {
        _grpcAddress = "http://192.168.195.143:50051";
        _shiftRepository = shiftRepository;
        _employeeRepository = employeeRepository;
    }

    
    public async Task<Entities.ShiftSwitchReply> AddAsync(Entities.ShiftSwitchReply reply)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchReply.ShiftSwitchReplyClient(channel);

            var response = await client.AddReplyAsync(new NewReplyDTO
            {
                TargetEmployeeId = reply.TargetEmployee.Id,
                TargetShiftId = reply.TargetShift.Id,
                Details = reply.Details
            });

           

            Entities.ShiftSwitchReply shiftSwitchReplyReceived =
                GrpcDtoConverter.GrpcReplyDtoToShiftSwitchReply(response, _shiftRepository, _employeeRepository);
            return shiftSwitchReplyReceived;
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.AlreadyExists)
            {
                throw new ArgumentException($"Shift switch reply already exists: {reply.Id}", nameof(reply.Id));
            }

            throw;
        }
    }

    public async Task<Entities.ShiftSwitchReply> UpdateAsync(Entities.ShiftSwitchReply reply)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchReply.ShiftSwitchReplyClient(channel);

            var response = await client.UpdateReplyAsync(new UpdateReplyDTO
            {
                Details = reply.Details,
                Id = reply.Id
            });

            

            return GrpcDtoConverter.GrpcReplyDtoToShiftSwitchReply(response, _shiftRepository, _employeeRepository);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message + ": " + reply.Id, nameof(reply.Id));
            }

            throw;
        }
    }

    public async Task DeleteAsync(long id)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new ShiftSwitchReply.ShiftSwitchReplyClient(channel);
        var request = new Id { Id_ = id };

        try
        {
            await client.DeleteReplyAsync(request);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message + ": " + id, nameof(id));
            }

            throw;
        }
    }

    public IQueryable<Entities.ShiftSwitchReply> GetManyAsync()
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new ShiftSwitchReply.ShiftSwitchReplyClient(channel);
        var response = client.GetAll(new Empty());
        var shiftSwitchReplies = response.Dtos.Select(dto => GrpcDtoConverter.GrpcReplyDtoToShiftSwitchReply(dto, _shiftRepository, _employeeRepository)).ToList();
        return shiftSwitchReplies.AsQueryable();
    }
    public async Task<Entities.ShiftSwitchReply> GetSingleAsync(long id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchReply.ShiftSwitchReplyClient(channel);
            var request = new Id { Id_ = id };
            var response = await client.GetSingleByIdAsync(request);
            return GrpcDtoConverter.GrpcReplyDtoToShiftSwitchReply(response, _shiftRepository, _employeeRepository);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message + ": " + id, nameof(id));
            }

            throw;
        }
    }
    
    public async Task<bool> SetTargetAcceptedAsync(long id, bool accepted)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchReply.ShiftSwitchReplyClient(channel);
            var response = await client.SetAcceptReplyTargetAsync(new IdBooleanPair
                {
                   Id = id,
                   Boolean = accepted
                });

            return response.Boolean_;
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message + ": " + id, nameof(id));
            }

            throw;
        }
    }

    public async Task<bool> SetOriginAcceptedAsync(long id, bool accepted)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchReply.ShiftSwitchReplyClient(channel);
            var response = client.SetAcceptReplyOrigin(new IdBooleanPair
            {
                Id = id,
                Boolean = accepted
            });

            return response.Boolean_;
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message + ": " + id, nameof(id));
            }

            throw;
        }
    }
}