namespace Entities;

public class Employee
{
  
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int WorkingNumber { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public long Id { get; set; }
    public string Password { get; set; }
    public List<Shift> Shifts = new List<Shift>();
}