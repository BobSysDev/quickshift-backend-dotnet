using System.Numerics;
using System.Runtime.InteropServices;
using DTOs.Announcements;
using DTOs.Shift;
using DTOs.ShiftSwitching;
using Entities;
using RepositoryContracts;


namespace DTOs;

public class EntityDtoConverter
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
            Shifts = ListShiftDtosToListShift(dto.Shifts),
            IsManager = dto.IsManager
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
            IsManager = dto.IsManager
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
            IsManager = dto.IsManager
        };
        return employee;
    }
    // public static Employee ShiftEmployeeDtoToEmployee(ShiftEmpoyeeDTO dto)
    // {
    //     Employee employee = new Employee()
    //     {
    //         FirstName = dto.FirstName,
    //         LastName = dto.LastName,
    //         WorkingNumber = dto.WorkingNumber,
    //         Id = dto.Id,
    //         Shifts = ListShiftDtosToListShift(dto.Shifts),
    //         IsManager = dto.IsManager
    //     };
    //     return employee;
    // }
    public static Employee SimpleEmployeeDtoToEmployee(SimpleEmployeeDTO dto)
    {
        Employee employee = new Employee()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            WorkingNumber = dto.WorkingNumber,
            Id = dto.Id,
            IsManager = dto.IsManager
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
            Password = dto.Password,
            IsManager = dto.IsManager
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
            Shifts = ListShiftToListShiftDtos(e.Shifts),
            IsManager = e.IsManager
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
            IsManager = e.IsManager
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
            IsManager = e.IsManager
        };
        return dto;
    }
    
    // public static ShiftEmpoyeeDTO EmployeeToShiftEmployeeDto(Employee e)
    // {
    //     ShiftEmpoyeeDTO dto = new ShiftEmpoyeeDTO()
    //     {
    //         FirstName = e.FirstName,
    //         LastName = e.LastName,
    //         WorkingNumber = e.WorkingNumber,
    //         Id = e.Id,
    //         Shifts = ListShiftToListShiftDtos(e.Shifts),
    //         IsManager = e.IsManager
    //     };
    //     return dto;
    // }
    
    public static SimpleEmployeeDTO EmployeeToSimpleEmployeeDto(Employee e)
    {
        SimpleEmployeeDTO dto = new SimpleEmployeeDTO()
        {
            FirstName = e.FirstName,
            LastName = e.LastName,
            WorkingNumber = e.WorkingNumber,
            Id = e.Id,
            IsManager = e.IsManager
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
            IsManager = e.IsManager
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
    
    public static async Task<ShiftSwitchReply> ShiftSwitchReplyDtoToShiftSwitchReply(ShiftSwitchReplyDTO dto, IShiftRepository _shiftRepository)
    {
        ShiftSwitchReply shift = new ShiftSwitchReply()
        {
            Id = dto.Id,
            TargetShift = await _shiftRepository.GetSingleAsync(dto.TargetShiftId)
        };
        
        return shift;
    }
    public static async Task<ShiftSwitchRequest> ShiftSwitchRequestDtoToShiftSwitchRequest(ShiftSwitchRequestDTO dto, IShiftRepository _shiftRepository, IEmployeeRepository _employeeRepository)
    {
        ShiftSwitchRequest shift = new ShiftSwitchRequest()
        {
            Id = dto.Id,
            OriginShift = await _shiftRepository.GetSingleAsync(dto.OriginShiftId),
            OriginEmployee = await _employeeRepository.GetSingleAsync(dto.OriginEmployeeId),
            Details = dto.Details
        };
        
        return shift;
    }
    public static ShiftSwitchRequestTimeframe ShiftSwitchRequestTimeframeDtoToShiftSwitchRequestTimeframe(ShiftSwitchRequestTimeframeDTO dto)
    {
        ShiftSwitchRequestTimeframe shift = new ShiftSwitchRequestTimeframe()
        {
            Id = dto.Id,
            TimeFrameStart = dto.StartDate,
            TimeFrameEnd = dto.EndDate
        };
        
        return shift;
    }
    
    
    //all shift entity to shift dtos
    //list1
    public static List<ShiftDTO> ListShiftToListShiftDtos(List<Entities.Shift> list)
    {
        List<ShiftDTO> shiftDtosToReturn = new List<ShiftDTO>();
        foreach (var shift in list)
        {
            ShiftDTO dto = new ShiftDTO()
            {
                Id = shift.Id,
                StartDateTime = shift.StartDateTime,
                EndDateTime = shift.EndDateTime,
                TypeOfShift = shift.TypeOfShift,
                ShiftStatus = shift.ShiftStatus,
                Description = shift.Description,
                Location = shift.Location,
                AssignedEmployees = shift.AssingnedEmployees
            };
            shiftDtosToReturn.Add(dto);
        }

        return shiftDtosToReturn;
    }
    
    //list2
    public static List<ShiftSwitchReplyDTO> ListShiftSwitchRepliesToListShiftSwitchReplyDtos(List<ShiftSwitchReply> list, long requestId)
    {
        List<ShiftSwitchReplyDTO> shiftDtosToReturn = new List<ShiftSwitchReplyDTO>();
        foreach (var reply in list)
        {
            ShiftSwitchReplyDTO dto = new ShiftSwitchReplyDTO()
            {
                Id = reply.Id,
                RequestId = requestId,
                TargetEmployeeId = reply.TargetEmployee.Id,
                TargetShiftId = reply.TargetShift.Id,
                Details = reply.Details,
                OriginAccepted = reply.OriginAccepted,
                TargetAccepted = reply.TargetAccepted
            };
            shiftDtosToReturn.Add(dto);
        }

        return shiftDtosToReturn;
    }
    
   
    
    public static NewShiftDTO ShiftToNewShiftDto(Entities.Shift s)
    {
        NewShiftDTO dto = new NewShiftDTO()
        {
            StartDateTime = s.StartDateTime,
            EndDateTime = s.EndDateTime,
            TypeOfShift = s.TypeOfShift,
            ShiftStatus = s.ShiftStatus,
            Description = s.Description,
            Location = s.Location,
        };
        
        return dto;
    }
    
    public static ShiftDTO ShiftToShiftDto(Entities.Shift s)
    {
        ShiftDTO dto = new ShiftDTO()
        {
            Id = s.Id,
            StartDateTime = s.StartDateTime,
            EndDateTime = s.EndDateTime,
            TypeOfShift = s.TypeOfShift,
            ShiftStatus = s.ShiftStatus,
            Description = s.Description,
            Location = s.Location,
            AssignedEmployees = s.AssingnedEmployees
        };
        
        return dto;
    }
    
    
    public static ShiftSwitchReplyDTO ShiftSwitchReplyToShiftSwitchReplyDto(Entities.ShiftSwitchReply s, long requestId)
    {
        ShiftSwitchReplyDTO dto = new ShiftSwitchReplyDTO()
        {
            Id = s.Id,
            RequestId = requestId,
            TargetEmployeeId = s.TargetEmployee.Id,
            TargetShiftId = s.TargetShift.Id,
            Details = s.Details,
            OriginAccepted = s.OriginAccepted,
            TargetAccepted = s.TargetAccepted
        };
        
        return dto;
    }
    
    public static ShiftSwitchRequestDTO ShiftSwitchRequestToShiftSwitchRequestDto(Entities.ShiftSwitchRequest s)
    {
        ShiftSwitchRequestDTO dto = new ShiftSwitchRequestDTO()
        {
            Id = s.Id,
            OriginEmployeeId = s.OriginEmployee.Id,
            OriginShiftId = s.OriginShift.Id,
            Details = s.Details,
            ReplyDtos = ListShiftSwitchRepliesToListShiftSwitchReplyDtos(s.SwitchReplies,s.Id),
            TimeframeDtos = ListShiftSwitchRequestTimeframesToListShiftSwitchRequestTimeframeDtos(s.Timeframes)
        };
        
        return dto;
    }
    
    public static ShiftSwitchRequestTimeframeDTO ShiftSwitchRequestTimeframeToShiftSwitchRequestTimeframeDto(Entities.ShiftSwitchRequestTimeframe s, long requestId)
    {
        ShiftSwitchRequestTimeframeDTO dto = new ShiftSwitchRequestTimeframeDTO()
        {
            Id = s.Id,
            RequestId = requestId,
            StartDate = s.TimeFrameStart,
            EndDate = s.TimeFrameEnd
        };
        return dto;
    }
    
    //other shift switch ones: (dtos->entities)
    //--replies
    public static async Task<ShiftSwitchReply> NewShiftSwitchReplyDtoToShiftSwitchReply(NewShiftSwitchReplyDTO dto, IShiftRepository _shiftRepository, IEmployeeRepository _employeeRepository )
    {
        ShiftSwitchReply reply = new ShiftSwitchReply()
        {
            TargetShift = await _shiftRepository.GetSingleAsync(dto.TargetShiftId),
            TargetEmployee = await _employeeRepository.GetSingleAsync(dto.TargetEmployeeId),
            Details = dto.Details
        };
        return reply;
    }
    
    public static ShiftSwitchReply UpdateShiftSwitchReplyDtoToShiftSwitchReply(UpdateShiftSwitchReplyDTO dto)
    {
        ShiftSwitchReply reply = new ShiftSwitchReply()
        {
            Details = dto.Details
        };
        return reply;
    }
    //--requests
    public static async Task<ShiftSwitchRequest> NewShiftSwitchRequestDtoToShiftSwitchRequest(NewShiftSwitchRequestDTO dto, IShiftRepository _shiftRepository, IEmployeeRepository _employeeRepository )
    {
        // try
        // {
            ShiftSwitchRequest request = new ShiftSwitchRequest()
            {
                OriginEmployee = await _employeeRepository.GetSingleAsync(dto.OriginEmployeeId),
                OriginShift = await _shiftRepository.GetSingleAsync(dto.OriginShiftId),
                Details = dto.Details,
                Timeframes = ListShiftSwitchRequestTimeframeDtosToListShiftSwitchRequestTimeframes(dto.TimeframeDtos)
            };
            return request;
        // }
        // catch (ArgumentException e)
        // {
        //     throw e;
        // }
        // catch (InvalidOperationException e)
        // {
        //     throw new InvalidOperationException(e.Message);
        // }
       
    }
    
    public static ShiftSwitchRequest UpdateShiftSwitchRequestDtoToShiftSwitchRequest(UpdateShiftSwitchRequestDTO dto )
    {
        ShiftSwitchRequest request = new ShiftSwitchRequest()
        {
            Details = dto.Details
        };
        return request;
    }
    
    //--timeframes
    public static ShiftSwitchRequestTimeframe NewShiftSwitchRequestTimeframeDtoToShiftSwitchRequestTimeframe(NewShiftSwitchRequestTimeframeDTO dto)
    {
        ShiftSwitchRequestTimeframe timeframe = new ShiftSwitchRequestTimeframe()
        {
            TimeFrameStart = dto.StartDate,
            TimeFrameEnd = dto.EndDate
        };
        return timeframe;
    }
    
    //list3
    public static List<ShiftSwitchRequestTimeframeDTO> ListShiftSwitchRequestTimeframesToListShiftSwitchRequestTimeframeDtos(List<ShiftSwitchRequestTimeframe> list)
    {
        List<ShiftSwitchRequestTimeframeDTO> TrequestTimeframeDtos = new List<ShiftSwitchRequestTimeframeDTO>();
        foreach (var request in list)
        {
            ShiftSwitchRequestTimeframeDTO dto = new ShiftSwitchRequestTimeframeDTO()
            {
                Id = request.Id,
                RequestId = request.Id,
                StartDate = request.TimeFrameStart,
                EndDate = request.TimeFrameEnd
            };
            TrequestTimeframeDtos.Add(dto);
        }

        return TrequestTimeframeDtos;
    }
    
    //list4
    public static List<ShiftSwitchRequestTimeframe> ListShiftSwitchRequestTimeframeDtosToListShiftSwitchRequestTimeframes(List<ShiftSwitchRequestTimeframeDTO> list)
    {
        List<ShiftSwitchRequestTimeframe> timeframes = new List<ShiftSwitchRequestTimeframe>();
        foreach (var reply in list)
        {
            ShiftSwitchRequestTimeframe timeframe = new ShiftSwitchRequestTimeframe()
            {
                Id = reply.Id,
                TimeFrameStart = reply.StartDate,
                TimeFrameEnd = reply.EndDate,
            };
            timeframes.Add(timeframe);
        }

        return timeframes;
    }
    
    // Announcements
    public static Announcement NewAnnouncementDtoToEntity(NewAnnouncementDTO dto)
    {
        return new Announcement
        {
            Id = 0,
            AuthorId = dto.AuthorId,
            Title = dto.Title,
            Body = dto.Body,
            DateTimeOfPosting = dto.DateTimeOfPosting
        };
    }

    public async static Task<AnnouncementDTO> AnnouncementEntityToDto(Announcement a, IEmployeeRepository empRep)
    {
        Employee author = await empRep.GetSingleAsync(a.AuthorId);

        return new AnnouncementDTO
        {
            Id = a.Id,
            AuthorId = a.AuthorId,
            AuthorName = author.FirstName + " " + author.LastName,
            AuthorWorkingNumber = author.WorkingNumber,
            Title = a.Title,
            Body = a.Body,
            DateTimeOfPosting = a.DateTimeOfPosting
        };
    }
}