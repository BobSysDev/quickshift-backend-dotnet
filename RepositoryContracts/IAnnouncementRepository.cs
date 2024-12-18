using Entities;

namespace RepositoryContracts;

public interface IAnnouncementRepository
{
    public Task<Announcement> AddNewAnnouncementAsync(Announcement newAnnouncement);
    public Task<Announcement> UpdateAnnouncementAsync(Announcement announcement);
    public Task<Announcement> GetSingleAnnouncementByIdAsync(long id);
    public Task<List<Announcement>> GetALlAnnouncementsAsync();
    public Task<List<Announcement>> GetMostRecentAnnouncementsAsync(int amount);
    public Task DeleteSingleAnnouncement(long id);
}