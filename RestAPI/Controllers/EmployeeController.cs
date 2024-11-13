using DTOs;
using DTOs.Shift;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RestAPI.Controllers;

[ApiController]
[Route("Employees")] 

public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository employeeRepo;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        this.employeeRepo = employeeRepository;
    }
    
    
    
    [HttpPost]
    public async Task<ActionResult<SimpleEmployeeDTO>> AddEmployee([FromBody] CreateEmployeeDTO request)
    {
        try
        {
            List<Employee> employees = employeeRepo.GetManyAsync().ToList();
            List<SimpleEmployeeDTO> dtos = employees.Select(e => new SimpleEmployeeDTO
            {
                FirstName = e.FirstName,
                LastName = e.LastName,
                WorkingNumber = e.WorkingNumber
            }).ToList();

            return Ok(dtos); // Returning OK with dtos
        }
        catch (Exception e)
        {
            return Problem(e.Message); 
        }
    }
    
    
    
    
    [HttpPatch]
    public async Task<ActionResult<PublicEmployeeDTO>> UpdateEmployee([FromBody] EmployeeDTO request)
    {
        if(request.FirstName == null || request.FirstName.Equals(""))
        {
            return BadRequest("First Name required.");
        }
        
        if(request.LastName == null || request.LastName.Equals(""))
        {
            return BadRequest("Last Name required.");
        }
        
        if(request.FirstName.Equals("string")|| request.LastName.Equals("string"))
        {
            return BadRequest("Invalid input.");
        }

        try
        {
            List<ShiftDTO> shiftDTOs = request.Shifts;
            List<Shift> shifts = new List<Shift>();
        
            foreach (var shiftDto in shiftDTOs)
            {
                var shift = new Shift
                {
                    Description = shiftDto.Description,
                    EndDateTime = shiftDto.EndDateTime,
                    Location = shiftDto.Location,
                    ShiftStatus = shiftDto.ShiftStatus,
                    StartDateTime = shiftDto.StartDateTime,
                    TypeOfShift = shiftDto.TypeOfShift,
                    Id = shiftDto.Id
                };
                shifts.Add(shift);
            }

            Employee employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                WorkingNumber = request.WorkingNumber,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Shifts = shifts,
                Id = request.Id
            };
            
            await employeeRepo.UpdateAsync(employee);
            Employee updated = await employeeRepo.GetSingleAsync(employee.WorkingNumber);
            PublicEmployeeDTO dto = new()
            {
                FirstName = updated.FirstName,
                LastName = updated.LastName,
                WorkingNumber = updated.WorkingNumber
            };
            return Accepted($"/Employees/{dto.WorkingNumber}", updated);
        }
        catch (InvalidDataException e)
        {
            return Problem(e.Message); 
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message); 
        }
        
    }
    
    [HttpGet("/Employee/{WorkingNumber}")]
    public async Task<ActionResult<PublicEmployeeDTO>> GetSingle([FromRoute] int WorkingNumber)
    {
        if(WorkingNumber==null)
        {
            return BadRequest("Working number required.");
        }

        try
        {
            Employee gotEmployee = await employeeRepo.GetSingleAsync(WorkingNumber);

            PublicEmployeeDTO dto = new()
            {
                WorkingNumber = gotEmployee.WorkingNumber,
                FirstName = gotEmployee.FirstName,
                LastName =  gotEmployee.LastName
            };
            return Accepted($"/Employees/{dto.WorkingNumber}", dto);
        }
        catch (InvalidDataException e)
        {
            return Problem(e.Message); 
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message); 
        }
        
    }
    
    [HttpGet ("/Employee/")]
    public async Task<ActionResult<List<PublicEmployeeDTO>>> GetMany()
    {
        try
        {
            List<Employee> employees = employeeRepo.GetManyAsync().ToList();
            List<PublicEmployeeDTO> dtos = new();
            employees.ForEach(employee =>
            {
                dtos.Add(new PublicEmployeeDTO
                {
                    WorkingNumber = employee.WorkingNumber,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName
                });
            });
            
            return Accepted($"/Employees/", dtos);
        }
        catch (Exception e)
        {
            return Problem(e.Message); 
        }
    }
// TODO: add checking the password before deleting, maybe make a separate dto for it, currently it just takes working no., 1st n 2nd name
    [HttpDelete]public async Task<ActionResult> Delete([FromBody] EmployeeDTO request)
    {
        if (request.WorkingNumber == null || request.WorkingNumber == 0) 
        {
            return BadRequest("Working Number required.");
        }
        if(request.FirstName==null || request.FirstName.Equals(""))
        {
            return BadRequest("First Name required.");
        }

        try
        {
            Employee employeeToDelete = await employeeRepo.GetSingleAsync(request.WorkingNumber);
            if (employeeToDelete.WorkingNumber == request.WorkingNumber)
            {
                employeeRepo.DeleteAsync(request.WorkingNumber);
                return Ok();
            }
            return Unauthorized("Wrong Working Number.");
        }
        catch (InvalidDataException e)
        {
            return Problem(e.Message); 
        }
        catch (InvalidOperationException e)
        {
            return Problem(e.Message); 
        }
    }
}