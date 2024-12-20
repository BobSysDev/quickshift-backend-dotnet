﻿namespace Entities;

public class Announcement
{
    public long Id { get; set; }
    public long AuthorId { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public DateTime? DateTimeOfPosting { get; set; }
}