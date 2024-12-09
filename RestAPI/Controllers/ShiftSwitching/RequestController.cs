﻿using DTOs;
using DTOs.Shift;
using DTOs.ShiftSwitching;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RestAPI.Controllers.ShiftSwitching;

[ApiController]
[Route("ShiftSwitching/[controller]")]

public class RequestController : ControllerBase
{
    private readonly IShiftSwitchRepository _shiftSwitchRepository;
    private readonly IShiftRepository _shiftRepository;
    private readonly IEmployeeRepository _employeeRepository;
    
    public RequestController(IShiftSwitchRepository shiftSwitchRepository, IShiftRepository shiftRepository, IEmployeeRepository employeeRepository)
    {
        _shiftSwitchRepository = shiftSwitchRepository;
        _shiftRepository = shiftRepository;
        _employeeRepository = employeeRepository;
    }
    
    [HttpPost]
    public async Task<ActionResult<ShiftSwitchRequestDTO>> AddSwitchRequest([FromBody] NewShiftSwitchRequestDTO dto)
    {
        try
        {
            ShiftSwitchRequest requestToAdd = EntityDtoConverter
                .NewShiftSwitchRequestDtoToShiftSwitchRequest(dto, _shiftRepository, _employeeRepository);
            var addedRequest = await _shiftSwitchRepository.AddShiftSwitchRequestAsync(requestToAdd);
            return Ok(EntityDtoConverter.ShiftSwitchRequestToShiftSwitchRequestDto(addedRequest));
        }
        catch
        {
            throw new NotImplementedException(); //TODO: ERROR CATCHING
        }
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ShiftSwitchRequestDTO>> GetSingleShiftRequestById([FromRoute] long id)
    {
        try
        {
            ShiftSwitchRequest retrievedRequest = await _shiftSwitchRepository.GetSingleShiftSwitchRequestAsync(id);
            return Ok(EntityDtoConverter.ShiftSwitchRequestToShiftSwitchRequestDto(retrievedRequest));
        }
        catch
        {
            throw new NotImplementedException(); //TODO: ERROR CATCHING
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ShiftSwitchRequestDTO>>> GetAllShiftRequests()
    {
        try
        {
            List<ShiftSwitchRequest> retrievedRequests = _shiftSwitchRepository.GetManyShiftSwitchRequestAsync().ToList();
            List<ShiftSwitchRequestDTO> requestDtos = new List<ShiftSwitchRequestDTO>();
            retrievedRequests.ForEach(request => requestDtos.Add(EntityDtoConverter.ShiftSwitchRequestToShiftSwitchRequestDto(request)));
            return Ok(requestDtos);
        }
        catch
        {
            throw new NotImplementedException(); //TODO: ERROR CATCHING
        }
    }

    [HttpGet("/Shift/{shiftId:long}/ShiftSwitching/[controller]")]
    public async Task<ActionResult<List<ShiftSwitchRequestDTO>>> GetAllShiftRequestsByShiftId([FromRoute] long shiftId)
    {
        try
        {
            List<ShiftSwitchRequest> retrievedRequests = await _shiftSwitchRepository.GetManyShiftSwitchRequestsByShiftIdAsync(shiftId);
            List<ShiftSwitchRequestDTO> requestDtos = new List<ShiftSwitchRequestDTO>();
            retrievedRequests.ForEach(request => requestDtos.Add(EntityDtoConverter.ShiftSwitchRequestToShiftSwitchRequestDto(request)));
            return Ok(requestDtos);
        }
        catch
        {
            throw new NotImplementedException(); //TODO: ERROR CATCHING
        }
    }
    
    [HttpGet("/Employee/{employeeId:long}/ShiftSwitching/[controller]")]
    public async Task<ActionResult<List<ShiftSwitchRequestDTO>>> GetAllShiftRequestsByEmployeeId([FromRoute] long employeeId)
    {
        try
        {
            List<ShiftSwitchRequest> retrievedRequests = await _shiftSwitchRepository.GetManyShiftSwitchRequestsByEmployeeIdAsync(employeeId);
            List<ShiftSwitchRequestDTO> requestDtos = new List<ShiftSwitchRequestDTO>();
            retrievedRequests.ForEach(request => requestDtos.Add(EntityDtoConverter.ShiftSwitchRequestToShiftSwitchRequestDto(request)));
            return Ok(requestDtos);
        }
        catch
        {
            throw new NotImplementedException(); //TODO: ERROR CATCHING
        }
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ShiftSwitchRequestDTO>> UpdateShiftRequest([FromRoute] long id, [FromBody] UpdateShiftSwitchRequestDTO dto)
    {
        try
        {
            ShiftSwitchRequest requestToUpdate = await _shiftSwitchRepository.GetSingleShiftSwitchRequestAsync(id);
            requestToUpdate.Details = dto.Details ?? "";
            var updatedRequest = await _shiftSwitchRepository.UpdateShiftSwitchRequestAsync(requestToUpdate);
            return Ok(EntityDtoConverter.ShiftSwitchRequestToShiftSwitchRequestDto(updatedRequest));
        }
        catch
        {
            throw new NotImplementedException(); //TODO: ERROR CATCHING
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteShiftRequest([FromRoute] long id)
    {
        try
        {
            await _shiftSwitchRepository.DeleteShiftSwitchRequestAsync(id);
            return Ok();
        }
        catch
        {
            throw new NotImplementedException(); //TODO: ERROR CATCHING
        }
    }
}