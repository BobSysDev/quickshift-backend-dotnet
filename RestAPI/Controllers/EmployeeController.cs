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
using UpdateEmployeeDTO = GrpcClient.UpdateEmployeeDTO;


namespace RestAPI.Controllers;

[ApiController]
[Route("[controller]")] 

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


    [HttpPut("/Employee/{id:int}")] //problem: shifts are erased when user is updated
    public async Task<ActionResult<SimpleEmployeeDTO>> UpdateEmployee([FromRoute] int id, [FromBody] DTOs.UpdateEmployeeDTO request)
    {
        try
        {
            //List<Shift> shifts = new List<Shift>();
            
            Employee existingEmployee = await employeeRepo.GetSingleAsync(long.CreateChecked(id));
            if (existingEmployee == null)
            {
                return NotFound($"Employee with the ID {id} not found");
            }


            existingEmployee.FirstName = request.FirstName;
            existingEmployee.LastName = request.LastName;
            existingEmployee.WorkingNumber = request.WorkingNumber;
            existingEmployee.Email = request.Email;
            //existingEmployee.Shifts = EmployeeGrpcRepository.EntityShiftDtosToEntityShiftsList(request.Shifts);
            //existingEmployee.Id = request.Id;
            existingEmployee.Password = AuthController.Hash(request.Password);

            Employee updated = await employeeRepo.UpdateAsync(existingEmployee);
            
            SimpleEmployeeDTO dto = new()
            {
                FirstName = updated.FirstName,
                LastName = updated.LastName,
                WorkingNumber = updated.WorkingNumber,
                Id = updated.Id
                
            };
            return Accepted($"/Employee/{dto.Id}", $"{dto.FirstName} {dto.LastName} id=[{dto.Id}] was updated!");
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
    
    [HttpGet("/Employee/{id:long}")]
    public async Task<ActionResult<PublicEmployeeDTO>> GetSingle([FromRoute] long id)
    {
       // Console.WriteLine(id.GetType());
        try
        {
            Employee gotEmployee = await employeeRepo.GetSingleAsync(id);
            Console.WriteLine(gotEmployee.FirstName);
            PublicEmployeeDTO dto = new()
            {
                WorkingNumber = gotEmployee.WorkingNumber,
                FirstName = gotEmployee.FirstName,
                LastName =  gotEmployee.LastName
            };
            return Accepted($"/Employee/{gotEmployee.Id}", dto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message); 
        }
        
    }
    
    [HttpGet("/Employees/")]
    public async Task<ActionResult<List<PublicEmployeeDTO>>> GetMany()
    {
        try
        {
            Console.WriteLine("1");
            IQueryable<Employee> employees = employeeRepo.GetManyAsync();
            Console.WriteLine("2");

            List<PublicEmployeeDTO> dtos = employees.Select(employee => new PublicEmployeeDTO
            {
                WorkingNumber = employee.WorkingNumber,
                FirstName = employee.FirstName,
                LastName = employee.LastName
            }).ToList();
            Console.WriteLine("3");

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
        try
        {
            Employee employee = await employeeRepo.GetSingleAsync(request.id);
            if (AuthController.Validate(employee.Password, request.Password))
            {
                await employeeRepo.DeleteAsync(request.id);
                return Ok("Employee deleted successfully.");
            }
            return NotFound("Employee with these credentials not found.");
           
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message); 
        }
    
    }
}