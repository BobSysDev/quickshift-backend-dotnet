using Microsoft.VisualBasic.FileIO;

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
        throw new NotImplementedException();
    }
    
    public static UpdateEmployeeDTO EmployeeToGrpcUpdateEmployeeDto(Entities.Employee e)
    {
        throw new NotImplementedException();
    }

    
    //FOR SHIFT (grpcDto -> shift)
    public static Entities.Shift GrpcShiftDtoToShift(ShiftDTO dto)
    {
        throw new NotImplementedException();
    }
    public static Entities.Shift GrpcNewShiftDtoToShift(NewShiftDTO dto)
    {
        throw new NotImplementedException();
    }
    //list
    public static List<Entities.Shift> GrpcShiftDtoListToListShifts(ShiftDTOList dtos)
    {
        throw new NotImplementedException();
    }


    //FOR SHIFT (shift -> grpcDto)
    public static ShiftDTO ShiftToGrpcShiftDto(Entities.Shift s)
    {
        throw new NotImplementedException();
    }
    
    public static NewShiftDTO ShiftToGrpcNewShiftDto(Entities.Shift s)
    {
        throw new NotImplementedException();
    }
    //list
    public static ShiftDTOList ListShiftsToGrpcShiftDtoList(List<Entities.Shift> shifts)
    {
        throw new NotImplementedException();
    }
    
    
    //FOR REPLY (grpcDto -> reply)
    public static Entities.ShiftSwitchReply GrpcReplyDtoToShiftSwitchReply(ReplyDTO dto)
    {
        throw new NotImplementedException();
    }
    
    public static Entities.ShiftSwitchReply GrpcNewReplyDtoToShiftSwitchReply(NewReplyDTO dto)
    {
        throw new NotImplementedException();
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

