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
            var simpleDto = new SimpleEmployeeDTO
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                WorkingNumber = employee.WorkingNumber, 
                Id = employee.Id
            };
            
            return Ok(simpleDto);

        }
        catch(ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpPatch] //problem: shifts are erased when user is updated
    public async Task<ActionResult<SimpleEmployeeDTO>> UpdateEmployee([FromQuery] long id, [FromBody] EmployeeDTO request)
    {
        try
        {
            List<Shift> shifts = new List<Shift>();
            
            Employee existingEmployee = await employeeRepo.GetSingleAsync(id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee with that working number {id} not found");
            }


            existingEmployee.FirstName = request.FirstName;
            existingEmployee.LastName = request.LastName;
            existingEmployee.WorkingNumber = request.WorkingNumber;
            existingEmployee.Email = request.Email;
            existingEmployee.PhoneNumber = request.PhoneNumber;
            existingEmployee.Shifts = EmployeeGrpcRepository.EntityShiftDtosToEntityShiftsList(request.Shifts);
            existingEmployee.Id = request.Id;

            Employee updated = await employeeRepo.UpdateAsync(existingEmployee);
            
            SimpleEmployeeDTO dto = new()
            {
                FirstName = updated.FirstName,
                LastName = updated.LastName,
                WorkingNumber = updated.WorkingNumber,
                Id = updated.Id
                
            };
            return Accepted($"/Employee/{dto.Id}", " was updated.");
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
    
    [HttpGet("/Employee/{Id}")]
    public async Task<ActionResult<PublicEmployeeDTO>> GetSingle([FromRoute] long id)
    {
        if(id==null)
        {
            return BadRequest("Working number required.");
        }

        try
        {
            Employee gotEmployee = await employeeRepo.GetSingleAsync(id);

            PublicEmployeeDTO dto = new()
            {
                WorkingNumber = gotEmployee.WorkingNumber,
                FirstName = gotEmployee.FirstName,
                LastName =  gotEmployee.LastName
            };
            return Accepted($"/Employee/{dto.WorkingNumber}", dto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message); 
        }
        
    }
    
    [HttpGet("/Employee/")]
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
    
    [HttpDelete]
    public async Task<ActionResult> Delete([FromBody] DeleteEmployeeDTO request)
    {
        if (request.WorkingNumber == 0)
        {
            return BadRequest("Working Number is required.");
        }
        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            return BadRequest("First Name is required.");
        }
        if (string.IsNullOrWhiteSpace(request.LastName))
        {
            return BadRequest("First Name is required.");
        }
        if (!request.Password.Equals(employeeRepo.GetSingleAsync(request.WorkingNumber).Result.Password))
        {
            return Unauthorized("Incorrect password.");
        }
    
        
        
        try
        {
            Employee employeeToDelete = await employeeRepo.GetSingleAsync(request.WorkingNumber);
            
            if (employeeToDelete == null)
            {
                return BadRequest($"Employee with that working number {request.WorkingNumber} not found");
            }
            
            
            await employeeRepo.DeleteAsync(request.WorkingNumber);
            return Ok("Employee deleted successfully.");
            
           
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message); 
        }
    
    }
}