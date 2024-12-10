using Grpc.Core;
using Grpc.Net.Client;
using RepositoryContracts;

namespace GrpcClient;

public class ShiftSwitchRequestTimeframeGrpcRepository : IShiftSwitchRequestTimeframeRepository
{
    private string _grpcAddress { get; set; }
    private readonly IShiftRepository _shiftRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public ShiftSwitchRequestTimeframeGrpcRepository(IShiftRepository shiftRepository, IEmployeeRepository employeeRepository, string grpcAddress)
    {
        _grpcAddress = grpcAddress;
        _shiftRepository = shiftRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<Entities.ShiftSwitchRequestTimeframe> AddAsync(Entities.ShiftSwitchRequestTimeframe timeframe, long requestId)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchRequestTimeframe.ShiftSwitchRequestTimeframeClient(channel);
            var request = await client.AddTimeframeAsync(new NewTimeframeDTO()
            {
                ShiftSwitchRequestId = requestId,
                TimeFrameStart = new DateTimeOffset(timeframe.TimeFrameStart).ToUnixTimeMilliseconds(),
                TimeFrameEnd = new DateTimeOffset(timeframe.TimeFrameEnd).ToUnixTimeMilliseconds()
            });
            Entities.ShiftSwitchRequestTimeframe shiftSwitchRequestTimeframeRecieved =
                GrpcDtoConverter.GrpcTimeframeDtoToShiftSwitchRequestTimeframe(request);
            return shiftSwitchRequestTimeframeRecieved;
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.AlreadyExists)
            {
                throw new ArgumentException($"Shift Switch Request Timeframe already exists: {timeframe.Id}", nameof(timeframe.Id));
            }

            throw new Exception("An error occurred while adding the shift switch request timeframe.", e);
        }
    }

    public async Task DeleteAsync(long id)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new ShiftSwitchRequestTimeframe.ShiftSwitchRequestTimeframeClient(channel);
        var request = new Id { Id_ = id };

        try
        {
            await client.DeleteTimeframeAsync(request);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message + ": " + id, nameof(id));
            }

            throw new Exception("An error occurred while deleting the shift switch request timeframe.", e);
        }
    }

    public async Task<Entities.ShiftSwitchRequestTimeframe> GetSingleAsync(long id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchRequestTimeframe.ShiftSwitchRequestTimeframeClient(channel);
            var request = new Id { Id_ = id };
            var response = await client.GetSingleByIdAsync(request);
            return GrpcDtoConverter.GrpcTimeframeDtoToShiftSwitchRequestTimeframe(response);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message + ": " + id, nameof(id));
            }

            throw new Exception("An error occurred while retrieving the shift switch request timeframe.", e);
        }
    }
}