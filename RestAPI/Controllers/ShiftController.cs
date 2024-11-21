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

    [HttpGet("{id}")]
    public async Task<ActionResult<Shift>> GetSingleShift([FromRoute] long id)
    {
        Console.WriteLine($"Requested ID: {id}");
        try
        {
            Shift shift = await _shiftRepository.GetSingleAsync(id);
            return Ok(shift);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }

    [HttpGet("Employee")]
    public ActionResult<IEnumerable<Shift>> GetAllShifts([FromQuery] long? id)
    {
        IQueryable<Shift> shifts = _shiftRepository.GetManyAsync();

        if (id is not null)
        {
            shifts = shifts.Where(x => x.Id == id);
        }

        return Ok(shifts.ToList());
    }


    [HttpPut("Update-Shift-By-Id")]
    public async Task<ActionResult<Shift>> UpdateShiftOfEmployee([FromQuery] long id, [FromBody] ShiftDTO shiftDto)
    {
        try
        {
            Shift existingShift = await _shiftRepository.GetSingleAsync(id);
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