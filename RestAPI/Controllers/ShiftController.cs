using DTOs.Shift;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using GrpcClient;
using Shift = Entities.Shift;
using ShiftDTO = DTOs.Shift.ShiftDTO;

namespace RestAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ShiftController : ControllerBase
{
    private readonly IShiftRepository _shiftRepository;

    public ShiftController(IShiftRepository shiftRepository)
    {
        _shiftRepository = shiftRepository;
    }

    [HttpPost]
    public async Task<ActionResult<ShiftDTO>> AddShift([FromBody] ShiftDTOWithoutId request)
    {
        try
        {
            Shift shift = await _shiftRepository.AddAsync(ShiftGrpcRepository.EntityShiftWithoutIdToEntityShift(request));

            var simpleDto = new ShiftDTO
            {
                Description = shift.Description,
                TypeOfShift = shift.TypeOfShift,
                ShiftStatus = shift.ShiftStatus,
                Id = shift.Id,
                StartDateTime = shift.StartDateTime,
                EndDateTime = shift.EndDateTime,
                Location = shift.Location
            };
            return Ok(simpleDto);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpPost("/shift/{shiftId}/assign/{employeeId}")]
    public async Task<ActionResult> AssignEmployeeToShift([FromRoute] long shiftId, [FromRoute] long employeeId)
    {
        try
        {
            await _shiftRepository.AssignEmployeeToShift(shiftId, employeeId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpPost("/shift/{shiftId}/unassign/{employeeId}")]
    public async Task<ActionResult> UnassignEmployeeFromShift([FromRoute] long shiftId, [FromRoute] long employeeId)
    {
        try
        {
            await _shiftRepository.UnassignEmployeeToShift(shiftId, employeeId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet("/shift/{id}")]
    public async Task<ActionResult<ShiftDTO>> GetSingleShift([FromRoute] long id)
    {
        try
        {
            Shift shift = await _shiftRepository.GetSingleAsync(id);
            
            var shiftDto = new ShiftDTO
            {
                Id = shift.Id,
                Description = shift.Description,
                TypeOfShift = shift.TypeOfShift,
                ShiftStatus = shift.ShiftStatus,
                StartDateTime = shift.StartDateTime,
                EndDateTime = shift.EndDateTime,
                Location = shift.Location
            };
            return Ok(shiftDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }


    [HttpGet("/Shift/GetAll")]
    public ActionResult<IEnumerable<ShiftDTO>> GetAllShifts()
    {
        IQueryable<Shift> shifts = _shiftRepository.GetManyAsync();
        
        var shiftDtos = shifts.Select(shift => new ShiftDTO
        {
            Id = shift.Id,
            Description = shift.Description,
            TypeOfShift = shift.TypeOfShift,
            ShiftStatus = shift.ShiftStatus,
            StartDateTime = shift.StartDateTime,
            EndDateTime = shift.EndDateTime,
            Location = shift.Location
        });

        return Ok(shiftDtos.ToList());
    }
    
    [HttpGet("/Employee/{id:int}/Shifts")]  
    public ActionResult<IEnumerable<Shift>> GetShiftsByEmployeeId([FromRoute] int id)
    {
        IQueryable<Shift> shifts = _shiftRepository.GetManyAsync();
        List<ShiftDTO> shiftDTOS = new List<ShiftDTO>();
        
        foreach (var shift in shifts)
        {
            if (shift.EmployeeId == long.CreateChecked(id))
            {
                shiftDTOS.Add(new ShiftDTO
                {
                    StartDateTime = shift.StartDateTime,
                    EndDateTime = shift.EndDateTime,
                    Description = shift.Description,
                    TypeOfShift = shift.TypeOfShift,
                    Id = shift.Id,
                    ShiftStatus = shift.ShiftStatus,
                    Location = shift.Location,
                    EmployeeId = shift.EmployeeId
                });
            }
        }
        
        
        return Ok(_shiftRepository);
    }



    [HttpPut("/Shift/{id:int}")]
    public async Task<ActionResult<ShiftDTO>> UpdateShiftByItsId([FromRoute] int id, [FromBody] ShiftDTO shiftDto)
    {
        try
        {
            Shift existingShift = await _shiftRepository.GetSingleAsync(long.CreateChecked(id));
            if (existingShift == null)
            {
                return NotFound($"Shift with ID {id} not found");
            }

            existingShift.StartDateTime = shiftDto.StartDateTime;
            existingShift.EndDateTime = shiftDto.EndDateTime;
            existingShift.TypeOfShift = shiftDto.TypeOfShift;
            existingShift.ShiftStatus = shiftDto.ShiftStatus;
            existingShift.Description = shiftDto.Description;
            existingShift.Location = shiftDto.Location;
            existingShift.EmployeeId = shiftDto.EmployeeId;

            Shift updatedShift = await _shiftRepository.UpdateAsync(existingShift);

            var updatedDto = new ShiftDTO
            {
                Id = updatedShift.Id,
                Description = updatedShift.Description,
                TypeOfShift = updatedShift.TypeOfShift,
                ShiftStatus = updatedShift.ShiftStatus,
                StartDateTime = updatedShift.StartDateTime,
                EndDateTime = updatedShift.EndDateTime,
                Location = updatedShift.Location
            };

            return Ok(updatedDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteShift([FromRoute] long id)
    {
        try
        {
            await _shiftRepository.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}
