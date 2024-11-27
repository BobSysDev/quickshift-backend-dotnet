using DTOs.Shift;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using GrpcClient;
using Microsoft.Extensions.Logging;
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
    public async Task<ActionResult<Shift>> GetSingleShift([FromRoute] long id)
    {
        try
        {
            Shift shift = await _shiftRepository.GetSingleAsync(id);
            return Ok(shift);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }

    [HttpGet("/Shift/GetAll")]  
    public ActionResult<IEnumerable<Shift>> GetAllShifts()
    {
        IQueryable<Shift> shifts = _shiftRepository.GetManyAsync();
        return Ok(shifts.ToList());
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
        
        // foreach (var shift in shifts)
        // {
        //     if (!shift.AssignedEmployees.Any())
        //     {
        //         continue;
        //     }
        //     if (shift.AssignedEmployees.Contains(long.CreateChecked(id)))
        //     {
        //         shiftDTOS.Add(new ShiftDTO
        //         {
        //             StartDateTime = shift.StartDateTime,
        //             EndDateTime = shift.EndDateTime,
        //             Description = shift.Description,
        //             TypeOfShift = shift.TypeOfShift,
        //             Id = shift.Id,
        //             ShiftStatus = shift.ShiftStatus,
        //             Location = shift.Location
        //         });
        //     }
        // }
        
        return Ok(_shiftRepository);
    }


    [HttpPut("/Shift/{id:int}")]
    public async Task<ActionResult<Shift>> UpdateShiftByItsId([FromRoute] int id, [FromBody] ShiftDTO shiftDto)
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

            await _shiftRepository.UpdateAsync(existingShift);
            return Ok(existingShift);
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

//TODO:change update to display in REST API ID everything
//TODO:make so getallshifts get all shifts from data base
//TODO:fix any problems that appear during testing

//TODO: create shift works --- shift/{id} works  ---   ---   ---   ---   ---