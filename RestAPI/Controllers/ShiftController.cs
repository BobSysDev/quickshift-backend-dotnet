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
    public async Task<ActionResult<ShiftDTOWithoutId>> AddShift([FromBody] ShiftDTOWithoutId request)
    {
        try
        {
            var grpcRepo = new GrpcRepo();
            ShiftDTO shiftDto = await grpcRepo.CreateShift(request);

            var shift = new Shift
            {
                Id = shiftDto.Id,
                StartDateTime = shiftDto.StartDateTime,
                EndDateTime = shiftDto.EndDateTime,
                TypeOfShift = shiftDto.TypeOfShift,
                ShiftStatus = shiftDto.ShiftStatus,
                Description = shiftDto.Description,
                Location = shiftDto.Location
            };

            await _shiftRepository.AddAsync(shift);

            var simpleShiftDto = new ShiftDTOWithoutId
            {
                StartDateTime = shiftDto.StartDateTime,
                EndDateTime = shiftDto.EndDateTime,
                TypeOfShift = shiftDto.TypeOfShift,
                ShiftStatus = shiftDto.ShiftStatus,
                Description = shiftDto.Description,
                Location = shiftDto.Location
            };

            return Ok(simpleShiftDto);
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