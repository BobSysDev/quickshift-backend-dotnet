using DTOs.Shift;
using DTOs.ShiftSwitching;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using RepositoryProxies;

namespace RestAPI.Controllers;

[ApiController]
[Route("ShiftSwitching/[controller]")]

public class RequestController
{
    private readonly IShiftSwitchRepository _shiftSwitchRepository;
    
    public RequestController(IShiftSwitchRepository shiftSwitchRepository)
    {
        _shiftSwitchRepository = shiftSwitchRepository;
    }
    
    [HttpPost]
    public async Task<ShiftSwitchRequestDTO> AddSwitchRequest([FromBody] NewShiftSwitchRequestDTO dto)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id:long}")]
    public async Task<ShiftSwitchRequestDTO> GetSingleShiftRequestById([FromRoute] long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet]
    public async Task<List<ShiftSwitchRequestDTO>> GetAllShiftRequests()
    {
        throw new NotImplementedException();
    }

    [HttpGet("/Shift/{shiftId:long}/ShiftSwitching/[controller]")]
    public async Task<List<ShiftSwitchRequestDTO>> GetAllShiftRequestsByShiftId([FromRoute] long shiftId)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("/Employee/{employeeId:long}/ShiftSwitching/[controller]")]
    public async Task<List<ShiftSwitchRequestDTO>> GetAllShiftRequestsByEmployeeId([FromRoute] long employeeId)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id:long}")]
    public async Task<ShiftSwitchRequestDTO> UpdateShiftRequest([FromRoute] long id, [FromBody] UpdateShiftSwitchRequestDTO dto)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id:long}")]
    public async Task DeleteShiftRequest([FromRoute] long id)
    {
        throw new NotImplementedException();
    }
}