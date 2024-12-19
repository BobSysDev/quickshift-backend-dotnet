using DTOs;

using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

using Employee = Entities.Employee;
using EmployeeDTO = DTOs.EmployeeDTO;

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

//this is the explanation --->             method is commented out as it was replaced by AuthController.cs/Register                
    // [HttpPost]
    // public async Task<ActionResult<SimpleEmployeeDTO>> AddEmployee([FromBody] NewEmployeeDTO request)
    // {
    //     try
    //     {
    //         Employee employee = await employeeRepo.AddAsync(EmployeeGrpcRepository.EntityNewEmployeeDtoToEntityEmployee(request));
    //         var simpleDto = new SimpleEmployeeDTO
    //         {
    //             FirstName = employee.FirstName,
    //             LastName = employee.LastName,
    //             WorkingNumber = employee.WorkingNumber, 
    //             Id = employee.Id
    //         };
    //         
    //         return Ok(simpleDto);
    //
    //     }
    //     catch(ArgumentException e)
    //     {
    //         return BadRequest(e.Message);
    //     }
    // }


    [HttpPut("{id:int}")] //problem: shifts are erased when user is updated
    public async Task<ActionResult<PublicEmployeeDTO>> UpdateEmployee([FromRoute] int id, [FromBody] DTOs.UpdateEmployeeDTO request)
    {
        try
        {
            Employee? existingEmployee = await employeeRepo.GetSingleAsync(long.CreateChecked(id));
            if (existingEmployee == null)
            {
                return NotFound($"Employee with the ID {id} not found");
            }
            existingEmployee.FirstName = request.FirstName;
            existingEmployee.LastName = request.LastName;
            existingEmployee.WorkingNumber = request.WorkingNumber;
            existingEmployee.Email = request.Email;
            if (request.Password != "")
            {
                existingEmployee.Password = AuthController.Hash(request.Password);
            }
            existingEmployee.IsManager = request.IsManager;

            Employee updated = await employeeRepo.UpdateAsync(existingEmployee);
            
            PublicEmployeeDTO dto = new()
            {
                FirstName = updated.FirstName,
                LastName = updated.LastName,
                WorkingNumber = updated.WorkingNumber,
                Id = updated.Id,
                IsManager = updated.IsManager
            };
            return Ok(dto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
    
    [HttpGet("{id:long}")]
    public async Task<ActionResult<EmployeeDTO>> GetSingle([FromRoute] long id)
    {
        try
        {
            Employee gotEmployee = await employeeRepo.GetSingleAsync(id);
            
            EmployeeDTO dto = new()
            {
                WorkingNumber = gotEmployee.WorkingNumber,
                FirstName = gotEmployee.FirstName,
                LastName =  gotEmployee.LastName,
                Id = gotEmployee.Id,
                Shifts = EntityDtoConverter.ListShiftToListShiftDtos(gotEmployee.Shifts),
                IsManager = gotEmployee.IsManager,
                Email = gotEmployee.Email,
                Password = gotEmployee.Password
            };
            return Ok(dto);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        
    }
    
    [HttpGet]
    public async Task<ActionResult<List<PublicEmployeeDTO>>> GetMany()
    {
        
            IQueryable<Employee> employees = employeeRepo.GetManyAsync();

            List<PublicEmployeeDTO> dtos = employees.Select(employee => new PublicEmployeeDTO
            {
                WorkingNumber = employee.WorkingNumber,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Id = employee.Id,
                IsManager = employee.IsManager
            }).ToList();

            return Ok(dtos);
        
        
    }
    
    [HttpDelete("{id:long}")]
    public async Task<ActionResult> Delete([FromRoute] long id)
    {
        try
        {
            await employeeRepo.DeleteAsync(id);
            return Ok();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message); 
        }
        
    }
}