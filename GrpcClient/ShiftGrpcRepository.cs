using Grpc.Core;
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
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);

            var client = new Shift.ShiftClient(channel);
            Console.WriteLine("Before adding shift");
            var reply = await client.AddSingleShiftAsync(new NewShiftDTO
            {
                Location = shift.Location,
                ShiftStatus = shift.ShiftStatus,
                Description = shift.Description,
                StartDateTime = new DateTimeOffset(shift.StartDateTime).ToUnixTimeMilliseconds(),
                EndDateTime = new DateTimeOffset(shift.EndDateTime).ToUnixTimeMilliseconds(),
            });
            Console.WriteLine("After adding shift");
            Entities.Shift shiftRecieved = grpcShiftObject(reply);
            return shiftRecieved;
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.AlreadyExists)
            {
                throw new ArgumentException(e.Message);
            }
            throw;
        }
    }

    public async Task<Entities.Shift> UpdateAsync(Entities.Shift shift)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Shift.ShiftClient(channel);
            var updateShiftDto = new ShiftDTO
            {
                Id = shift.Id,
                StartDateTime = new DateTimeOffset(shift.StartDateTime).ToUnixTimeMilliseconds(),
                EndDateTime = new DateTimeOffset(shift.EndDateTime).ToUnixTimeMilliseconds(),
                TypeOfShift = shift.TypeOfShift,
                ShiftStatus = shift.ShiftStatus,
                Description = shift.Description,
                Location = shift.Location
            };
            ShiftDTO shiftDto = await client.UpdateSingleShiftAsync(updateShiftDto);
            return grpcShiftObject(shiftDto);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message);
            } 
            else if(e.StatusCode == StatusCode.Cancelled) // change this to the right one
            {
                throw new ArgumentException(e.Message);
            }
            throw;
        }
    }

    public async Task DeleteAsync(long shift)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Shift.ShiftClient(channel);
            await client.DeleteSingleShiftAsync(new Id());
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message);
            }
            throw;
        }
    }

    public IQueryable<Entities.Shift> GetManyAsync()
    {
            using var channel = GrpcChannel.ForAddress("http://192.168.125.143:50051");
            var client = new Shift.ShiftClient(channel);
            List<ShiftDTO> shiftDtos = client.GetAllShifts(new Empty()).Dtos.ToList();
            List<Entities.Shift> shifts = new List<Entities.Shift>();
            shiftDtos.ForEach(dto => { shifts.Add(grpcShiftObject(dto)); });
            return shifts.AsQueryable();
    }

    public async Task<Entities.Shift> GetSingleAsync(long id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Shift.ShiftClient(channel);
            var request = new Id { Id_ = id };
            var reply = await client.GetSingleShiftByIdAsync(request);
            return grpcShiftObject(reply);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message);
            } 
            throw;
        }
    }
    
    public async Task<bool> IsShiftInRepository(long id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress); 
            var client = new Shift.ShiftClient(channel); 
            var reply = await client.IsShiftInRepositoryAsync(new Id { Id_ = id});
            return reply.Result;
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message);
            }
            throw;
        }
    }

    public async Task<Entities.Shift> AssignEmployeeToShift(long shiftId, long employeeId)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Shift.ShiftClient(channel);
        var assignEmployeeRequest = new ShiftEmployeePair
        {
            ShiftId = shiftId,
            EmployeeId = employeeId
        };
        var reply = await client.AssignEmployeeToShiftAsync(assignEmployeeRequest);
        var updatedShift = await client.GetSingleShiftByIdAsync(new Id { Id_ = shiftId });
        return grpcShiftObject(updatedShift);
    }

    public async  Task<Entities.Shift> UnassignEmployeeToShift(long shiftId, long employeeId)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Shift.ShiftClient(channel);
        var unassignRequest = new Id { Id_ = shiftId };
        var reply = await client.UnAssignEmployeeFromShiftAsync(unassignRequest);
        var updatedShift = await client.GetSingleShiftByIdAsync(new Id { Id_ = shiftId });
        return grpcShiftObject(updatedShift);
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