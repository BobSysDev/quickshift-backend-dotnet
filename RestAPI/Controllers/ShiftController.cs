﻿using DTOs.Shift;
using Grpc.Core;
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
            var shift = await _shiftRepository.AddAsync(ShiftGrpcRepository.EntityShiftWithoutIdToEntityShift(request));

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

    [HttpPatch("/Shift/{shiftId:int}/Assign/{employeeId:int}")]
    public async Task<ActionResult> AssignEmployeeToShift([FromRoute] int shiftId, [FromRoute] int employeeId)
    {
        try
        {
            await _shiftRepository.AssignEmployeeToShift(long.CreateChecked(shiftId), long.CreateChecked(employeeId));
            return Ok();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("/Shift/{shiftId}/Unassign/{employeeId}")]
    public async Task<ActionResult> UnassignEmployeeFromShift([FromRoute] long shiftId, [FromRoute] long employeeId)
    {
        try
        {
            await _shiftRepository.UnassignEmployeeToShift(shiftId, employeeId);
            return Ok();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ShiftDTO>> GetSingleShift([FromRoute] int id)
    {
        try
        {
            var shift = await _shiftRepository.GetSingleAsync(long.CreateChecked(id));

            var shiftDto = new ShiftDTO
            {
                Description = shift.Description,
                TypeOfShift = shift.TypeOfShift,
                ShiftStatus = shift.ShiftStatus,
                StartDateTime = shift.StartDateTime,
                EndDateTime = shift.EndDateTime,
                Location = shift.Location
            };
            return Ok(shiftDto);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<ShiftDTO>> GetAllShifts()
    {
        var shifts = _shiftRepository.GetManyAsync();

        var shiftDtos = shifts.Select(shift => new ShiftDTO
        {
            Id = shift.Id,
            Description = shift.Description,
            TypeOfShift = shift.TypeOfShift,
            ShiftStatus = shift.ShiftStatus,
            StartDateTime = shift.StartDateTime,
            EndDateTime = shift.EndDateTime,
            Location = shift.Location,
            AssignedEmployees = shift.AssingnedEmployees
            //EmployeeId = shift.EmployeeId == -1 ? null : shift.EmployeeId
        });
        
        
        return Ok(shiftDtos.ToList());
    }

    [HttpGet("/Shifts/Employee/{id:int}")]
    public async Task<ActionResult<IEnumerable<ShiftDTO>>> GetShiftsByEmployeeId([FromRoute] int id)
    {
        var shifts =  _shiftRepository.GetManyAsync();
        var shiftDtos = new List<ShiftDTO>();

        foreach (var shift in shifts)
        {
            if (shift.AssingnedEmployees.Contains(id))
            {
                shiftDtos.Add(new ShiftDTO
                {
                    StartDateTime = shift.StartDateTime,
                    EndDateTime = shift.EndDateTime,
                    Description = shift.Description,
                    TypeOfShift = shift.TypeOfShift,
                    Id = shift.Id,
                    ShiftStatus = shift.ShiftStatus,
                    Location = shift.Location,
                    AssignedEmployees = shift.AssingnedEmployees
                });
            }
        }

        return Ok(shiftDtos);
    }

    [HttpPut("/Shift/{id:int}")]
    public async Task<ActionResult<ShiftDTO>> UpdateShiftByItsId([FromRoute] int id, [FromBody] ShiftDTOWithoutId shiftDto)
    {
        try
        {
            var existingShift = await _shiftRepository.GetSingleAsync(long.CreateChecked(id));
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
        catch (InvalidOperationException e)
        {
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
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
}