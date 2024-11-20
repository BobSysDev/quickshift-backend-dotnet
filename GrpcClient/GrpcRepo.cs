using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;


namespace GrpcClient;

public class GrpcRepo
{
    //TODO sebo - prototype for your fixing session
    public async Task<DTOs.EmployeeDTO> CreateEmployee(DTOs.NewEmployeeDTO newEmployeeDto)
    {
        //create channel for connection to JAVA
        using var channel = GrpcChannel.ForAddress("http://localhost:50051"); //TODO the port might change
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

    public async Task<DTOs.Shift.ShiftDTO> CreateShift(DTOs.Shift.ShiftDTOWithoutId shiftDtoWithoutId)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:50051");
        var client = new 
    }
    // public async Task<string> SendHello(string name)
    // {
    //     using var channel = GrpcChannel.ForAddress("http://localhost:50051");
    //     var client = new Greeter.GreeterClient(channel);
    //     var reply = await client.SayHelloAsync(new HelloRequest { Name = name});
    //     Console.WriteLine(reply.Message);
    //     return reply.Message;
    // }
    //
    // public async Task<List<String>> GetAllHellos()
    // {
    //     using var channel = GrpcChannel.ForAddress("http://localhost:50051");
    //     var client = new Greeter.GreeterClient(channel);
    //     var reply = await client.GetAllHellosAsync(new Empty()); 
    //     return reply.Replies.ToList();
    // }
}