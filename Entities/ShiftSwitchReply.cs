﻿namespace Entities;

public class ShiftSwitchReply
{
    public long Id { get; set; }
    public Shift TargetShift { get; set; }
    public Employee TargetEmployee { get; set; }
    public bool TargetAccepted { get; set; }
    public bool OriginAccepted { get; set; }
    public string Details { get; set; }
}