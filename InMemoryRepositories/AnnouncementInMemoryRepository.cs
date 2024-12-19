using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class AnnouncementInMemoryRepository : IAnnouncementRepository
{
    private List<Announcement> _anonuncements = new List<Announcement>();
    
    public async Task<Announcement> AddNewAnnouncementAsync(Announcement newAnnouncement)
    {
        _anonuncements.Add(newAnnouncement);
        return newAnnouncement;
    }

    public async Task<Announcement> UpdateAnnouncementAsync(Announcement announcement)
    {
        _anonuncements.Remove(await GetSingleAnnouncementByIdAsync(announcement.Id));
        _anonuncements.Add(announcement);
        return announcement;
    }

    public async Task<Announcement> GetSingleAnnouncementByIdAsync(long id)
    {
        return _anonuncements.FirstOrDefault(announcement => announcement.Id == id);
    }

    public async Task<List<Announcement>> GetALlAnnouncementsAsync()
    {
        return _anonuncements;
    }

    public async Task<List<Announcement>> GetMostRecentAnnouncementsAsync(int amount)
    {
        _anonuncements.Sort((announcement, announcement1) => DateTime.Compare(announcement.DateTimeOfPosting.Value, announcement1.DateTimeOfPosting.Value));
        _anonuncements.Reverse();
        return _anonuncements.Count >= amount ? _anonuncements.GetRange(0, amount): _anonuncements;
    }

    public async Task DeleteSingleAnnouncement(long id)
    {
        var announcementToDelete = _anonuncements.Find(announcement => announcement.Id == id);
        _anonuncements.Remove(announcementToDelete);
    }
}