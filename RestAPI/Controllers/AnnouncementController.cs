using DTOs;
using DTOs.Announcements;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;


namespace RestAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class AnnouncementController : ControllerBase
{
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public AnnouncementController(IAnnouncementRepository announcementRepository, IEmployeeRepository employeeRepository)
    {
        _announcementRepository = announcementRepository;
        _employeeRepository = employeeRepository;
    }

    [HttpPost]
    public async Task<ActionResult<AnnouncementDTO>> PostSingleAnnouncementAsync([FromBody] NewAnnouncementDTO dto)
    {
        try
        {
            Announcement announcement =
                await _announcementRepository.AddNewAnnouncementAsync(
                    EntityDtoConverter.NewAnnouncementDtoToEntity(dto));
            return Ok(announcement);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("Top/{count:int}")]
    public async Task<ActionResult<List<AnnouncementDTO>>> GetRecentAnnouncements([FromRoute] int count)
    {
        try
        {
            List<Announcement> announcements = await _announcementRepository.GetMostRecentAnnouncementsAsync(count);
            List<AnnouncementDTO> announcementDtos = new List<AnnouncementDTO>();
            announcements.ForEach(async a => announcementDtos.Add(await EntityDtoConverter.AnnouncementEntityToDto(a, _employeeRepository)));
            return Ok(announcementDtos);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<List<AnnouncementDTO>>> GetAll()
    {
        try
        {
            List<Announcement> announcements = await _announcementRepository.GetALlAnnouncementsAsync();
            List<AnnouncementDTO> announcementDtos = new List<AnnouncementDTO>();
            announcements.ForEach(async a => announcementDtos.Add(await EntityDtoConverter.AnnouncementEntityToDto(a, _employeeRepository)));
            return Ok(announcementDtos);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteSingleAnnouncement([FromRoute] long id)
    {
        try
        {
            await _announcementRepository.DeleteSingleAnnouncement(id);
            return Ok();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
}