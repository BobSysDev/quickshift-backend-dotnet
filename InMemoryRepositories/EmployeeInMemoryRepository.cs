using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class EmployeeInMemoryRepository : IEmployeeRepository
{
    private List<Employee> _employees = new List<Employee>();

    public Task<Employee> AddAsync(Employee employee)
    {
        _employees.Add(employee);
        return Task.FromResult(employee);
    }

    public Task UpdateAsync(Employee employee)
    {
        Employee? existingEmployee = _employees.SingleOrDefault(e => e.Id == employee.Id);
        if (existingEmployee is null) throw new InvalidOperationException($"User({employee.Id}) not found");
        _employees.Remove(existingEmployee);
        _employees.Add(employee);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int WorkingNumber)
    {
        Employee? employeeToRemove = _employees.SingleOrDefault(e => e.WorkingNumber == WorkingNumber);
        if (employeeToRemove is null) throw new InvalidOperationException($"User({WorkingNumber}) not found");
        _employees.Remove(employeeToRemove);
        return Task.CompletedTask;
    }

    public IQueryable<Employee> GetManyAsync()
    {
        return _employees.AsQueryable();
    }

    public Task<Employee> GetSingleAsync(int workingNumber)
    {
        Employee? employeeToReturn = _employees.SingleOrDefault(u => u.WorkingNumber == workingNumber);
        // Console.WriteLine($"Attempted to find User with Id {id}, but none was found.");
        if (employeeToReturn is null) throw new InvalidOperationException($"User({workingNumber}) not found");
        return Task.FromResult(employeeToReturn);
    }

    public Task<bool> IsEmployeeInRepository(long Id)//TODO implement
    {
        throw new NotImplementedException();
    }
}