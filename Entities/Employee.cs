namespace Entities;

public class Employee
{
  
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int WorkingNumber { get; set; }
    public string Email { get; set; }
    public long Id { get; set; }
    public string Password { get; set; }
    public List<Shift> Shifts { get; set; }
    public bool IsManager { get; set; }

    public string PrintShifts()
    {
        string s = "";
        foreach (var shift in Shifts)
        {
            s += shift.StartDateTime;
            s += "\n";
            s += shift.EndDateTime;
            s += "\n";
            s += shift.TypeOfShift;
            s += "\n";
            s += shift.ShiftStatus;
            s += "\n";
            s += shift.Description;
            s += "\n";
            s += shift.Location;
            s += "\n";
            s += shift.AssingnedEmployees;  
            s += "\n";
            s += "-----------------";
        }

        return s;
    }
}