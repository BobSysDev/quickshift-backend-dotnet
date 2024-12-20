﻿namespace Entities;

public class Shift
{
    public long Id { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string TypeOfShift { get; set; }
    public string ShiftStatus { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public List<long> AssingnedEmployees { get; set; }

    public string Print()
    {
        return "Start: " + StartDateTime +
               "\n End: " + EndDateTime +
               "\n Type: " + TypeOfShift +
               "\n Status: " + ShiftStatus +
               "\n Description: " + Description +
               "\n Location: " + Location +
               "\n Assigned Employees: " + AssingnedEmployees;

    }
}