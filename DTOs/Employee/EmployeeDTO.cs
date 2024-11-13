using DTOs.Shift;

namespace DTOs;

public class EmployeeDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int WorkingNumber { get; set; }
    public int Id { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<ShiftDTO> Shifts { get; set; }

}