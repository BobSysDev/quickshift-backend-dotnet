namespace Entities;

public class Employee
{
    public Employee(string firstName, string lastName, int workingNumber, string email, int phoneNumber, IEnumerable<Shift> shifts)
    {
        FirstName = firstName;
        LastName = lastName;
        WorkingNumber = workingNumber;
        Email = email;
        PhoneNumber = phoneNumber;
        Shifts = new List<Shift>(shifts);
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int WorkingNumber { get; set; }
    public string Email { get; set; }
    public int PhoneNumber { get; set; }
    public List<Shift> Shifts = new List<Shift>();
}