﻿namespace DTOs;

public class NewEmployeeDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int WorkingNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsManager { get; set; }
}