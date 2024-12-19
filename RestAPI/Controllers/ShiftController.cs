using System.Text.Json.Serialization;
using DTOs;
using DTOs.Shift;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using GrpcClient;
using InMemoryRepositories;
using NewShiftDTO = DTOs.Shift.NewShiftDTO;
using Shift = Entities.Shift;
using ShiftDTO = DTOs.Shift.ShiftDTO;

namespace RestAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ShiftController : ControllerBase
{
    private readonly IShiftRepository _shiftRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public ShiftController(IShiftRepository shiftRepository, IEmployeeRepository employeeRepository)
    {
        _shiftRepository = shiftRepository;
        _employeeRepository = employeeRepository;
    }

    [HttpPost]
    public async Task<ActionResult<ShiftDTO>> AddShift([FromBody] NewShiftDTO request)
    {
            Shift tmp = EntityDtoConverter.NewShiftDtoToShift(request);
            //Console.WriteLine(tmp.Print());
            var shift = await _shiftRepository.AddAsync(tmp);
            //Console.WriteLine(shift.Print());
            ShiftDTO dto = EntityDtoConverter.ShiftToShiftDto(shift);
            return Ok(dto);
        
    }

    [HttpPatch("/Shift/{shiftId:int}/Assign/{employeeId:int}")]
    public async Task<ActionResult> AssignEmployeeToShift([FromRoute] int shiftId, [FromRoute] int employeeId)
    {
        try
        {
            await _shiftRepository.AssignEmployeeToShift(long.CreateChecked(shiftId), long.CreateChecked(employeeId));
            await ShiftUtilityMethods.UpdateEmployeeAfterChangingShifts(employeeId, _shiftRepository, _employeeRepository);
            return Ok();
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPatch("/Shift/{shiftId}/Unassign/{employeeId}")]
    public async Task<ActionResult> UnassignEmployeeFromShift([FromRoute] long shiftId, [FromRoute] long employeeId)
    {
        try
        {
            await _shiftRepository.UnassignEmployeeToShift(shiftId, employeeId);
            await ShiftUtilityMethods.UpdateEmployeeAfterChangingShifts(employeeId, _shiftRepository, _employeeRepository);
            return Ok();
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ShiftDTO>> GetSingleShift([FromRoute] int id)
    {
        try
        {
            var shift = await _shiftRepository.GetSingleAsync(long.CreateChecked(id));

            ShiftDTO shiftDto = EntityDtoConverter.ShiftToShiftDto(shift);
            return Ok(shiftDto);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<ShiftDTO>> GetAllShifts()
    {
        var shifts = _shiftRepository.GetManyAsync().ToList();

        //var shiftDtos = shifts.Select(shift => new ShiftDTO = EntityDtoConverter.ShiftToShiftDto());
        var shiftDtos = EntityDtoConverter.ListShiftToListShiftDtos(shifts);
        
        return Ok(shiftDtos);
    }

    [HttpGet("/Shifts/Employee/{id:int}")]
    public async Task<ActionResult<IEnumerable<ShiftDTO>>> GetShiftsByEmployeeId([FromRoute] int id)
    {
        var shifts =  _shiftRepository.GetManyAsync().ToList();
        var shiftDtos = new List<ShiftDTO>();
        foreach (var shift in shifts)
        {
            if (shift.AssingnedEmployees.Contains(id))
            {
                shiftDtos.Add(EntityDtoConverter.ShiftToShiftDto(shift));
            }
        }

        return Ok(shiftDtos);
    }

    [HttpPut("/Shift/{id:int}")]
    public async Task<ActionResult<ShiftDTO>> UpdateShiftByItsId([FromRoute] int id, [FromBody] NewShiftDTO newShiftDto)
    {
        try
        {
            var existingShift = await _shiftRepository.GetSingleAsync(long.CreateChecked(id));
            if (existingShift == null)
            {
                return NotFound($"Shift with ID {id} not found");
            }

            existingShift.StartDateTime = newShiftDto.StartDateTime;
            existingShift.EndDateTime = newShiftDto.EndDateTime;
            existingShift.TypeOfShift = newShiftDto.TypeOfShift;
            existingShift.ShiftStatus = newShiftDto.ShiftStatus;
            existingShift.Description = newShiftDto.Description;
            existingShift.Location = newShiftDto.Location;

            var updatedShift = await _shiftRepository.UpdateAsync(existingShift);

            var updatedDto = new ShiftDTO
            {
                Id = updatedShift.Id,
                Description = updatedShift.Description,
                TypeOfShift = updatedShift.TypeOfShift,
                ShiftStatus = updatedShift.ShiftStatus,
                StartDateTime = updatedShift.StartDateTime,
                EndDateTime = updatedShift.EndDateTime,
                Location = updatedShift.Location,
                AssignedEmployees = updatedShift.AssingnedEmployees
            };

            return Ok(updatedDto);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
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
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }
}