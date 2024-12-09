using DTOs;
using DTOs.Shift;
using Grpc.Core;
using Grpc.Net.Client;
using RepositoryContracts;


namespace GrpcClient;

public class ShiftSwitchSwitchRequestGrpcRepository : IShiftSwitchRequestRepository
{
    private string _grpcAddress { get; set; }

    public ShiftSwitchSwitchRequestGrpcRepository()
    {
        _grpcAddress = "http://192.168.195.143:50051";
    }


    public async Task<Entities.ShiftSwitchRequest> AddAsync(Entities.ShiftSwitchRequest request)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
        var reply = await client.AddRequestAsync(new NewRequestDTO
        {
            OriginEmployeeId = request.OriginEmployee.Id,
            OriginShiftId = request.OriginShift.Id,
            Details = request.Details
        });

        Entities.ShiftSwitchRequest shiftSwitchRequestReceived = EntityDTOConverter.ShiftSwitchRequestDtoToShift(reply);
        return shiftSwitchRequestReceived;
    }

    public async Task<Entities.ShiftSwitchRequest> UpdateAsync(Entities.ShiftSwitchRequest request)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
            var updateShiftSwitchRequestDto = new ShiftSwitchRequestDTO
            {
                Id = request.Id,
                OriginShiftId = request.OriginShift.Id
            };
            ShiftSwitchRequestDTO shiftSwitchRequestDto = await client.UpdateRequestAsync(updateShiftSwitchRequestDto);
            return EntityDTOConverter.ShiftSwitchRequestDtoToShift(shiftSwitchRequestDto);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message + ": " + request.Id, nameof(request.Id));
            }

            throw;
        }
    }

    public async Task DeleteAsync(long id)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
        var request = new Id { Id_ = id };

        try
        {
            await client.DeleteRequestAsync(request);
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

    public IQueryable<Entities.ShiftSwitchRequest> GetManyAsync()
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
        var response = client.GetAll(new Empty());
        var shiftSwitchRequests = response.Dtos.Select(EntityDTOConverter.ShiftSwitchRequestDtoToShift).ToList();
        return shiftSwitchRequests.AsQueryable();
    }

    public Task<Entities.ShiftSwitchRequest> GetSingleAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsRequestInRepository(long id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Shift.ShiftClient(channel);
            var request = new Id { Id_ = id };
            var reply = await client.GetSingleShiftByIdAsync(request);
            return EntityDTOConverter.ShiftSwitchRequestDtoToShift(reply);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message+": "+id, nameof(id));
            }

            throw;
        }
    }
    

    public async Task<List<Entities.ShiftSwitchRequest>> GetByEmployeeAsync(long employeeId)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
            var response = await client.GetRequestsByOriginEmployeeIdAsync(new Id { Id_ = employeeId });
            return response.Dtos.Select(EntityDTOConverter.ShiftSwitchRequestDtoToShift).ToList();
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message + ": " + employeeId, nameof(employeeId));
            }

            throw;
        }
    }

    public async Task<List<Entities.ShiftSwitchRequest>> GetByShiftAsync(long shiftId)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
        var response = await client.GetRequestsByOriginShiftIdAsync(new Id { Id_ = shiftId });
        return response.Dtos.Select(EntityDTOConverter.ShiftSwitchRequestDtoToShift).ToList();
    }
}