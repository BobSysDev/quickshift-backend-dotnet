using System.Security.Cryptography;
using DTOs;
using Entities;
using Grpc.Core;
using GrpcClient;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using NewEmployeeDTO = DTOs.NewEmployeeDTO;

namespace RestAPI.Controllers;
[ApiController]
[Route("[controller]")]

public class AuthController: ControllerBase
{
    private IEmployeeRepository _employeeRepository;

    public AuthController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult<SimpleEmployeeDTO>> Authenticate([FromBody] AuthEmployeeDTO request)
    {
        if ((request.WorkingNumber <= 0) ||
            string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Working number or password are wrong.");
        }

        try
        {
            var employeeFromRepository = await _employeeRepository.GetSingleEmployeeByWorkingNumberAsync(request.WorkingNumber);
            var employeeDTOtoReturn = new SimpleEmployeeDTO
            {
                Id = employeeFromRepository.Id,
                FirstName = employeeFromRepository.FirstName,
                LastName = employeeFromRepository.LastName,
                WorkingNumber = employeeFromRepository.WorkingNumber,
                IsManager = employeeFromRepository.IsManager
            };
            if (Validate(employeeFromRepository.Password, request.Password))
            {
                return Ok(employeeDTOtoReturn);
            }

            return Unauthorized();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<SimpleEmployeeDTO>> Register([FromBody] NewEmployeeDTO request)
    {
        try
        {
            request.Password = Hash(request.Password);
            var newEmployee = await _employeeRepository.AddAsync(EntityDtoConverter.NewEmployeeDtoToEmployee(request));
            SimpleEmployeeDTO dto = new SimpleEmployeeDTO
            {
                Id = newEmployee.Id,
                FirstName = newEmployee.FirstName,
                LastName = newEmployee.LastName,
                WorkingNumber = newEmployee.WorkingNumber,
                IsManager = newEmployee.IsManager
                };
            return Ok(dto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    public static string Hash(string password)
    {
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        byte[] hash = pbkdf2.GetBytes(20);

        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);
    
        string savedPasswordHash = Convert.ToBase64String(hashBytes);

        return savedPasswordHash;
    }

    public static bool Validate(string hashString, string password)
    {
        byte[] hashBytes = Convert.FromBase64String(hashString);
    
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);
    
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        byte[] hash = pbkdf2.GetBytes(20);
    
        for (int i = 0; i < 20; i++)
        {
            if (hashBytes[i + 16] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}

