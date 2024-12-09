using DTOs;
using DTOs.Shift;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RestAPI.Controllers.ShiftSwitching;

[ApiController]
[Route("ShiftSwitching/Request/{requestId:long}/[controller]")]
public class ReplyController : ControllerBase
{
    private readonly IShiftSwitchRepository _shiftSwitchRepository;
    private readonly IShiftRepository _shiftRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public ReplyController(IShiftSwitchRepository shiftSwitchRepository, IShiftRepository shiftRepository,
        IEmployeeRepository employeeRepository)
    {
        _shiftSwitchRepository = shiftSwitchRepository;
        _shiftRepository = shiftRepository;
        _employeeRepository = employeeRepository;
    }

    [HttpPost]
    public async Task<ActionResult<ShiftSwitchReplyDTO>> AddSwitchReply([FromRoute] long requestId,
        [FromBody] NewShiftSwitchReplyDTO dto)
    {
        try
        {
            ShiftSwitchReply replyToAdd = EntityDtoConverter
                .NewShiftSwitchReplyDtoToShiftSwitchReply(dto, _shiftRepository, _employeeRepository);

            var addedReply = await _shiftSwitchRepository.AddShiftSwitchReplyAsync(replyToAdd, requestId);
            return Ok(EntityDtoConverter.ShiftSwitchReplyToShiftSwitchReplyDto(addedReply, requestId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;  //TODO: ERROR CATCHING
        }
    }

    [HttpGet("/ShiftSwitching/Reply/{id:long}")]
    public async Task<ActionResult<ShiftSwitchReplyDTO>> GetSingleSwitchReplyById([FromRoute] long id)
    {
        try
        {
            ShiftSwitchReply retrievedReply = await _shiftSwitchRepository.GetSingleShiftSwitchReplyAsync(id);
            long requestId = await _shiftSwitchRepository.GetShiftSwitchRequestIdByShiftSwitchReplyId(id);
            return Ok(EntityDtoConverter.ShiftSwitchReplyToShiftSwitchReplyDto(retrievedReply, requestId));
        }
        catch
        {
            return null; //TODO: ERROR CATCHING
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<ShiftSwitchReplyDTO>>> GetAllSwitchRepliesByRequestId(
        [FromRoute] long requestId)
    {
        try
        {
            List<ShiftSwitchReply> retrievedReplies =
                await _shiftSwitchRepository.GetManyShiftSwitchRepliesByRequestIdAsync(requestId);
            List<ShiftSwitchReplyDTO> replyDtos = new List<ShiftSwitchReplyDTO>();
            retrievedReplies.ForEach(reply =>
                replyDtos.Add(EntityDtoConverter.ShiftSwitchReplyToShiftSwitchReplyDto(reply, requestId)));
            return Ok(replyDtos);
        }
        catch
        {
            return null;  //TODO: ERROR CATCHING
        }
    }

    [HttpGet("/Employee/{employeeId:long}/ShiftSwitching/[controller]")]
    public async Task<ActionResult<List<ShiftSwitchReplyDTO>>> GetAllSwitchRepliesByEmployeeId(
        [FromRoute] long employeeId)
    {
        try
        {
            List<ShiftSwitchReply> retrievedReplies =
                await _shiftSwitchRepository.GetManyShiftSwitchRepliesByTargetEmployeeAsync(employeeId);
            List<ShiftSwitchReplyDTO> replyDtos = new List<ShiftSwitchReplyDTO>();
            retrievedReplies.ForEach(async reply =>
            {
                long requestId = await _shiftSwitchRepository.GetShiftSwitchRequestIdByShiftSwitchReplyId(reply.Id);
                replyDtos.Add(EntityDtoConverter.ShiftSwitchReplyToShiftSwitchReplyDto(reply, requestId));
            });
            return Ok(replyDtos);
        }
        catch
        {
            return null;  //TODO: ERROR CATCHING
        }
    }

    [HttpPost("{id:long}/TargetAccept")]
    public async Task<ActionResult<ShiftSwitchReplyDTO>> TargetAcceptSwitchReply([FromRoute] long requestId,
        [FromRoute] long id)
    {
        try
        {
            await _shiftSwitchRepository.SetShiftSwitchReplyTargetAcceptedAsync(id, true);
            
            var reply = EntityDtoConverter.ShiftSwitchReplyToShiftSwitchReplyDto(
                await _shiftSwitchRepository.GetSingleShiftSwitchReplyAsync(id), requestId
            );
            return Ok();
        }
        catch
        {
            throw;  //TODO: ERROR CATCHING
        }
    }

    [HttpDelete("{id:long}/TargetAccept")]
    public async Task<ActionResult<ShiftSwitchReplyDTO>> TargetRemoveAcceptSwitchReply([FromRoute] long requestId,
        [FromRoute] long id)
    {
        try
        {
            await _shiftSwitchRepository.SetShiftSwitchReplyTargetAcceptedAsync(id, false);
            
            var reply = EntityDtoConverter.ShiftSwitchReplyToShiftSwitchReplyDto(
                await _shiftSwitchRepository.GetSingleShiftSwitchReplyAsync(id), requestId
            );
            return Ok();
        }
        catch
        {
            return null;  //TODO: ERROR CATCHING
        }
    }

    [HttpPost("{id:long}/OriginAccept")]
    public async Task<ActionResult<ShiftSwitchReplyDTO>> OriginAcceptSwitchReply([FromRoute] long requestId,
        [FromRoute] long id)
    {
        try
        {
            await _shiftSwitchRepository.SetShiftSwitchReplyOriginAcceptedAsync(id, true);
            
            var reply = EntityDtoConverter.ShiftSwitchReplyToShiftSwitchReplyDto(
                await _shiftSwitchRepository.GetSingleShiftSwitchReplyAsync(id), requestId
            );
            return Ok();
        }
        catch
        {
            return null;  //TODO: ERROR CATCHING
        }
    }

    [HttpDelete("{id:long}/OriginAccept")]
    public async Task<ActionResult<ShiftSwitchReplyDTO>> OriginRemoveAcceptSwitchReply([FromRoute] long requestId,
        [FromRoute] long id)
    {
        try
        {
            await _shiftSwitchRepository.SetShiftSwitchReplyOriginAcceptedAsync(id, false);
            
            var reply = EntityDtoConverter.ShiftSwitchReplyToShiftSwitchReplyDto(
                await _shiftSwitchRepository.GetSingleShiftSwitchReplyAsync(id), requestId
            );
            return Ok();
        }
        catch
        {
            return null;  //TODO: ERROR CATCHING
        }
    }

    [HttpPut("/ShiftSwitching/Reply/{id:long}")]
    public async Task<ActionResult<ShiftSwitchReplyDTO>> UpdateSingleSwitchReply([FromRoute] long id,
        [FromBody] UpdateShiftSwitchReplyDTO dto)
    {
        try
        {
            ShiftSwitchReply replyToUpdate = await _shiftSwitchRepository.GetSingleShiftSwitchReplyAsync(id);
            long requestId = await _shiftSwitchRepository.GetShiftSwitchRequestIdByShiftSwitchReplyId(id);
            replyToUpdate.Details = dto.Details ?? "";
            var updatedReply = await _shiftSwitchRepository.UpdateShiftSwitchReplyAsync(replyToUpdate);
            return Ok(EntityDtoConverter.ShiftSwitchReplyToShiftSwitchReplyDto(updatedReply, requestId));
        }
        catch
        {
            return null; //TODO: ERROR CATCHING
        }
    }

    [HttpDelete("/ShiftSwitching/Reply/{id:long}")]
    public async Task<ActionResult> DeleteSingleSwitchReplyById([FromRoute] long id)
    {
        try
        {
            await _shiftSwitchRepository.DeleteShiftSwitchReplyAsync(id);
            return Ok();
        }
        catch
        {
            return null;  //TODO: ERROR CATCHING
        }
    }

    [HttpPatch("{id:long}/Resolve")]
    public async Task<ActionResult<List<ShiftDTO>>> ResolveRequest([FromRoute] long requestId, [FromRoute] long id)
    {
        try
        {
            var request = await _shiftSwitchRepository.GetSingleShiftSwitchRequestAsync(requestId);
            var reply = await _shiftSwitchRepository.GetSingleShiftSwitchReplyAsync(id);

            if (!reply.OriginAccepted)
            {
                return Conflict("The origin employee needs to accept the switch first!");
            }
            if (!reply.OriginAccepted)
            {
                return Conflict("The target employee needs to accept the switch first!");
            }

            var originShiftId = request.OriginShift.Id;
            var targetShiftId = reply.TargetShift.Id;
            var originEmployeeId = request.OriginEmployee.Id;
            var targetEmployeeId = reply.TargetEmployee.Id;

            await _shiftRepository.UnassignEmployeeToShift(originShiftId, originEmployeeId);
            await _shiftRepository.AssignEmployeeToShift(targetShiftId, originEmployeeId);
            await _shiftRepository.UnassignEmployeeToShift(targetShiftId, targetEmployeeId);
            await _shiftRepository.AssignEmployeeToShift(originShiftId, targetEmployeeId);

            var originShiftDto = EntityDtoConverter.ShiftToShiftDto(await _shiftRepository.GetSingleAsync(originShiftId)); 
            var targetShiftDto = EntityDtoConverter.ShiftToShiftDto(await _shiftRepository.GetSingleAsync(targetShiftId));
            
            return Ok(new List<ShiftDTO>{originShiftDto, targetShiftDto});
        }
        catch
        {
            return null; //TODO: ERROR CATCHING
        }
    }
}