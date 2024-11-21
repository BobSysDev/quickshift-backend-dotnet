using Grpc.Core;
using Grpc.Net.Client;
using RepositoryContracts;

namespace GrpcClient;

public class EmployeeGrpcRepository : IEmployeeRepository
{
    private string _grpcAddress { get; set; }

    public EmployeeGrpcRepository()
    {
        _grpcAddress = "http://192.168.140.143:50051";
    }

    public async Task<Entities.Employee> AddAsync(Entities.Employee employee)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress); 
        //create client for employee entity, "EmployeeClient" method is auto-genereted               
        var client = new Employee.EmployeeClient(channel);
        //reply - "AddSingleEmployee(newEmployeeDto)" comes from .proto file + need to convert the og-DTO to .proto-DTO
        try
        {
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
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.AlreadyExists)
            {
                throw new ArgumentException(e.Message);
            }
        }




        return null;
    }

    public async Task<Entities.Employee> UpdateAsync(Entities.Employee employee)
    {
        Entities.Employee employee2 = new Entities.Employee();
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress); 
            var client = new Employee.EmployeeClient(channel);
            var reply = client.UpdateSingleEmployee(new UpdateEmployeeDTO
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                WorkingNumber = UInt32.CreateChecked(employee.WorkingNumber),
                Email = employee.Email,
                Password = employee.Password
            });
            employee2 = grpcEmplyeeDtoToEntitiyEmployee(reply,
                GrpcShiftDtosToEntityShiftsList(reply.AssignedShifts.Dtos.ToList()));
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message);
            }
            else if (e.StatusCode == StatusCode.AlreadyExists)
            {
                throw new ArgumentException(e.Message);
            }
        }

        return employee2;
    }

    public async Task DeleteAsync(long id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress); 
            var client = new Employee.EmployeeClient(channel);
            var reply = client.DeleteSingleEmployee(new Id { Id_ = id });
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }

    public IQueryable<Entities.Employee> GetManyAsync()
    {
        List<Entities.Employee> employees = new List<Entities.Employee>();
        
        using var channel = GrpcChannel.ForAddress(_grpcAddress); 
        var client = new Employee.EmployeeClient(channel);
        var reply = client.GetAllEmployees(new Empty());
        employees = grpcEmplyeeDtoListToEntitiyEmployeeList(reply);
       
        return employees.AsQueryable();
    }

    public async Task<Entities.Employee> GetSingleAsync(long id)
    {
        Entities.Employee employee = new Entities.Employee();
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress); 
            var client = new Employee.EmployeeClient(channel);
            var reply = client.GetSingleEmployeeById(new Id { Id_ = id }); 
            employee = grpcEmplyeeDtoToEntitiyEmployee(reply,
                GrpcShiftDtosToEntityShiftsList(reply.AssignedShifts.Dtos.ToList()));
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message);
            }
        }

        return employee;
    }

    public async Task<bool> IsEmployeeInRepository(long Id)
    {
        System.Boolean b = new System.Boolean();
        
        using var channel = GrpcChannel.ForAddress(_grpcAddress); 
        var client = new Employee.EmployeeClient(channel);
        var reply = client.IsEmployeeInRepository(new Id { Id_ = Id });
        b = reply.Result;
        
        return b;
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
    
    public static List<Entities.Shift> EntityShiftDtosToEntityShiftsList(List<DTOs.Shift.ShiftDTO> shiftDtos)
    {
        List<Entities.Shift> shifts = new List<Entities.Shift>();
        foreach (var shiftDto in shiftDtos)
        {
            var shift = new Entities.Shift
            {
                Id = shiftDto.Id,
                StartDateTime = shiftDto.StartDateTime,
                EndDateTime = shiftDto.EndDateTime,
                TypeOfShift = shiftDto.TypeOfShift,
                ShiftStatus = shiftDto.TypeOfShift,
                Description = shiftDto.Description,
                Location = shiftDto.Description
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
    
    public static List<Entities.Employee>grpcEmplyeeDtoListToEntitiyEmployeeList(EmployeeDTOList list)
    {
        List<Entities.Employee> employees = new List<Entities.Employee>();
        foreach (var dto in list.Dtos)
        {
            Entities.Employee employee = new Entities.Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Id = dto.Id,
                WorkingNumber = int.CreateChecked(dto.WorkingNumber),
                Shifts = GrpcShiftDtosToEntityShiftsList(dto.AssignedShifts.Dtos.ToList()),
                Email = dto.Email,
                Password = dto.Password
            };
            employees.Add(employee);
        }
        
        return employees;
    }
    
    public static Entities.Employee EntityNewEmployeeDtoToEntityEmployee(DTOs.NewEmployeeDTO newEmployeeDto)
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