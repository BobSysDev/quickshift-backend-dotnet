using System.Runtime.CompilerServices;
using DTOs;
using GrpcClient;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Exception = System.Exception;
using Microsoft.EntityFrameworkCore;
using Employee = Entities.Employee;
using EmployeeDTO = DTOs.EmployeeDTO;
using NewEmployeeDTO = DTOs.NewEmployeeDTO;
using Shift = Entities.Shift;
using ShiftDTO = DTOs.Shift.ShiftDTO;


namespace RestAPI.Controllers;

[ApiController]
[Route("Employees")] 

public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository employeeRepo;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        employeeRepo = employeeRepository;
    }


    [HttpPost]
    public async Task<ActionResult<SimpleEmployeeDTO>> AddEmployee([FromBody] NewEmployeeDTO request)
    {
        try
        {
            Employee employee = await employeeRepo.AddAsync(EmployeeGrpcRepository.EntityNewEmployeeDtoToEntityEmployee(request));
            Employee addedEmployee = await employeeRepo.AddAsync(employee);
            var simpleDto = new SimpleEmployeeDTO
            {
                FirstName = addedEmployee.FirstName,
                LastName = addedEmployee.LastName,
                WorkingNumber = addedEmployee.WorkingNumber, 
                Id = addedEmployee.Id
            };
            
            return Ok(simpleDto);

        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }


    [HttpPatch] //TODO: sebo fix this
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
    
    [HttpGet("/Employee/")] //TODO: sebo fix getmany
    public async Task<ActionResult<List<PublicEmployeeDTO>>> GetMany()
    {
        try
        {
            List<Employee> employees = await employeeRepo.GetManyAsync().ToListAsync();
            List<PublicEmployeeDTO> dtos = employees.Select(employee => new PublicEmployeeDTO
            {
                WorkingNumber = employee.WorkingNumber,
                FirstName = employee.FirstName,
                LastName = employee.LastName
            }).ToList();
        
            return Ok(dtos);
        }
        catch (Exception e)
        {
            return Problem(e.Message); 
        }
    }
    
    // [HttpDelete]//TODO maybe not working sebo samo
    // public async Task<ActionResult> Delete([FromBody] DeleteEmployeeDTO request)
    // {
    //     if (request.WorkingNumber == 0)
    //     {
    //         return BadRequest("Working Number is required.");
    //     }
    //     if (string.IsNullOrWhiteSpace(request.FirstName))
    //     {
    //         return BadRequest("First Name is required.");
    //     }
    //     if (string.IsNullOrWhiteSpace(request.LastName))
    //     {
    //         return BadRequest("First Name is required.");
    //     }
    //
    //     if (!request.Password.Equals(employeeRepo.GetSingleAsync(request.WorkingNumber).Result.Password))
    //     {
    //         return Unauthorized("Incorrect password.");
    //     }
    //
    //     try
    //     {
    //         Boolean reply = await grpcRepo.DeleteAsync(request);
    //         //Employee employeeToDelete = await employeeRepo.GetSingleAsync(request.WorkingNumber);
    //         
    //         if (reply == false)
    //         {
    //             return NotFound("Employee not found.");
    //         }
    //         
    //         // if (!employeeToDelete.FirstName.Equals(request.FirstName, StringComparison.OrdinalIgnoreCase))
    //         // {
    //         //     return Unauthorized("First Name does not match.");
    //         // }
    //
    //         // if (!string.IsNullOrEmpty(request.Password) && request.Password != employeeToDelete.Password)
    //         // {
    //         //     return Unauthorized("Incorrect password.");
    //         // }
    //         
    //         if (reply)
    //         {
    //             await employeeRepo.DeleteAsync(request.WorkingNumber);
    //             return Ok("Employee deleted successfully.");
    //         }
    //        
    //     }
    //     catch (Exception e)
    //     {
    //         return Problem(e.Message); 
    //     }
    //
    //     return Problem("Shit failed...");
    // }
}