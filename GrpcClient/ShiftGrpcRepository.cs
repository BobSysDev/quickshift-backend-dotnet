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

    public ShiftGrpcRepository(string grpcAddress)
    {
        _grpcAddress = grpcAddress;
    }

    public async Task<Entities.Shift> AddAsync(Entities.Shift shift)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Shift.ShiftClient(channel);

            var newShiftDto = GrpcDtoConverter.ShiftToGrpcNewShiftDto(shift);
            var reply = await client.AddSingleShiftAsync(newShiftDto);

            Entities.Shift shiftReceived = GrpcDtoConverter.GrpcShiftDtoToShift(reply);
            return shiftReceived;
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.AlreadyExists)
            {
                throw new ArgumentException($"Shift already exists: {shift.Id}", nameof(shift.Id));
            }

            throw new Exception("An error occurred while adding the shift.", e);
        }
    }

    public async Task<Entities.Shift> UpdateAsync(Entities.Shift shift)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Shift.ShiftClient(channel);

            var updateShiftDto = GrpcDtoConverter.ShiftToGrpcShiftDto(shift);
            var reply = await client.UpdateSingleShiftAsync(updateShiftDto);

            return GrpcDtoConverter.GrpcShiftDtoToShift(reply);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message + ": " + shift.Id, nameof(shift.Id));
            }

            throw new Exception("An error occurred while updating the shift.", e);
        }
    }

    public async Task DeleteAsync(long shiftId)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Shift.ShiftClient(channel);
        var request = new Id { Id_ = shiftId };

        try
        {
            await client.DeleteSingleShiftAsync(request);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message + ": " + shiftId, nameof(shiftId));
            }

            throw new Exception("An error occurred while deleting the shift.", e);
        }
    }

    public IQueryable<Entities.Shift> GetManyAsync()
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Shift.ShiftClient(channel);
            List<ShiftDTO> shiftDtos = client.GetAllShifts(new Empty()).Dtos.ToList();
            List<Entities.Shift> shifts = new List<Entities.Shift>();
            shiftDtos.ForEach(dto => { shifts.Add(GrpcDtoConverter.GrpcShiftDtoToShift(dto)); });
            return shifts.AsQueryable();
        }
        catch (RpcException e)
        {
            throw new Exception("An error occurred while retrieving shifts.", e);
        }
    }

    public async Task<Entities.Shift> GetSingleAsync(long id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Shift.ShiftClient(channel);
            var request = new Id { Id_ = id };
            var reply = await client.GetSingleShiftByIdAsync(request);
            return GrpcDtoConverter.GrpcShiftDtoToShift(reply);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message + ": " + id, nameof(id));
            }

            throw new Exception("An error occurred while retrieving the shift.", e);
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

            throw new Exception("An error occurred while checking if the shift is in the repository.", e);
        }
    }

    public async Task<Entities.Shift> AssignEmployeeToShift(long shiftId, long employeeId)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Shift.ShiftClient(channel);

            var assignEmployeeRequest = GrpcDtoConverter.ShiftIdAndEmployeeIdToShiftEmployeePair(shiftId, employeeId);
            var reply = await client.AssignEmployeeToShiftAsync(assignEmployeeRequest);
            var updatedShift = await client.GetSingleShiftByIdAsync(new Id { Id_ = shiftId });

            return GrpcDtoConverter.GrpcShiftDtoToShift(updatedShift);
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

            throw new Exception("An error occurred while assigning the employee to the shift.", e);
        }
    }

    public async Task<Entities.Shift> UnassignEmployeeToShift(long shiftId, long employeeId)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Shift.ShiftClient(channel);

            var unassignEmployeeRequest = GrpcDtoConverter.ShiftIdAndEmployeeIdToShiftEmployeePair(shiftId, employeeId);
            var reply = await client.UnAssignEmployeeFromShiftAsync(unassignEmployeeRequest);
            var updatedShift = await client.GetSingleShiftByIdAsync(new Id { Id_ = shiftId });

            return GrpcDtoConverter.GrpcShiftDtoToShift(updatedShift);
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

            throw new Exception("An error occurred while unassigning the employee from the shift.", e);
        }
    }
    
}