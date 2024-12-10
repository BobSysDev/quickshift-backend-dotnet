using System.Xml;
using Google.Protobuf.Collections;
using Microsoft.VisualBasic.FileIO;
using RepositoryContracts;

namespace GrpcClient;

public class GrpcDtoConverter
{
    
    //FOR EMPLOYEE: (grpcDto->emp)
    public static Entities.Employee GrpcEmployeeDtoToEmployee(EmployeeDTO dto)
    {
        Entities.Employee employee = new Entities.Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Id = dto.Id,
            WorkingNumber = int.CreateChecked(dto.WorkingNumber),
            Shifts = GrpcShiftDtoListToListShifts(dto.AssignedShifts), //TODO this method
            Email = dto.Email,
            Password = dto.Password,
        };
        return employee;
    }
    
    public static Entities.Employee GrpcNewEmployeeDtoToEmployee(NewEmployeeDTO dto)
    {
        Entities.Employee employee = new Entities.Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            WorkingNumber = int.CreateChecked(dto.WorkingNumber),
            Email = dto.Email,
            Password = dto.Password,
        };
        return employee;
    }
    
    public static Entities.Employee GrpcUpdateEmployeeDtoToEmployee(UpdateEmployeeDTO dto)
    {
        Entities.Employee employee = new Entities.Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Id = dto.Id,
            WorkingNumber = int.CreateChecked(dto.WorkingNumber),
            Email = dto.Email,
            Password = dto.Password,
        };
        return employee;
    }
    
    //list
    
    public static List<Entities.Employee>GrpcEmployeeDtoListToEntityEmployeeList(EmployeeDTOList list)
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
                Shifts = GrpcShiftDtoListToListShifts(dto.AssignedShifts),
                Email = dto.Email,
                Password = dto.Password
            };
            employees.Add(employee);
        }
        
        return employees;
    }
    
    
    //FOR EMPLOYEE: (emp->grpcDto)
    public static EmployeeDTO EmployeeToGrpcEmployeeDto(Entities.Employee e)
    {
        EmployeeDTO dto = new EmployeeDTO
        {
            FirstName = e.FirstName,
            LastName = e.LastName,
            Id = e.Id,
            WorkingNumber = uint.CreateChecked(e.WorkingNumber),
            AssignedShifts = ListShiftsToGrpcShiftDtoList(e.Shifts), 
            Email = e.Email,
            Password = e.Password,
        };
        return dto;
    }

    public static NewEmployeeDTO EmployeeToGrpcNewEmployeeDto(Entities.Employee e)
    {
        NewEmployeeDTO dto = new NewEmployeeDTO
        {
            FirstName = e.FirstName,
            LastName = e.LastName,
            WorkingNumber = uint.CreateChecked(e.WorkingNumber),
            Email = e.Email,
            Password = e.Password,
        };
        return dto;
    }
    
    public static UpdateEmployeeDTO EmployeeToGrpcUpdateEmployeeDto(Entities.Employee e)
    {
        UpdateEmployeeDTO dto = new UpdateEmployeeDTO
        {
            FirstName = e.FirstName,
            LastName = e.LastName,
            Id = e.Id,
            WorkingNumber = uint.CreateChecked(e.WorkingNumber),
            Email = e.Email,
            Password = e.Password,
        };
        return dto;
    }

    
    //FOR SHIFT (grpcDto -> shift)
    public static Entities.Shift GrpcShiftDtoToShift(ShiftDTO dto)
    {
        Entities.Shift shift = new Entities.Shift()
        {
            Id = dto.Id,
            StartDateTime = DateTime.UnixEpoch.AddMilliseconds(dto.StartDateTime),
            EndDateTime = DateTime.UnixEpoch.AddMilliseconds(dto.EndDateTime),
            TypeOfShift = dto.TypeOfShift,
            ShiftStatus = dto.ShiftStatus,
            Description = dto.Description,
            Location = dto.Location,
            AssingnedEmployees = dto.AssignedEmployeeIds.ToList()
        };
        return shift;
    }
    public static Entities.Shift GrpcNewShiftDtoToShift(NewShiftDTO dto)
    {
        Entities.Shift shift = new Entities.Shift()
        {
            StartDateTime = DateTime.UnixEpoch.AddMilliseconds(dto.StartDateTime),
            EndDateTime = DateTime.UnixEpoch.AddMilliseconds(dto.EndDateTime),
            TypeOfShift = dto.TypeOfShift,
            ShiftStatus = dto.ShiftStatus,
            Description = dto.Description,
            Location = dto.Location,
        };
        return shift;
    }

    //TODO:DELETE
    
    // public static Entities.ShiftSwitchReply ShiftSwitchReplyDtoToShiftSwitch(ReplyDTO dto, IShiftRepository shiftRepository, IEmployeeRepository employeeRepository)
    // {
    //     Entities.ShiftSwitchReply shiftSwitchReply = new Entities.ShiftSwitchReply()
    //     {
    //         Details = dto.Details,
    //         Id = dto.Id,
    //         OriginAccepted = dto.OriginAccepted,
    //         TargetEmployee = employeeRepository.GetSingleAsync(dto.TargetEmployeeId).Result,
    //         TargetAccepted = dto.TargetAccepted,
    //         TargetShift = shiftRepository.GetSingleAsync(dto.TargetShiftId).Result
    //     };
    //     return shiftSwitchReply;
    // }
    
    //list
    public static List<Entities.Shift> GrpcShiftDtoListToListShifts(ShiftDTOList dtos)
    {
        List<ShiftDTO> dtos2 = dtos.Dtos.ToList();
        List<Entities.Shift> shifts = new List<Entities.Shift>();
        foreach (var dto in dtos2)
        {
            Entities.Shift shift = new Entities.Shift()
            {
                Id = dto.Id,
                StartDateTime = DateTime.UnixEpoch.AddMilliseconds(dto.StartDateTime),
                EndDateTime = DateTime.UnixEpoch.AddMilliseconds(dto.EndDateTime),
                TypeOfShift = dto.TypeOfShift,
                ShiftStatus = dto.ShiftStatus,
                Description = dto.Description,
                Location = dto.Location,
                AssingnedEmployees = dto.AssignedEmployeeIds.ToList()
            };
            shifts.Add(shift);
        }

        return shifts;
    }
    
    


    //FOR SHIFT (shift -> grpcDto)
    public static ShiftDTO ShiftToGrpcShiftDto(Entities.Shift s)
    {
        RepeatedField<long> Ids = new RepeatedField<long>();
        Ids.Add(s.AssingnedEmployees);
        ShiftDTO dto = new ShiftDTO()
        {
            Id = s.Id,
            StartDateTime = new DateTimeOffset(s.StartDateTime).ToUnixTimeMilliseconds(),
            EndDateTime = new DateTimeOffset(s.EndDateTime).ToUnixTimeMilliseconds(),
            TypeOfShift = s.TypeOfShift,
            ShiftStatus = s.ShiftStatus,
            Description = s.Description,
            Location = s.Location,
            AssignedEmployeeIds = { Ids }
            
        };
        return dto;
    }
    
    public static NewShiftDTO ShiftToGrpcNewShiftDto(Entities.Shift s)
    {
        NewShiftDTO dto = new NewShiftDTO()
        {
            StartDateTime = new DateTimeOffset(s.StartDateTime).ToUnixTimeMilliseconds(),
            EndDateTime = new DateTimeOffset(s.EndDateTime).ToUnixTimeMilliseconds(),
            TypeOfShift = s.TypeOfShift,
            ShiftStatus = s.ShiftStatus,
            Description = s.Description,
            Location = s.Location
        };
        return dto;
    }
    //list
    public static ShiftDTOList ListShiftsToGrpcShiftDtoList(List<Entities.Shift> shifts)
    {
        ShiftDTOList shiftsToReturn = new ShiftDTOList();
        foreach (var shift in shifts)
        {
            ShiftDTO dto = new ShiftDTO()
            {
                Id = shift.Id,
                StartDateTime = new DateTimeOffset(shift.StartDateTime).ToUnixTimeMilliseconds(),
                EndDateTime = new DateTimeOffset(shift.EndDateTime).ToUnixTimeMilliseconds(),
                TypeOfShift = shift.TypeOfShift,
                ShiftStatus = shift.ShiftStatus,
                Description = shift.Description,
                Location = shift.Location,
                AssignedEmployeeIds = { shift.AssingnedEmployees }
            };
            shiftsToReturn.Dtos.Add(dto);
        }

        return shiftsToReturn;
    }
    
    public static List<ShiftDTO> ShiftListToListOfGrpcShiftDto(List<Entities.Shift> shifts)
    {
        List<ShiftDTO> shiftsToReturn = new List<ShiftDTO>();
        foreach (var shift in shifts)
        {
            ShiftDTO dto = new ShiftDTO()
            {
                Id = shift.Id,
                StartDateTime = new DateTimeOffset(shift.StartDateTime).ToUnixTimeMilliseconds(),
                EndDateTime = new DateTimeOffset(shift.EndDateTime).ToUnixTimeMilliseconds(),
                TypeOfShift = shift.TypeOfShift,
                ShiftStatus = shift.ShiftStatus,
                Description = shift.Description,
                Location = shift.Location,
                AssignedEmployeeIds = { shift.AssingnedEmployees }
            };
            shiftsToReturn.Add(dto);
        }

        return shiftsToReturn;
    }
    
    
    //FOR REPLY (grpcDto -> reply)
    public static Entities.ShiftSwitchReply GrpcReplyDtoToShiftSwitchReply(ReplyDTO dto, IShiftRepository _shiftRepository, IEmployeeRepository _employeeRepository)
    {
        Entities.ShiftSwitchReply reply = new Entities.ShiftSwitchReply()
        {
            Id = dto.Id,
            TargetShift = _shiftRepository.GetSingleAsync(dto.TargetShiftId).Result,
            TargetEmployee = _employeeRepository.GetSingleAsync(dto.TargetEmployeeId).Result,
            TargetAccepted = dto.TargetAccepted,
            OriginAccepted = dto.OriginAccepted,
            Details = dto.Details,
        };
        return reply;
    }
    
    public static Entities.ShiftSwitchReply GrpcNewReplyDtoToShiftSwitchReply(NewReplyDTO dto, IShiftRepository _shiftRepository, IEmployeeRepository _employeeRepository)
    {
        Entities.ShiftSwitchReply reply = new Entities.ShiftSwitchReply()
        {
            TargetShift = _shiftRepository.GetSingleAsync(dto.TargetShiftId).Result,
            TargetEmployee = _employeeRepository.GetSingleAsync(dto.TargetEmployeeId).Result,
            Details = dto.Details
        };
        return reply;
    }
    
    //list
    public static List<Entities.ShiftSwitchReply> GrpcShiftSwitchReplyListDtoToShiftSwitchRequestList(ReplyDTOList dtos, IShiftRepository _shiftRepository, IEmployeeRepository _employeeRepository)
    {
        List<ReplyDTO> dtos2 = dtos.Dtos.ToList();
        List<Entities.ShiftSwitchReply> replies = new List<Entities.ShiftSwitchReply>();
        foreach (var dto in dtos2)
        {
            Entities.ShiftSwitchReply shift = GrpcReplyDtoToShiftSwitchReply(dto, _shiftRepository, _employeeRepository);
            replies.Add(shift);
        }

        return replies;
    }
    
    //FOR REPLY (reply -> grpcDto)
    public static ReplyDTO ShiftSwitchReplyToGrpcReplyDto(Entities.ShiftSwitchReply r)
    {
        ReplyDTO dto = new ReplyDTO()
        {
            Id = r.Id,
            TargetShiftId = r.TargetShift.Id,
            TargetEmployeeId = r.TargetEmployee.Id,
            TargetAccepted = r.TargetAccepted,
            OriginAccepted = r.OriginAccepted,
            Details = r.Details,
        };
        return dto;
    }
    
    public static NewReplyDTO ShiftSwitchReplyToGrpcNewReplyDto(Entities.ShiftSwitchReply r)
    {
        NewReplyDTO dto = new NewReplyDTO()
        {
            TargetShiftId = r.TargetShift.Id,
            TargetEmployeeId = r.TargetEmployee.Id,
            Details = r.Details,
        };
        return dto;
    }
    
    public static UpdateReplyDTO ShiftSwitchReplyToGrpcUpdateReplyDto(Entities.ShiftSwitchReply r)
    {
        UpdateReplyDTO dto = new UpdateReplyDTO()
        {
            Details = r.Details,
            Id = r.Id
        };
        return dto;
    }
    //list
    public static ReplyDTOList ShiftSwitchReplyListToGrpcShiftSwitchReplyListDto(List<Entities.ShiftSwitchReply> replies)
    {
        ReplyDTOList shiftsToReturn = new ReplyDTOList();
        foreach (var r in replies)
        {
            ReplyDTO dto = new ReplyDTO()
            {
                Id = r.Id,
                TargetShiftId = r.TargetShift.Id,
                TargetEmployeeId = r.TargetEmployee.Id,
                TargetAccepted = r.TargetAccepted,
                OriginAccepted = r.OriginAccepted,
                Details = r.Details,
            };
            shiftsToReturn.Dtos.Add(dto);
        }

        return shiftsToReturn;
    }
    
    
    //FOR REQUEST (grpcDto -> request)
    public static Entities.ShiftSwitchRequest GrpcRequestDtoToShiftSwitchRequest(RequestDTO dto, IShiftRepository _shiftRepository, IEmployeeRepository _employeeRepository)
    {
        Entities.ShiftSwitchRequest request = new Entities.ShiftSwitchRequest()
        {
            Id = dto.Id,
            OriginShift = _shiftRepository.GetSingleAsync(dto.OriginShiftId).Result,
            OriginEmployee = _employeeRepository.GetSingleAsync(dto.OriginEmployeeId).Result,
            Details = dto.Details,
            SwitchReplies =
                GrpcShiftSwitchReplyListDtoToShiftSwitchRequestList(dto.Replies, _shiftRepository, _employeeRepository),
            Timeframes = ShiftSwitchRequestTimeframeDtoListToShiftSwitchRequestTimeframeList(dto.Timeframes)
        };
        return request;
    }
    
    
    
    public static Entities.ShiftSwitchRequest GrpcNewRequestDtoToShiftSwitchRequest(NewRequestDTO dto, IShiftRepository _shiftRepository, IEmployeeRepository _employeeRepository)
    {
        Entities.ShiftSwitchRequest request = new Entities.ShiftSwitchRequest()
        {
            OriginShift = _shiftRepository.GetSingleAsync(dto.OriginShiftId).Result,
            OriginEmployee = _employeeRepository.GetSingleAsync(dto.OriginEmployeeId).Result,
            Details = dto.Details,
            Timeframes = ShiftSwitchRequestTimeframeDtoListToShiftSwitchRequestTimeframeList(dto.Timeframes)
        };
        return request;
    }
    
    public static Entities.ShiftSwitchRequest GrpcUpdateRequestDtoToShiftSwitchRequest(UpdateRequestDTO dto)
    {
        Entities.ShiftSwitchRequest request = new Entities.ShiftSwitchRequest()
        {
            Id = dto.Id,
            Details = dto.Details,
        };
        return request;
    }
    
    
    //FOR REQUEST (request -> grpcDto)
    public static RequestDTO ShiftSwitchRequestToGrpcRequestDto(Entities.ShiftSwitchRequest r)
    {
        RequestDTO request = new RequestDTO()
        {
            Id = r.Id,
            OriginShiftId = r.OriginShift.Id,
            OriginEmployeeId = r.OriginEmployee.Id,
            Details = r.Details,
            Replies = ShiftSwitchReplyListToGrpcShiftSwitchReplyListDto(r.SwitchReplies),
            Timeframes = ShiftSwitchRequestTimeframeListToShiftSwitchRequestTimeframeDtoList(r.Timeframes)
        };
        return request;
    }
    
    public static NewRequestDTO ShiftSwitchRequestToGrpcNewRequestDto(Entities.ShiftSwitchRequest r)
    {
        NewRequestDTO request = new NewRequestDTO()
        {
            OriginShiftId = r.OriginShift.Id,
            OriginEmployeeId = r.OriginEmployee.Id,
            Details = r.Details,
            Timeframes = ShiftSwitchRequestTimeframeListToShiftSwitchRequestTimeframeDtoList(r.Timeframes)
        };
        return request;
    }
    
    public static UpdateRequestDTO ShiftSwitchRequestToGrpcUpdateRequestDto(Entities.ShiftSwitchRequest r)
    {
        UpdateRequestDTO request = new UpdateRequestDTO()
        {
            Id = r.Id,
            Details = r.Details,
        };
        return request;
    }
    
    
    //FOR TIMEFRAME (grpcDto -> timeframe)
    public static Entities.ShiftSwitchRequestTimeframe GrpcTimeframeDtoToShiftSwitchRequestTimeframe(TimeframeDTO dto)
    {
        Entities.ShiftSwitchRequestTimeframe timeframe = new Entities.ShiftSwitchRequestTimeframe()
        {
            Id = dto.Id,
            TimeFrameStart = DateTime.UnixEpoch.AddMilliseconds(dto.TimeFrameStart),
            TimeFrameEnd = DateTime.UnixEpoch.AddMilliseconds(dto.TimeFrameEnd),
        };
        return timeframe;
    }
    
    public static Entities.ShiftSwitchRequestTimeframe GrpcNewTimeframeDtoToShiftSwitchRequestTimeframe(NewTimeframeDTO dto)
    {
        Entities.ShiftSwitchRequestTimeframe timeframe = new Entities.ShiftSwitchRequestTimeframe()
        {
            TimeFrameStart = DateTime.UnixEpoch.AddMilliseconds(dto.TimeFrameStart),
            TimeFrameEnd = DateTime.UnixEpoch.AddMilliseconds(dto.TimeFrameEnd),
        };
        return timeframe;
    }
    //list
    public static List<Entities.ShiftSwitchRequestTimeframe>
        ShiftSwitchRequestTimeframeDtoListToShiftSwitchRequestTimeframeList(TimeframeDTOList dtos)
    {
        List<Entities.ShiftSwitchRequestTimeframe> timeframes = new List<Entities.ShiftSwitchRequestTimeframe>();
        List<TimeframeDTO> dtos2 = dtos.Dtos.ToList();
        foreach (var dto in dtos2)
        {
            Entities.ShiftSwitchRequestTimeframe t = new Entities.ShiftSwitchRequestTimeframe()
            {
                Id = dto.Id,
                TimeFrameStart = DateTime.UnixEpoch.AddMilliseconds(dto.TimeFrameStart),
                TimeFrameEnd = DateTime.UnixEpoch.AddMilliseconds(dto.TimeFrameEnd),
            };
            timeframes.Add(t);
        }

        return timeframes;
    }

    //FOR TIMEFRAME (timeframe -> grpcDto)
    public static TimeframeDTO ShiftSwitchRequestTimeframeToGrpcTimeframeDto(Entities.ShiftSwitchRequestTimeframe timeframe)
    {
        TimeframeDTO dto = new TimeframeDTO()
        {
            Id = timeframe.Id,
            TimeFrameStart = new DateTimeOffset(timeframe.TimeFrameStart).ToUnixTimeMilliseconds(),
            TimeFrameEnd = new DateTimeOffset(timeframe.TimeFrameEnd).ToUnixTimeMilliseconds(),
        };
        return dto;
    }
    
    public static NewTimeframeDTO ShiftSwitchRequestTimeframeToGrpcNewTimeframeDto(Entities.ShiftSwitchRequestTimeframe timeframe)
    {
        NewTimeframeDTO dto = new NewTimeframeDTO()
        {
            TimeFrameStart = new DateTimeOffset(timeframe.TimeFrameStart).ToUnixTimeMilliseconds(),
            TimeFrameEnd = new DateTimeOffset(timeframe.TimeFrameEnd).ToUnixTimeMilliseconds(),
        };
        return dto;
    }
    //list
    public static TimeframeDTOList
        ShiftSwitchRequestTimeframeListToShiftSwitchRequestTimeframeDtoList(List<Entities.ShiftSwitchRequestTimeframe> timeframes)
    {
        TimeframeDTOList dtos = new TimeframeDTOList();
        foreach (var timeframe in timeframes)
        {
            TimeframeDTO dto = new TimeframeDTO()
            {
                Id = timeframe.Id,
                TimeFrameStart = new DateTimeOffset(timeframe.TimeFrameStart).ToUnixTimeMilliseconds(),
                TimeFrameEnd = new DateTimeOffset(timeframe.TimeFrameEnd).ToUnixTimeMilliseconds(),
                
            };
            dtos.Dtos.Add(dto);
        }

        return dtos;
    }
    
    //createShift
    public static ShiftEmployeePair ShiftIdAndEmployeeIdToShiftEmployeePair(long shiftId, long employeeId)
    {
        return new ShiftEmployeePair
        {
            ShiftId = shiftId,
            EmployeeId = employeeId
        };
    }
    
    
}

