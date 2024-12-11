using DTOs;
using DTOs.Shift;
using Entities;
using Grpc.Core;
using InMemoryRepositories;
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
            ShiftSwitchReply replyToAdd = await EntityDtoConverter
                .NewShiftSwitchReplyDtoToShiftSwitchReply(dto, _shiftRepository, _employeeRepository);

            var addedReply = await _shiftSwitchRepository.AddShiftSwitchReplyAsync(replyToAdd, requestId);
            return Ok(EntityDtoConverter.ShiftSwitchReplyToShiftSwitchReplyDto(addedReply, requestId));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
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
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
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
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
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
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
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
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);;
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
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);;
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);;
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
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);;
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);;
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
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);;
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);;
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
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);;
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);;
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
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);;
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);;
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

            var originShift = request.OriginShift;
            var targetShift = reply.TargetShift;
            var originEmployee = request.OriginEmployee;
            var targetEmployee = reply.TargetEmployee;

            if (originShift.AssingnedEmployees.Contains(targetEmployee.Id) ||
                targetShift.AssingnedEmployees.Contains(originEmployee.Id))
            {
                return Conflict("One of the shifts contains conflicting employees");
            }
            
            await _shiftRepository.UnassignEmployeeToShift(originShift.Id, originEmployee.Id);
            
            try
            {
                await _shiftRepository.AssignEmployeeToShift(targetShift.Id, originEmployee.Id);
            }
            catch (Exception e)
            {
                await _shiftRepository.AssignEmployeeToShift(originShift.Id, originEmployee.Id);
                return Conflict("Shift switching failed: step 2. Changes reverted");
            }

            try
            {
                await _shiftRepository.UnassignEmployeeToShift(targetShift.Id, targetEmployee.Id);
            }
            catch (Exception e)
            {
                await _shiftRepository.AssignEmployeeToShift(originShift.Id, originEmployee.Id);
                await _shiftRepository.UnassignEmployeeToShift(targetShift.Id, originEmployee.Id);
                return Conflict("Shift switching failed: step 3. Changes reverted");
            }
            
            try
            {
                await _shiftRepository.AssignEmployeeToShift(originShift.Id, targetEmployee.Id);
            }
            catch (Exception e)
            {
                await _shiftRepository.AssignEmployeeToShift(originShift.Id, originEmployee.Id);
                await _shiftRepository.UnassignEmployeeToShift(targetShift.Id, originEmployee.Id);
                await _shiftRepository.AssignEmployeeToShift(targetShift.Id, targetEmployee.Id);
                return Conflict("Shift switching failed: step 4. Changes reverted");
            }
            
            await ShiftUtilityMethods.UpdateEmployeeAfterChangingShifts(originEmployee.Id, _shiftRepository, _employeeRepository);
            await ShiftUtilityMethods.UpdateEmployeeAfterChangingShifts(targetEmployee.Id, _shiftRepository, _employeeRepository);
            
            var originShiftDto = EntityDtoConverter.ShiftToShiftDto(await _shiftRepository.GetSingleAsync(originShift.Id)); 
            var targetShiftDto = EntityDtoConverter.ShiftToShiftDto(await _shiftRepository.GetSingleAsync(targetShift.Id));

            await _shiftSwitchRepository.DeleteShiftSwitchRequestAsync(requestId);
            
            return Ok(new List<ShiftDTO>{originShiftDto, targetShiftDto});
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