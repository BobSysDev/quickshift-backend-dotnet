using DTOs;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RestAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class ReplyController
{
    private readonly IShiftSwitchReplyRepository _replyRepository;

    public ReplyController(IShiftSwitchReplyRepository replyRepository)
    {
        _replyRepository = replyRepository;
    }

    [HttpPost("/ShiftSwitching/Request/{requestId:long}/[controller]")]
    public async Task<ShiftSwitchReplyDTO> AddSwitchReply([FromRoute] long requestId, [FromBody] NewShiftSwitchReplyDTO dto)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("/ShiftSwitching/[controller]/{id:long}")]
    public async Task<ShiftSwitchReplyDTO> GetSingleSwitchReplyById([FromRoute] long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("/ShiftSwitching/[controller]/")]
    public async Task<ShiftSwitchReplyDTO> GetAllSwitchReplies()
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("/ShiftSwitching/Request/{requestId:long}/[controller]")]
    public async Task<ShiftSwitchReplyDTO> GetAllSwitchRepliesByRequestId([FromRoute] long requestId)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("/Employee/{employeeId:long}/ShiftSwitching/[controller]")]
    public async Task<ShiftSwitchReplyDTO> GetAllSwitchRepliesByEmployeeId([FromRoute] long employeeId)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("/ShiftSwitching/Request/{requestId:long}/[controller]/{id:long}/TargetAccept")]
    public async Task<ShiftSwitchReplyDTO> TargetAcceptSwitchReply([FromRoute] long requestId, [FromRoute] long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("/ShiftSwitching/Request/{requestId:long}/[controller]/{id:long}/TargetAccept")]
    public async Task<ShiftSwitchReplyDTO> TargetRemoveAcceptSwitchReply([FromRoute] long requestId, [FromRoute] long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("/ShiftSwitching/Request/{requestId:long}/[controller]/{id:long}/OriginAccept")]
    public async Task<ShiftSwitchReplyDTO> OriginAcceptSwitchReply([FromRoute] long requestId, [FromRoute] long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("/ShiftSwitching/Request/{requestId:long}/[controller]/{id:long}/OriginAccept")]
    public async Task<ShiftSwitchReplyDTO> OriginRemoveAcceptSwitchReply([FromRoute] long requestId, [FromRoute] long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpPut("/ShiftSwitching/[controller]/{id:long}")]
    public async Task<ShiftSwitchReplyDTO> UpdateSingleSwitchReply([FromRoute] long id, [FromBody] UpdateShiftSwitchReplyDTO dto)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("/ShiftSwitching/[controller]/{id:long}")]
    public async Task DeleteSingleSwitchReplyById([FromRoute] long id)
    {
        throw new NotImplementedException();
    }
}