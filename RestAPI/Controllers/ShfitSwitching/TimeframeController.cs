using DTOs.Shift;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RestAPI.Controllers;

[ApiController]
[Route("/ShiftSwitching/Request/{requestId:long}/[controller]")]

public class TimeframeController
{
    private readonly IShiftSwitchRequestTimeframeRepository _timeframeRepository;

    public TimeframeController(IShiftSwitchRequestTimeframeRepository timeframeRepository)
    {
        _timeframeRepository = timeframeRepository;
    }
    
    [HttpPost]
    public async Task<ShiftSwitchRequestDTO> AddSwitchRequestTimeframe([FromRoute] long requestId, [FromBody] NewShiftSwitchRequestTimeframeDTO dto)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("/ShiftSwitching/Timeframe/{id:long}")]
    public async Task<ShiftSwitchRequestDTO> GetSwitchRequestTimeframeById(long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet]
    public async Task<List<ShiftSwitchRequestDTO>> GetSwitchRequestTimeframeByReqeustId([FromRoute] long requestId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("/ShiftSwitching/Timeframe/{id:long}")]
    public async Task DeleteSwitchRequestTimeframe([FromRoute] long id)
    {
        throw new NotImplementedException();
    }
}