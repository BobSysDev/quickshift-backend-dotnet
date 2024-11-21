using System.Runtime.InteropServices.JavaScript;
using DTOs.Shift;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;


namespace GrpcClient;

public class GrpcRepo
{
    public async Task<DTOs.EmployeeDTO> CreateEmployee(DTOs.NewEmployeeDTO newEmployeeDto)
    {
        //create channel for connection to JAVA
        using var channel = GrpcChannel.ForAddress("http://192.168.125.143:50051"); //TODO the port might change
        //create client for employee entity, "EmployeeClient" method is auto-genereted               
        var client = new Employee.EmployeeClient(channel);
        //reply - "AddSingleEmployee(newEmployeeDto)" comes from .proto file + need to convert the og-DTO to .proto-DTO
        var reply =  client.AddSingleEmployee(new NewEmployeeDTO
        {
            FirstName = newEmployeeDto.FirstName,
            LastName = newEmployeeDto.LastName,
            WorkingNumber = UInt32.CreateChecked(newEmployeeDto.WorkingNumber),
            Email = newEmployeeDto.Email,
            Password = newEmployeeDto.Password
        }); //TODO await maybe
        //create new employee now with id to be returned to web app basically, source it from reply
        EmployeeDTO employeeDto = new EmployeeDTO //maybe unnecessary
        {
            FirstName = reply.FirstName,
            LastName = reply.LastName,
            Id = reply.Id,
            WorkingNumber = reply.WorkingNumber,
            AssignedShifts = reply.AssignedShifts,
            Email = reply.Email,
            Password = reply.Password
        };

        List<ShiftDTO> grpcShiftDtos = employeeDto.AssignedShifts.Dtos.ToList();
        List<DTOs.Shift.ShiftDTO> shiftDtos = new List<DTOs.Shift.ShiftDTO>();
        grpcShiftDtos.ForEach((grpcShiftDto) =>
        {
            shiftDtos.Add(new DTOs.Shift.ShiftDTO
            {
                StartDateTime = new DateTime(grpcShiftDto.StartDateTime),
                EndDateTime = new DateTime(grpcShiftDto.EndDateTime),
                Description = grpcShiftDto.Description,
                Id = grpcShiftDto.Id,
                Location = grpcShiftDto.Location,
                ShiftStatus = grpcShiftDto.ShiftStatus,
                TypeOfShift = grpcShiftDto.ShiftStatus
                
            });
        });
        
        //convert to og DTO
        DTOs.EmployeeDTO ogEmployeeDto = new DTOs.EmployeeDTO
        {
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            Id = employeeDto.Id,
            WorkingNumber = int.CreateChecked(employeeDto.WorkingNumber),
            Shifts = shiftDtos,
            Email = employeeDto.Email,
            Password = employeeDto.Password
        };
        return ogEmployeeDto;
    }

    // public async Task<JSType.Boolean> DeleteEmployee(DTOs.DeleteEmployeeDTO deleteEmployeeDto)
    // {
    //     using var channel = GrpcChannel.ForAddress("http://192.168.125.143:50051"); //TODO the port might change
    //     var client = new Employee.EmployeeClient(channel);
    //     string reply = client.DeleteSingleEmployee(new Id{Id_ = deleteEmployeeDto.id}).Text;
    //     if (reply.Equals("Employee deleted successfully."))
    //     {
    //         return true;
    //     }
    //
    //     return false;
    // }

    public async Task<DTOs.Shift.ShiftDTO> CreateShift(ShiftDTOWithoutId shiftDtoWithoutId)
    {
        using var channel = GrpcChannel.ForAddress("http://192.168.125.143:50051");
        var client = new Shift.ShiftClient(channel);
        var newShiftDto = new NewShiftDTO
        {
            Description = shiftDtoWithoutId.Description,
            TypeOfShift = shiftDtoWithoutId.TypeOfShift,
            ShiftStatus = shiftDtoWithoutId.ShiftStatus,
            StartDateTime = new DateTimeOffset(shiftDtoWithoutId.StartDateTime).ToUnixTimeMilliseconds(),
            EndDateTime = new DateTimeOffset(shiftDtoWithoutId.EndDateTime).ToUnixTimeMilliseconds(),
            Location = shiftDtoWithoutId.Location
        };
        var reply = client.AddSingleShift(newShiftDto);

        ShiftDTO shiftDto = new ShiftDTO
        {
            Description = reply.Description,
            TypeOfShift = reply.TypeOfShift,
            Id = reply.Id,
            ShiftStatus = reply.ShiftStatus,
            StartDateTime = reply.StartDateTime,
            EndDateTime = reply.EndDateTime,
            Location = reply.Location
        };

        DTOs.Shift.ShiftDTO ogShiftDto = new DTOs.Shift.ShiftDTO
        {
            Description = shiftDto.Description,
            TypeOfShift = shiftDto.TypeOfShift,
            Id = shiftDto.Id,
            ShiftStatus = shiftDto.ShiftStatus,
            StartDateTime = DateTimeOffset.FromUnixTimeMilliseconds(shiftDto.StartDateTime).DateTime,
            EndDateTime = DateTimeOffset.FromUnixTimeMilliseconds(shiftDto.EndDateTime).Date,
            Location = shiftDto.Location
        };
        return ogShiftDto;
    }
}


