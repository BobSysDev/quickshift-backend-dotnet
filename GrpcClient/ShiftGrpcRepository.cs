using Grpc.Net.Client;
using RepositoryContracts;

namespace GrpcClient;

public class ShiftGrpcRepository : IShiftRepository
{
    private string _grpcAddress { get; set; }

    public ShiftGrpcRepository()
    {
        _grpcAddress = "http://192.168.125.143:50051";
    }
    
    public async Task<Entities.Shift> AddAsync(Entities.Shift shift)
    {
        using var channel = GrpcChannel.ForAddress("http://192.168.125.143:50051");

        var client = new Shift.ShiftClient(channel);

        var reply = client.AddSingleShift(new NewShiftDTO
        { 
            Location = shift.Location,
            ShiftStatus = shift.ShiftStatus,
            Description = shift.Description,
            StartDateTime = new DateTimeOffset(shift.StartDateTime).ToUnixTimeMilliseconds(),
            EndDateTime = new DateTimeOffset(shift.EndDateTime).ToUnixTimeMilliseconds(),
        });
        
        Entities.Shift shiftRecieved = grpcShiftObject(reply);
        return shiftRecieved;
    }

    public Task UpdateAsync(Entities.Shift shift)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long shift)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Entities.Shift> GetManyAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Entities.Shift> GetSingleAsync(long id)
    {
        throw new NotImplementedException();
    }

    public static Entities.Shift grpcShiftObject(ShiftDTO shiftDto)
    {
        Entities.Shift shift = new Entities.Shift()
        {
            Description = shiftDto.Description,
            TypeOfShift = shiftDto.TypeOfShift,
            ShiftStatus = shiftDto.ShiftStatus,
            Id = shiftDto.Id,
            StartDateTime = DateTimeOffset.FromUnixTimeMilliseconds(shiftDto.StartDateTime).DateTime,
            EndDateTime = DateTimeOffset.FromUnixTimeMilliseconds(shiftDto.EndDateTime).Date,
            Location = shiftDto.Location
        };
        return shift;
    }

    public static Entities.Shift EntityShiftWithoutIdToEntityShift(DTOs.Shift.ShiftDTOWithoutId shiftDtoWithoutId)
    {
        Entities.Shift shift = new Entities.Shift
        {
            Description = shiftDtoWithoutId.Description,
            Location = shiftDtoWithoutId.Location,
            ShiftStatus = shiftDtoWithoutId.ShiftStatus,
            StartDateTime = shiftDtoWithoutId.StartDateTime,
            EndDateTime = shiftDtoWithoutId.EndDateTime,
            TypeOfShift = shiftDtoWithoutId.TypeOfShift
        };
        return shift;
    }
}