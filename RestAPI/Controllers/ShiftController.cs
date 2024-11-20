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
    public async Task<IResult> GetSingleShift([FromRoute] long id)
    {
        Console.WriteLine($"Requested ID: {id}");
        try
        {
            Shift shift = await _shiftRepository.GetSingleAsync(id);
            return Results.Ok(shift);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return Results.NotFound(e.Message);
        }
    }

    [HttpGet("Employee")]
    public async Task<IResult> GetAllShifts([FromQuery] long? id)
    {
        IQueryable<Shift> shifts = _shiftRepository.GetManyAsync();
        
        if (id is not null)
        {
            shifts = shifts.Where(x => x.Id == id);
        }
        
        return Results.Ok(shifts);
    }


    [HttpPut("Update-Shift-By-Id")]
    public async Task<IResult> UpdateShiftOfEmployee([FromQuery] long id, [FromBody] ShiftDTO shiftDto)
    {
        try
        {
            Shift existingShift = await _shiftRepository.GetSingleAsync(id);
            if (existingShift == null)
            {
                return Results.NotFound($"Shift with ID {id} not found");
            }

            existingShift.StartDateTime = shiftDto.StartDateTime;
            existingShift.EndDateTime = shiftDto.EndDateTime;
            existingShift.TypeOfShift = shiftDto.TypeOfShift;
            existingShift.ShiftStatus = shiftDto.ShiftStatus;
            existingShift.Description = shiftDto.Description;
            existingShift.Location = shiftDto.Location;

            await _shiftRepository.UpdateAsync(existingShift);
            return Results.Ok(existingShift);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IResult> DeleteShift([FromRoute] long id)
    {
        await _shiftRepository.DeleteAsync(id);
        return Results.Ok();
    }
}