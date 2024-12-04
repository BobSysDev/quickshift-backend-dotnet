using System.Runtime.InteropServices;
using DTOs.Shift;
using Entities;
using RepositoryContracts;


namespace DTOs;

public class EntityDTOConverter
{
    
    //all emp. dtos to emp. entity
    public static Employee EmployeeDtoToEmployee(EmployeeDTO dto)
    {
        Employee employee = new Employee()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            WorkingNumber = dto.WorkingNumber,
            Email = dto.Email,
            Id = dto.Id,
            Password = dto.Password,
            Shifts = ListShiftDtosToListShift(dto.Shifts)
        };
        return employee;
    }
    
    public static Employee AuthEmployeeDtoToEmployee(AuthEmployeeDTO dto)
    {
        Employee employee = new Employee()
        {
            WorkingNumber = dto.WorkingNumber,
            Password = dto.Password,
        };
        return employee;
    }
    public static Employee NewEmployeeDtoToEmployee(NewEmployeeDTO dto)
    {
        Employee employee = new Employee()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            WorkingNumber = dto.WorkingNumber,
            Email = dto.Email,
            Password = dto.Password,
        };
        return employee;
    }
    public static Employee PublicEmployeeDtoToEmployee(PublicEmployeeDTO dto)
    {
        Employee employee = new Employee()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            WorkingNumber = dto.WorkingNumber,
            Id = dto.Id,
        };
        return employee;
    }
    public static Employee ShiftEmployeeDtoToEmployee(ShiftEmpoyeeDTO dto)
    {
        Employee employee = new Employee()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            WorkingNumber = dto.WorkingNumber,
            Id = dto.Id,
            Shifts = ListShiftDtosToListShift(dto.Shifts)
        };
        return employee;
    }
    public static Employee SimpleEmployeeDtoToEmployee(SimpleEmployeeDTO dto)
    {
        Employee employee = new Employee()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            WorkingNumber = dto.WorkingNumber,
            Id = dto.Id
        };
        return employee;
    }
    public static Employee UpdateEmployeeDtoToEmployee(UpdateEmployeeDTO dto)
    {
        Employee employee = new Employee()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            WorkingNumber = dto.WorkingNumber,
            Email = dto.Email,
            Password = dto.Password
        };
        return employee;
    }
    
    
    
    //all emp. entity to emp. dtos
    
    public static AuthEmployeeDTO EmployeeToAuthEmployeeDto(Employee e)
    {
        AuthEmployeeDTO dto = new AuthEmployeeDTO()
        {
            WorkingNumber = e.WorkingNumber,
            Password = e.Password,
        };
        return dto;
    }
    
    public static EmployeeDTO EmployeeToEmployeeDto(Employee e)
    {
        EmployeeDTO dto = new EmployeeDTO()
        {
            FirstName = e.FirstName,
            LastName = e.LastName,
            WorkingNumber = e.WorkingNumber,
            Email = e.Email,
            Id = e.Id,
            Password = e.Password,
            Shifts = ListShiftToListShiftDtos(e.Shifts)
        };
        return dto;
    }
    
    public static NewEmployeeDTO EmployeeToNewEmployeeDto(Employee e)
    {
        NewEmployeeDTO dto = new NewEmployeeDTO()
        {
            FirstName = e.FirstName,
            LastName = e.LastName,
            WorkingNumber = e.WorkingNumber,
            Email = e.Email,
            Password = e.Password,
        };
        return dto;
    }
    
    public static PublicEmployeeDTO EmployeeToPublicEmployeeDto(Employee e)
    {
        PublicEmployeeDTO dto = new PublicEmployeeDTO()
        {
            FirstName = e.FirstName,
            LastName = e.LastName,
            WorkingNumber = e.WorkingNumber,
            Id = e.Id,
        };
        return dto;
    }
    
    public static ShiftEmpoyeeDTO EmployeeToShiftEmployeeDto(Employee e)
    {
        ShiftEmpoyeeDTO dto = new ShiftEmpoyeeDTO()
        {
            FirstName = e.FirstName,
            LastName = e.LastName,
            WorkingNumber = e.WorkingNumber,
            Id = e.Id,
            Shifts = ListShiftToListShiftDtos(e.Shifts)
        };
        return dto;
    }
    
    public static SimpleEmployeeDTO EmployeeToSimpleEmployeeDto(Employee e)
    {
        SimpleEmployeeDTO dto = new SimpleEmployeeDTO()
        {
            FirstName = e.FirstName,
            LastName = e.LastName,
            WorkingNumber = e.WorkingNumber,
            Id = e.Id,
        };
        return dto;
    }
    
    public static UpdateEmployeeDTO EmployeeToUpdateEmployeeDto(Employee e)
    {
        UpdateEmployeeDTO dto = new UpdateEmployeeDTO()
        {
            FirstName = e.FirstName,
            LastName = e.LastName,
            WorkingNumber = e.WorkingNumber,
            Email = e.Email,
            Password = e.Password,
        };
        return dto;
    }
    
    
    //all shiftDtos to shift entities

    public static List<Entities.Shift> ListShiftDtosToListShift(List<ShiftDTO> dtos)
    {
        List<Entities.Shift> shiftsToReturn = new List<Entities.Shift>();
        foreach (var dto in dtos)
        {
            Entities.Shift shift = new Entities.Shift()
            {
                Id = dto.Id,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                TypeOfShift = dto.TypeOfShift,
                ShiftStatus = dto.ShiftStatus,
                Description = dto.Description,
                Location = dto.Location,
                AssingnedEmployees = dto.AssignedEmployees
            };
            shiftsToReturn.Add(shift);
        }

        return shiftsToReturn;
    }
    public static Entities.Shift NewShiftDtoToShift(NewShiftDTO dto)
    {
        Entities.Shift shift = new Entities.Shift()
        {
            StartDateTime = dto.StartDateTime,
            EndDateTime = dto.EndDateTime,
            TypeOfShift = dto.TypeOfShift,
            ShiftStatus = dto.ShiftStatus,
            Description = dto.Description,
            Location = dto.Location,
        };

        return shift;
    }
    public static Entities.Shift ShiftDtoToShift(ShiftDTO dto)
    {
        Entities.Shift shift = new Entities.Shift()
        {
            Id = dto.Id,
            StartDateTime = dto.StartDateTime,
            EndDateTime = dto.EndDateTime,
            TypeOfShift = dto.TypeOfShift,
            ShiftStatus = dto.ShiftStatus,
            Description = dto.Description,
            Location = dto.Location,
            AssingnedEmployees = dto.AssignedEmployees
        };
        
        return shift;
    }
    // public static Entities.Shift ShiftProposalDtoToShift(ShiftProposalDTO dto)
    // {
    //     Entities.Shift shift = new Entities.Shift()
    //     {
    //         Id = dto.Id,
    //         StartDateTime = dto.StartDateTime,
    //         EndDateTime = dto.EndDateTime,
    //         TypeOfShift = dto.TypeOfShift,
    //         ShiftStatus = dto.ShiftStatus,
    //         Description = dto.Description,
    //         Location = dto.Location,
    //         AssingnedEmployees = dto.AssignedEmployees
    //     };
    //     
    //     return shift;
    // }
    public static ShiftSwitchReply ShiftSwitchReplyDtoToShiftSwitchReply(ShiftSwitchReplyDTO dto, IShiftRepository _shiftRepository)
    {
        ShiftSwitchReply shift = new ShiftSwitchReply()
        {
            Id = dto.Id,
            TargetShift = _shiftRepository.GetSingleAsync(dto.TargetShiftId).Result
        };
        
        return shift;
    }
    public static Entities.Shift ShiftSwitchRequestDtoToShift(ShiftSwitchRequestDTO dto)
    {
        throw new NotImplementedException();
    }
    public static Entities.Shift ShiftSwitchRequestTimeframeDtoToShift(ShiftSwitchRequestTimeframeDTO dto)
    {
        throw new NotImplementedException();
    }
    
    
    //all shift entity to shift dtos
    
    public static List<ShiftDTO> ListShiftToListShiftDtos(List<Entities.Shift> list)
    {
        throw new NotImplementedException();
    }
    public static NewShiftDTO ShiftToNewShiftDto(Entities.Shift s)
    {
        throw new NotImplementedException();
    }
    
    public static ShiftDTO ShiftToShiftDto(Entities.Shift s)
    {
        throw new NotImplementedException();
    }
    
    public static ShiftProposalDTO ShiftToShiftProposalDto(Entities.Shift s)
    {
        throw new NotImplementedException();
    }
    
    public static ShiftSwitchReply ShiftToShiftSwitchReplyDto(Entities.ShiftSwitchReply s)
    {
        throw new NotImplementedException();
    }
    
    public static ShiftSwitchRequest ShiftToShiftSwitchRequest(Entities.ShiftSwitchRequest s)
    {
        throw new NotImplementedException();
    }
    
    public static ShiftSwitchRequestTimeframeDTO ShiftToShiftSwitchRequestTimeframeDto(Entities.ShiftSwitchRequestTimeframe s)
    {
        throw new NotImplementedException();
    }
}