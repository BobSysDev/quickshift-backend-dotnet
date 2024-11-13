using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using DTOs.Shift;
using Entities;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace RestAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ShiftController
{
    private readonly IShiftRepository _shiftRepository;

    public ShiftController(IShiftRepository shiftRepository)
    {
        _shiftRepository = shiftRepository;
    }

    [HttpPost]
    public async Task<IResult> CreateShift([FromBody] ShiftDTO shiftDto)
    {
        if (string.IsNullOrEmpty(shiftDto.StartDateTime.ToString("MM/dd/yyyy")))
        {
            return Results.BadRequest("Date is required");
        }

        Shift newShift = new Shift()
        {
            Id = shiftDto.Id,
            StartDateTime = shiftDto.StartDateTime,
            EndDateTime = shiftDto.EndDateTime,
            TypeOfShift = shiftDto.TypeOfShift,
            ShiftStatus = shiftDto.ShiftStatus,
            Description = shiftDto.Description,
            Location = shiftDto.Location
        };

        Shift createdShift = await _shiftRepository.AddAsync(newShift);
        
        return Results.Created($"/shift/{createdShift.Id}", createdShift);
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetSingleShift([FromRoute] long id)
    {
        try
        {
            Shift shift = await _shiftRepository.GetSingleAsync(id);
            return Results.Ok(shift);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.NotFound(e.Message);
        }
    }
    
    [HttpGet]

    public async Task<IResult> GetAllShifts([FromQuery] long? id)
    {
        IQueryable<Shift> shifts = _shiftRepository.GetManyAsync();

        if (id is not null)
        {
            shifts = shifts.Where(x => x.Id == id);
        }
        
        return Results.Ok(shifts);
    }

    [HttpDelete("{id}")]

    public async Task<IResult> DeleteShift([FromRoute] long id)
    {
        await _shiftRepository.DeleteAsync(id);
        return Results.Ok();
    }
}