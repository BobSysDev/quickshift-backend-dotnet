using Grpc.Core;
using Grpc.Net.Client;
using RepositoryContracts;

namespace GrpcClient;

public class EmployeeGrpcRepository : IEmployeeRepository
{
    private string _grpcAddress { get; set; }

    public EmployeeGrpcRepository(string grpcAddress)
    {
        _grpcAddress = grpcAddress;
    }

    public async Task<Entities.Employee> AddAsync(Entities.Employee employee)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Employee.EmployeeClient(channel);
        try
        {
            var reply = await client.AddSingleEmployeeAsync(GrpcDtoConverter.EmployeeToGrpcNewEmployeeDto(employee));
            Entities.Employee employeeReceived = GrpcDtoConverter.GrpcEmployeeDtoToEmployee(reply);
            
            return employeeReceived;
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.AlreadyExists)
            {
                throw new ArgumentException($"Employee already exists: {employee.Id}", nameof(employee.Id));
            }

            throw new Exception("An error occurred while adding the employee.", e);
        }
    }

    public async Task<Entities.Employee> UpdateAsync(Entities.Employee employee)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Employee.EmployeeClient(channel);
            var reply = await client.UpdateSingleEmployeeAsync(GrpcDtoConverter.EmployeeToGrpcUpdateEmployeeDto(employee));

            return GrpcDtoConverter.GrpcEmployeeDtoToEmployee(reply);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException($"Employee not found: {employee.Id}", nameof(employee.Id));
            }
            else if (e.StatusCode == StatusCode.AlreadyExists)
            {
                throw new ArgumentException($"Employee already exists: {employee.Id}", nameof(employee.Id));
            }

            throw new Exception("An error occurred while updating the employee.", e);
        }
    }

    public async Task DeleteAsync(long id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Employee.EmployeeClient(channel);
            await client.DeleteSingleEmployeeAsync(new Id { Id_ = id });
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException($"Employee not found: {id}", nameof(id));
            }

            throw new Exception("An error occurred while deleting the employee.", e);
        }
    }

    public IQueryable<Entities.Employee> GetManyAsync()
    {
        
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Employee.EmployeeClient(channel);
            var reply = client.GetAllEmployees(new Empty());
            var employees = GrpcDtoConverter.GrpcEmployeeDtoListToEntityEmployeeList(reply);

            return employees.AsQueryable();
        }
        catch (RpcException e)
        {
            throw new Exception("An error occurred while retrieving employees.", e);
        }
    }

    public async Task<Entities.Employee> GetSingleAsync(long id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Employee.EmployeeClient(channel);
            var reply = await client.GetSingleEmployeeByIdAsync(new Id { Id_ = id });

            return GrpcEmployeeDtoToEntityEmployee(reply, GrpcShiftDtosToEntityShiftsList(reply.AssignedShifts.Dtos.ToList()));
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException($"Employee not found: {id}", nameof(id));
            }

            throw new Exception("An error occurred while retrieving the employee.", e);
        }
    }

    public async Task<Entities.Employee> GetSingleEmployeeByWorkingNumberAsync(int WorkingNumber)
    {
        Entities.Employee employee = new Entities.Employee();
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress); 
            var client = new Employee.EmployeeClient(channel);
            var reply = await client.GetSingleEmployeeByWorkingNumberAsync(new WorkingNumber { WorkingNumber_ = uint.CreateChecked(WorkingNumber)  }); 
            employee = GrpcEmployeeDtoToEntityEmployee(reply, GrpcShiftDtosToEntityShiftsList(reply.AssignedShifts.Dtos.ToList()));
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
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Employee.EmployeeClient(channel);
            var reply = await client.IsEmployeeInRepositoryAsync(new Id { Id_ = Id });
            b = reply.Boolean_;
            return b;
        }
        catch(RpcException e)
        {
            throw new Exception("An error occurred while checking if the employee is in the repository.", e);
        }
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
                ShiftStatus = shiftDTO.ShiftStatus,
                Description = shiftDTO.Description,
                Location = shiftDTO.Location,
                AssingnedEmployees = shiftDTO.AssignedEmployeeIds.ToList()
            };
            shifts.Add(shift);
        }

        return shifts;
    }
    
    public static List<Entities.Shift> ShiftDtosToEntityShiftsList(List<DTOs.Shift.ShiftDTO> shiftDtos)
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
                ShiftStatus = shiftDto.ShiftStatus,
                Description = shiftDto.Description,
                Location = shiftDto.Location,
                AssingnedEmployees = shiftDto.AssignedEmployees
            };
            shifts.Add(shift);
        }

        return shifts;
    }

    public static Entities.Employee GrpcEmployeeDtoToEntityEmployee(EmployeeDTO employeeDto, List<Entities.Shift> shifts)
    {
        Entities.Employee employee = new Entities.Employee
        {
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            Id = employeeDto.Id,
            WorkingNumber = int.CreateChecked(employeeDto.WorkingNumber),
            Shifts = shifts,
            Email = employeeDto.Email,
            Password = employeeDto.Password,
        };
        return employee;
    }
    
    
    
    public static Entities.Employee NewEmployeeDtoToEntityEmployee(DTOs.NewEmployeeDTO newEmployeeDto)
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

    public static List<DTOs.Shift.ShiftDTO> ShiftsToEntityShiftDTOs(List<Entities.Shift> shifts)
    {
        List<DTOs.Shift.ShiftDTO> shiftsToReturn = new List<DTOs.Shift.ShiftDTO>();
        foreach (var shiftTemp in shifts)
        {
            var shiftDTO = new DTOs.Shift.ShiftDTO
            {
                Id = shiftTemp.Id,
                StartDateTime = shiftTemp.StartDateTime,
                EndDateTime = shiftTemp.EndDateTime,
                TypeOfShift = shiftTemp.TypeOfShift,
                ShiftStatus = shiftTemp.ShiftStatus,
                Description = shiftTemp.Description,
                Location = shiftTemp.Location,
                AssignedEmployees = shiftTemp.AssingnedEmployees
            };
            shiftsToReturn.Add(shiftDTO);
        }

        return shiftsToReturn;
    }

}