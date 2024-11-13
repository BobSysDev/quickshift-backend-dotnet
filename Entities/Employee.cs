namespace Entities;

public class Employee
{
    // public Employee(string firstName, string lastName, int workingNumber, string email, string phoneNumber, IEnumerable<Shift> shifts)
    // {
    //     FirstName = firstName;
    //     LastName = lastName;
    //     WorkingNumber = workingNumber;
    //     Email = email;
    //     PhoneNumber = phoneNumber;
    //     Shifts = new List<Shift>(shifts);
    //     Id = new
    //     
    // }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int WorkingNumber { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public long Id { get; set; }
    public List<Shift> Shifts = new List<Shift>();
}