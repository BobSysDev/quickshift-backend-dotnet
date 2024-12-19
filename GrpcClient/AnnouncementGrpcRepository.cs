using Entities;
using Grpc.Core;
using Grpc.Net.Client;
using RepositoryContracts;

namespace GrpcClient;

public class AnnouncementGrpcRepository : IAnnouncementRepository
{
    private string _grpcAddress { get; set; }

    public AnnouncementGrpcRepository(string grpcAddress)
    {
        _grpcAddress = grpcAddress;
    }

    public async Task<Entities.Announcement> AddNewAnnouncementAsync(Entities.Announcement newAnnouncement)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Announcement.AnnouncementClient(channel);
        try
        {
            var reply = await client.AddSingleAnnouncementAsync(
                GrpcDtoConverter.AnnouncementEntityToNewGrpcDto(newAnnouncement));

            Entities.Announcement announcementReceived = GrpcDtoConverter.AnnouncementGrpcDtoToEntity(reply);

            return announcementReceived;
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException($"Author not found: {newAnnouncement.AuthorId}",
                    nameof(newAnnouncement.AuthorId));
            }
            
            throw new Exception("An error occurred while creating the announcement.", e);
        }
    }

    public async Task<Entities.Announcement> UpdateAnnouncementAsync(Entities.Announcement announcement)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Announcement.AnnouncementClient(channel);
        try
        {
            var reply = await client.UpdateSingleAnnouncementAsync(
                GrpcDtoConverter.AnnouncementEntityToGrpcDto(announcement));

            Entities.Announcement announcementReceived = GrpcDtoConverter.AnnouncementGrpcDtoToEntity(reply);

            return announcementReceived;
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException($"Announcement not found: {announcement.Id}",
                    nameof(announcement.Id));
            }
            
            throw new Exception("An error occurred while updating the announcement.", e);
        }
    }

    public async Task<Entities.Announcement> GetSingleAnnouncementByIdAsync(long id)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Announcement.AnnouncementClient(channel);
        try
        {
            var reply = await client.GetSingleAnnouncementByIdAsync(new Id{Id_ = id});

            Entities.Announcement announcementReceived = GrpcDtoConverter.AnnouncementGrpcDtoToEntity(reply);

            return announcementReceived;
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException($"Announcement not found: {id}",
                    nameof(id));
            }
            
            throw new Exception("An error occurred while retrieving the announcement.", e);
        }
    }

    public async Task<List<Entities.Announcement>> GetALlAnnouncementsAsync()
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Announcement.AnnouncementClient(channel);
        try
        {
            var reply = (await client.GetAllAnnouncementsAsync(new Empty())).Announcements;
            List<Entities.Announcement> announcements = new List<Entities.Announcement>();

            foreach (var announcementDto in reply)
            {
                announcements.Add(GrpcDtoConverter.AnnouncementGrpcDtoToEntity(announcementDto));
            }

            return announcements;
        }
        catch (RpcException e)
        {
            throw new Exception("An error occurred while retrieving the announcements.", e);
        }
    }

    public async Task<List<Entities.Announcement>> GetMostRecentAnnouncementsAsync(int amount)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Announcement.AnnouncementClient(channel);
        try
        {
            var reply = (await client.GetMostRecentAnnouncementsAsync(new GenericInteger{Number = amount})).Announcements;
            List<Entities.Announcement> announcements = new List<Entities.Announcement>();

            foreach (var announcementDto in reply)
            {
                announcements.Add(GrpcDtoConverter.AnnouncementGrpcDtoToEntity(announcementDto));
            }

            return announcements;
        }
        catch (RpcException e)
        {
            throw new Exception("An error occurred while retrieving the announcements.", e);
        }
    }

    public async Task DeleteSingleAnnouncement(long id)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Announcement.AnnouncementClient(channel);
        try
        {
            var reply = await client.DeleteSingleAnnouncementAsync(new Id{Id_ = id});
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException($"Announcement not found: {id}",
                    nameof(id));
            }
            
            throw new Exception("An error occurred while deleting the announcement.", e);
        }
    }
}