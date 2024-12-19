using GrpcClient;
using InMemoryRepositories;
using RepositoryContracts;
using Announcement = Entities.Announcement;

namespace RepositoryProxies;

public class AnnouncementRepositoryProxy : IAnnouncementRepository
{
    private IAnnouncementRepository _announcementCachingRepository { get; set; }
    private IAnnouncementRepository _announcementStorageRepository { get; set; }
    private DateTime _lastCacheUpdate { get; set; }
    private static string _grpcAddress = "http://localhost:50051";

    public AnnouncementRepositoryProxy()
    {
        _announcementCachingRepository = new AnnouncementInMemoryRepository();
        _announcementStorageRepository = new AnnouncementGrpcRepository(_grpcAddress);

        List<Announcement> announcements = _announcementStorageRepository.GetALlAnnouncementsAsync().Result;
        announcements.ForEach(announcement => _announcementCachingRepository.AddNewAnnouncementAsync(announcement));

        _lastCacheUpdate = DateTime.Now;
    }


    public async Task<Announcement> AddNewAnnouncementAsync(Announcement newAnnouncement)
    {
        var addedAnnouncement = await _announcementStorageRepository.AddNewAnnouncementAsync(newAnnouncement);
        await _announcementCachingRepository.AddNewAnnouncementAsync(addedAnnouncement);
        return addedAnnouncement;
    }

    public async Task<Announcement> UpdateAnnouncementAsync(Announcement announcement)
    {
        var updatedAnnouncement = await _announcementStorageRepository.UpdateAnnouncementAsync(announcement);
        await _announcementCachingRepository.UpdateAnnouncementAsync(updatedAnnouncement);
        return updatedAnnouncement;
    }

    public async Task<Announcement> GetSingleAnnouncementByIdAsync(long id)
    {
        await RefreshCache();
        return await _announcementCachingRepository.GetSingleAnnouncementByIdAsync(id);
    }

    public async Task<List<Announcement>> GetALlAnnouncementsAsync()
    {
        await RefreshCache();
        return await _announcementCachingRepository.GetALlAnnouncementsAsync();
    }

    public async Task<List<Announcement>> GetMostRecentAnnouncementsAsync(int amount)
    {
        await RefreshCache();
        return await _announcementCachingRepository.GetMostRecentAnnouncementsAsync(amount);
    }

    public async Task DeleteSingleAnnouncement(long id)
    {
        await _announcementStorageRepository.DeleteSingleAnnouncement(id);
        await _announcementCachingRepository.DeleteSingleAnnouncement(id);
    }

    private async Task RefreshCache()
    {
        if (_lastCacheUpdate.AddMinutes(2).CompareTo(DateTime.Now) <= 0)
        {
            List<Announcement>
                announcements = _announcementStorageRepository.GetALlAnnouncementsAsync().Result;
            _announcementCachingRepository = new AnnouncementInMemoryRepository();
            announcements.ForEach(announcement => _announcementCachingRepository.AddNewAnnouncementAsync(announcement));

            _lastCacheUpdate = DateTime.Now;
        }
    }
}