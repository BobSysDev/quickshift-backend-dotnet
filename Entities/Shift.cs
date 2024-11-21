﻿namespace Entities;

public class Shift
{
    // public Shift(long id, DateTime startDateTime, DateTime endDateTime, string typeOfShift, string shiftStatus, string description, string location)
    // {
    //     Id = id;
    //     StartDateTime = startDateTime;
    //     EndDateTime = endDateTime;
    //     TypeOfShift = typeOfShift;
    //     ShiftStatus = shiftStatus;
    //     Description = description;
    //     Location = location;
    //     
    // }

    public long Id { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string TypeOfShift { get; set; }
    public string ShiftStatus { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public List<long> AssignedEmployees { get; set; } = new List<long>();
}