using DTOs.Shift;
using DTOs.ShiftSwitching;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RestAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class RequestController
{
    private readonly IShiftSwitchRequestRepository _requestRepository;
    public RequestController(IShiftSwitchRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }
    
    [HttpPost("/ShiftSwitching/[controller]")]
    public async Task<ShiftSwitchRequestDTO> AddSwitchRequest([FromBody] NewShiftSwitchRequestDTO dto)
    {
        throw new NotImplementedException();
    }

    [HttpGet("/ShiftSwitching/[controller]/{id:long}")]
    public async Task<ShiftSwitchRequestDTO> GetSingleShiftRequestById([FromRoute] long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("/ShiftSwitching/[controller]")]
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

    [HttpPut("/ShiftSwitching/[controller]/{id:long}")]
    public async Task<ShiftSwitchRequestDTO> UpdateShiftRequest([FromRoute] long id, [FromBody] UpdateShiftSwitchRequestDTO dto)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("/ShiftSwitching/[controller]/{id:long}")]
    public async Task DeleteShiftRequest([FromRoute] long id)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("/ShiftSwitching/[controller]/{id:long}/Reply/{replyId:long}/Resolve")]
    public async Task<List<ShiftDTO>> ResolveRequest([FromRoute] long id, [FromRoute] long replyId)
    {
        throw new NotImplementedException();
    }
}