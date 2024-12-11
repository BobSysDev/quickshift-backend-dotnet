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

    public ShiftSwitchReplyGrpcRepository(IShiftRepository shiftRepository, IEmployeeRepository employeeRepository, string grpcAddress)
    {
        _grpcAddress = grpcAddress;
        _shiftRepository = shiftRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<Entities.ShiftSwitchReply> AddAsync(Entities.ShiftSwitchReply reply, long requestId)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchReply.ShiftSwitchReplyClient(channel);

            var response = await client.AddReplyAsync(GrpcDtoConverter.ShiftSwitchReplyToGrpcNewReplyDto(reply,requestId));

            Entities.ShiftSwitchReply shiftSwitchReplyReceived =
                await GrpcDtoConverter.GrpcReplyDtoToShiftSwitchReply(response, _shiftRepository, _employeeRepository);
            return shiftSwitchReplyReceived;
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.AlreadyExists)
            {
                throw new ArgumentException($"Shift switch reply already exists: {reply.Id}", nameof(reply.Id));
            }

            throw new Exception("An error occurred while adding the shift switch reply.", e);
        }
    }

    public async Task<Entities.ShiftSwitchReply> UpdateAsync(Entities.ShiftSwitchReply reply)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchReply.ShiftSwitchReplyClient(channel);

            var response = await client.UpdateReplyAsync(GrpcDtoConverter.ShiftSwitchReplyToGrpcUpdateReplyDto(reply));

            return await GrpcDtoConverter.GrpcReplyDtoToShiftSwitchReply(response, _shiftRepository, _employeeRepository);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message + ": " + reply.Id, nameof(reply.Id));
            }

            throw new Exception("An error occurred while updating the shift switch reply.", e);
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

            throw new Exception("An error occurred while deleting the shift switch reply.", e);
        }
    }

    public  IQueryable<Entities.ShiftSwitchReply> GetManyAsync()
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchReply.ShiftSwitchReplyClient(channel);
            var response = client.GetAll(new Empty());
            var shiftSwitchReplies = response.Dtos.Select(dto => GrpcDtoConverter.GrpcReplyDtoToShiftSwitchReply(dto, _shiftRepository, _employeeRepository).Result).ToList();
            return shiftSwitchReplies.AsQueryable();
        }
        catch (RpcException e)
        {
            throw new Exception("An error occurred while retrieving shift switch replies.", e);
        }
    }

    public async Task<Entities.ShiftSwitchReply> GetSingleAsync(long id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchReply.ShiftSwitchReplyClient(channel);
            var request = new Id { Id_ = id };
            var response = await client.GetSingleByIdAsync(request);
            return await GrpcDtoConverter.GrpcReplyDtoToShiftSwitchReply(response, _shiftRepository, _employeeRepository);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message + ": " + id, nameof(id));
            }

            throw new Exception("An error occurred while retrieving the shift switch reply.", e);
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

            throw new Exception("An error occurred while setting target accepted for the shift switch reply.", e);
        }
    }

    public async Task<bool> SetOriginAcceptedAsync(long id, bool accepted)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchReply.ShiftSwitchReplyClient(channel);
            var response = await client.SetAcceptReplyOriginAsync(new IdBooleanPair
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

            throw new Exception("An error occurred while setting origin accepted for the shift switch reply.", e);
        }
    }
}