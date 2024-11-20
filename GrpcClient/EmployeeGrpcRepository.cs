using Grpc.Net.Client;
using RepositoryContracts;

namespace GrpcClient;

public class EmployeeGrpcRepository : IEmployeeRepository
{
    private string _grpcAddress { get; set; }

    public EmployeeGrpcRepository()
    {
        _grpcAddress = "http://192.168.125.143:50051";
    }

    public async Task<Entities.Employee> AddAsync(Entities.Employee employee)
    {
        using var channel = GrpcChannel.ForAddress("http://192.168.125.143:50051"); //TODO the port might change
        //create client for employee entity, "EmployeeClient" method is auto-genereted               
        var client = new Employee.EmployeeClient(channel);
        //reply - "AddSingleEmployee(newEmployeeDto)" comes from .proto file + need to convert the og-DTO to .proto-DTO
        var reply = client.AddSingleEmployee(new NewEmployeeDTO
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            WorkingNumber = UInt32.CreateChecked(employee.WorkingNumber),
            Email = employee.Email,
            Password = employee.Password
        });
        
        //converting
        List<ShiftDTO> grpcShiftDtos = reply.AssignedShifts.Dtos.ToList();
        List<Entities.Shift> shifts = GrpcShiftDtosToEntityShiftsList(grpcShiftDtos);
        Entities.Employee employeeRecieved = grpcEmplyeeDtoToEntitiyEmployee(reply, shifts);
        
        return employeeRecieved;
    }

    public Task UpdateAsync(Entities.Employee employee)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int WorkingNumber)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Entities.Employee> GetManyAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Entities.Employee> GetSingleAsync(int WorkingNumber)
    {
        throw new NotImplementedException();
    }

    public static List<Entities.Shift> GrpcShiftDtosToEntityShiftsList(List<ShiftDTO> grpcShiftDtos)
    {
        List<Entities.Shift> shifts = new List<Entities.Shift>();
        foreach (var shiftDTO in grpcShiftDtos)
        {
            var shift = new Entities.Shift
            {
                Id = shiftDTO.Id,
                StartDateTime = new DateTime(shiftDTO.StartDateTime),
                EndDateTime = new DateTime(shiftDTO.EndDateTime),
                TypeOfShift = shiftDTO.TypeOfShift,
                ShiftStatus = shiftDTO.TypeOfShift,
                Description = shiftDTO.Description,
                Location = shiftDTO.Description
            };
            shifts.Add(shift);
        }

        return shifts;
    }

    public static Entities.Employee grpcEmplyeeDtoToEntitiyEmployee(EmployeeDTO employeeDto, List<Entities.Shift> shifts)
    {
        Entities.Employee employee = new Entities.Employee
        {
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            Id = employeeDto.Id,
            WorkingNumber = int.CreateChecked(employeeDto.WorkingNumber),
            Shifts = shifts,
            Email = employeeDto.Email,
            Password = employeeDto.Password
        };
        return employee;
    }
    
    public static Entities.Employee grpcEmplyeeDtoToEntitiyEmployee(DTOs.NewEmployeeDTO newEmployeeDto)
    {
        Entities.Employee employee = new Entities.Employee
        {
            FirstName = newEmployeeDto.FirstName,
            LastName = newEmployeeDto.LastName,
            WorkingNumber = int.CreateChecked(newEmployeeDto.WorkingNumber),
            Email = newEmployeeDto.Email,
            Password = newEmployeeDto.Password
        };
        return employee;
    }


}