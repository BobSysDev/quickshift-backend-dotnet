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
            StartDateTime = new DateTime(dto.StartDateTime), 
            EndDateTime = new DateTime(dto.EndDateTime), 
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
            StartDateTime = new DateTime(dto.StartDateTime), 
            EndDateTime = new DateTime(dto.EndDateTime), 
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
                StartDateTime = new DateTime(dto.StartDateTime), 
                EndDateTime = new DateTime(dto.EndDateTime), 
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
            StartDateTime = s.StartDateTime.Ticks, 
            EndDateTime = s.EndDateTime.Ticks, 
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
            StartDateTime = s.StartDateTime.Ticks, 
            EndDateTime = s.EndDateTime.Ticks, 
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
                StartDateTime = shift.StartDateTime.Ticks, 
                EndDateTime = shift.EndDateTime.Ticks, 
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
    
    
    //FOR REPLY (reply -> grpcDto)
    public static ReplyDTO ShiftSwitchReplyToGrpcReplyDto(Entities.ShiftSwitchReply r)
    {
        throw new NotImplementedException();
    }
    
    public static NewReplyDTO ShiftSwitchReplyToGrpcNewReplyDto(Entities.ShiftSwitchReply r)
    {
        throw new NotImplementedException();
    }
    
    
    //FOR REQUEST (grpcDto -> request)
    public static Entities.ShiftSwitchRequest GrpcRequestDtoToShiftSwitchRequest(RequestDTO dto)
    {
        throw new NotImplementedException();
    }
    
    public static Entities.ShiftSwitchRequest GrpcNewRequestDtoToShiftSwitchRequest(NewRequestDTO dto)
    {
        throw new NotImplementedException();
    }
    
    public static Entities.ShiftSwitchRequest GrpcUpdateRequestDtoToShiftSwitchRequest(UpdateRequestDTO dto)
    {
        throw new NotImplementedException();
    }
    
    
    //FOR REQUEST (request -> grpcDto)
    public static RequestDTO ShiftSwitchRequestToGrpcRequestDto(Entities.ShiftSwitchRequest r)
    {
        throw new NotImplementedException();
    }
    
    public static NewRequestDTO ShiftSwitchRequestToGrpcNewRequestDto(Entities.ShiftSwitchRequest r)
    {
        throw new NotImplementedException();
    }
    
    public static UpdateRequestDTO ShiftSwitchRequestToGrpcUpdateNewRequestDto(Entities.ShiftSwitchRequest r)
    {
        throw new NotImplementedException();
    }
    
    
    //FOR TIMEFRAME (grpcDto -> timeframe)
    public static Entities.ShiftSwitchRequestTimeframe GrpcTimeframeDtoToShiftSwitchRequestTimeframe(TimeframeDTO dto)
    {
        throw new NotImplementedException();
    }
    
    public static Entities.ShiftSwitchRequestTimeframe GrpcNewTimeframeDtoToShiftSwitchRequestTimeframe(NewTimeframeDTO dto)
    {
        throw new NotImplementedException();
    }
    
    
    //FOR TIMEFRAME (timeframe -> grpcDto)
    public static TimeframeDTO ShiftSwitchRequestTimeframeToGrpcTimeframeDto(Entities.ShiftSwitchRequestTimeframe dto)
    {
        throw new NotImplementedException();
    }
    
    public static NewTimeframeDTO ShiftSwitchRequestTimeframeToGrpcNewTimeframeDto(Entities.ShiftSwitchRequestTimeframe dto)
    {
        throw new NotImplementedException();
    }
}

