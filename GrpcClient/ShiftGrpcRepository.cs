using DTOs.Shift;
using Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using RepositoryContracts;

namespace GrpcClient;

public class ShiftGrpcRepository : IShiftRepository
{
    private string _grpcAddress { get; set; }

    public ShiftGrpcRepository()
    {
        _grpcAddress = "http://192.168.195.143:50051";
    }

    public async Task<Entities.Shift> AddAsync(Entities.Shift shift)
    {
        
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        
        var client = new Shift.ShiftClient(channel);
        
        var reply = await client.AddSingleShiftAsync(new NewShiftDTO
        {
            TypeOfShift = shift.TypeOfShift,
            Location = shift.Location,
            ShiftStatus = shift.ShiftStatus,
            Description = shift.Description,
            StartDateTime = new DateTimeOffset(shift.StartDateTime).ToUnixTimeMilliseconds(),
            EndDateTime = new DateTimeOffset(shift.EndDateTime).ToUnixTimeMilliseconds(),
            
        });
        
        Entities.Shift shiftReceived = GrpcShiftDtoToEntityShift(reply);
        return shiftReceived;
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
            return GrpcShiftDtoToEntityShift(shiftDto);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message+": "+shift.Id, nameof(shift.Id));
            }

            throw;
        }
    }

    public async Task DeleteAsync(long shiftId)
    {
        
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Shift.ShiftClient(channel);
        var request = new Id{Id_ = shiftId};
        
        try
        {
            await client.DeleteSingleShiftAsync(request);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message+": "+shiftId, nameof(shiftId));
            }

            throw;
        }
    }

    public IQueryable<Entities.Shift> GetManyAsync()
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Shift.ShiftClient(channel);
        List<ShiftDTO> shiftDtos = client.GetAllShifts(new Empty()).Dtos.ToList();
        List<Entities.Shift> shifts = new List<Entities.Shift>();
        shiftDtos.ForEach(dto => { shifts.Add(GrpcShiftDtoToEntityShift(dto)); });
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
            return GrpcShiftDtoToEntityShift(reply);
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

    public async Task<bool> IsShiftInRepository(long id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Shift.ShiftClient(channel);
            var reply = await client.IsShiftInRepositoryAsync(new Id { Id_ = id });
            return reply.Boolean_;
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
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Shift.ShiftClient(channel);
            var assignEmployeeRequest = new ShiftEmployeePair
            {
                ShiftId = shiftId,
                EmployeeId = employeeId
            };

            Console.WriteLine($"Assigning Employee ID: {employeeId} to Shift ID: {shiftId}");
            var reply = await client.AssignEmployeeToShiftAsync(assignEmployeeRequest);
            Console.WriteLine("Assignment successful, fetching updated shift");

            var updatedShift = await client.GetSingleShiftByIdAsync(new Id { Id_ = shiftId });
            Console.WriteLine($"Updated Shift: {updatedShift.Id}, Employee ID: {employeeId}");

            return GrpcShiftDtoToEntityShift(updatedShift);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                if (e.Message.Contains("Shift"))
                {
                    throw new ArgumentException(e.Message + ": " + shiftId, nameof(shiftId));
                }
                else
                {
                    throw new ArgumentException(e.Message + ": " + employeeId, nameof(employeeId));
                }
            }

            throw;
        }
    }

    public async  Task<Entities.Shift> UnassignEmployeeToShift(long shiftId, long employeeId)
    {
        try
        {
            var unassignEmployeeRequest = new ShiftEmployeePair
            {
                ShiftId = shiftId,
                EmployeeId = employeeId
            };
            
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Shift.ShiftClient(channel);
            var unassignRequest = new ShiftEmployeePair { ShiftId = shiftId, EmployeeId = employeeId};
            var reply = await client.UnAssignEmployeeFromShiftAsync(unassignEmployeeRequest);
            var updatedShift = await client.GetSingleShiftByIdAsync(new Id { Id_ = shiftId });
            return GrpcShiftDtoToEntityShift(updatedShift);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                if (e.Message.Contains("Shift"))
                {
                    throw new ArgumentException(e.Message + ": " + shiftId, nameof(shiftId));
                }
                else
                {
                    throw new ArgumentException(e.Message + ": " + employeeId, nameof(employeeId));
                }
            }

            throw;
        }
        
    }
    

    public static Entities.Shift GrpcShiftDtoToEntityShift(ShiftDTO shiftDto)
    {

        Entities.Shift shift = new Entities.Shift()
        {
            Description = shiftDto.Description,
            TypeOfShift = shiftDto.TypeOfShift,
            ShiftStatus = shiftDto.ShiftStatus,
            Id = shiftDto.Id,
            StartDateTime = DateTimeOffset.FromUnixTimeMilliseconds(shiftDto.StartDateTime).DateTime,
            EndDateTime = DateTimeOffset.FromUnixTimeMilliseconds(shiftDto.EndDateTime).Date,
            Location = shiftDto.Location,
            AssingnedEmployees = shiftDto.AssignedEmployeeIds.ToList()
        };
        return shift;
    }

    public static Entities.Shift NewShiftDtoToEntityShift(DTOs.Shift.NewShiftDTO newShiftDto)
    {
        Entities.Shift shift = new Entities.Shift
        {
            Description = newShiftDto.Description,
            Location = newShiftDto.Location,
            ShiftStatus = newShiftDto.ShiftStatus,
            StartDateTime = newShiftDto.StartDateTime,
            EndDateTime = newShiftDto.EndDateTime,
            TypeOfShift = newShiftDto.TypeOfShift
        };
        return shift;
    }
}



















