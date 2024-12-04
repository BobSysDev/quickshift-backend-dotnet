using DTOs;
using DTOs.Shift;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RestAPI.Controllers;

[ApiController]
[Route("ShiftSwitching/Request/{requestId:long}/[controller]")]

public class ReplyController
{
    private readonly IShiftSwitchReplyRepository _replyRepository;

    public ReplyController(IShiftSwitchReplyRepository replyRepository)
    {
        _replyRepository = replyRepository;
    }

    [HttpPost]
    public async Task<ShiftSwitchReplyDTO> AddSwitchReply([FromRoute] long requestId, [FromBody] NewShiftSwitchReplyDTO dto)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("/ShiftSwitching/Reply/{id:long}")]
    public async Task<ShiftSwitchReplyDTO> GetSingleSwitchReplyById([FromRoute] long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet]
    public async Task<List<ShiftSwitchReplyDTO>> GetAllSwitchRepliesByRequestId([FromRoute] long requestId)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("/Employee/{employeeId:long}/ShiftSwitching/[controller]")]
    public async Task<List<ShiftSwitchReplyDTO>> GetAllSwitchRepliesByEmployeeId([FromRoute] long employeeId)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("{id:long}/TargetAccept")]
    public async Task<ShiftSwitchReplyDTO> TargetAcceptSwitchReply([FromRoute] long requestId, [FromRoute] long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("{id:long}/TargetAccept")]
    public async Task<ShiftSwitchReplyDTO> TargetRemoveAcceptSwitchReply([FromRoute] long requestId, [FromRoute] long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("{id:long}/OriginAccept")]
    public async Task<ShiftSwitchReplyDTO> OriginAcceptSwitchReply([FromRoute] long requestId, [FromRoute] long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("{id:long}/OriginAccept")]
    public async Task<ShiftSwitchReplyDTO> OriginRemoveAcceptSwitchReply([FromRoute] long requestId, [FromRoute] long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpPut("/ShiftSwitching/Reply/{id:long}")]
    public async Task<ShiftSwitchReplyDTO> UpdateSingleSwitchReply([FromRoute] long id, [FromBody] UpdateShiftSwitchReplyDTO dto)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("/ShiftSwitching/Reply/{id:long}")]
    public async Task DeleteSingleSwitchReplyById([FromRoute] long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpPatch("{id:long}/Resolve")]
    public async Task<List<ShiftDTO>> ResolveRequest([FromRoute] long requestId, [FromRoute] long id)
    {
        throw new NotImplementedException();
    }
}