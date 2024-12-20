﻿using DTOs;
using DTOs.Shift;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RestAPI.Controllers.ShiftSwitching;

[ApiController]
[Route("/ShiftSwitching/Request/{requestId:long}/[controller]")]

public class TimeframeController : ControllerBase
{
    private readonly IShiftSwitchRepository _shiftSwitchRepository;
    private readonly IShiftRepository _shiftRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public TimeframeController(IShiftSwitchRepository shiftSwitchRepository, IShiftRepository shiftRepository, IEmployeeRepository employeeRepository)
    {
        _shiftSwitchRepository = shiftSwitchRepository;
        _shiftRepository = shiftRepository;
        _employeeRepository = employeeRepository;
    }
    
    [HttpPost]
    public async Task<ActionResult<ShiftSwitchRequestTimeframeDTO>> AddSwitchRequestTimeframe([FromRoute] long requestId, [FromBody] NewShiftSwitchRequestTimeframeDTO dto)
    {
        try
        {
            ShiftSwitchRequestTimeframe timeframeToAdd = EntityDtoConverter
                .NewShiftSwitchRequestTimeframeDtoToShiftSwitchRequestTimeframe(dto);
            var addedTimeframe = await _shiftSwitchRepository.AddShiftSwitchRequestTimeframeAsync(timeframeToAdd, requestId);
            return Ok(EntityDtoConverter.ShiftSwitchRequestTimeframeToShiftSwitchRequestTimeframeDto(addedTimeframe, requestId));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("/ShiftSwitching/Timeframe/{id:long}")]
    public async Task<ActionResult<ShiftSwitchRequestTimeframeDTO>> GetSwitchRequestTimeframeById(long id)
    {
        try
        {
            var timeframe = await _shiftSwitchRepository.GetShiftSwitchRequestTimeframeSingleAsync(id);
            var requestId = await _shiftSwitchRepository.GetShiftSwitchRequestIdByShiftSwitchRequestTimeframeId(id);
            return Ok(EntityDtoConverter.ShiftSwitchRequestTimeframeToShiftSwitchRequestTimeframeDto(timeframe, requestId));
        }

        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ShiftSwitchRequestTimeframeDTO>>> GetSwitchRequestTimeframeByReqeustId([FromRoute] long requestId)
    {
        try
        {
            var timeframes = await _shiftSwitchRepository.GetManyShiftSwitchRequestTimeframesByRequestIdAsync(requestId);
            var dtos = new List<ShiftSwitchRequestTimeframeDTO>();
            timeframes.ForEach(async timeframe =>
            {
                var requestId =
                    await _shiftSwitchRepository.GetShiftSwitchRequestIdByShiftSwitchRequestTimeframeId(timeframe.Id);
                dtos.Add(EntityDtoConverter.ShiftSwitchRequestTimeframeToShiftSwitchRequestTimeframeDto(timeframe,
                    requestId));
            });
            
            return Ok(timeframes);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("/ShiftSwitching/Timeframe/{id:long}")]
    public async Task<ActionResult> DeleteSwitchRequestTimeframe([FromRoute] long id)
    {
        try
        {
            await _shiftSwitchRepository.DeleteShiftSwitchRequestTimeframeAsync(id);
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }
}