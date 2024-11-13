using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class EmployeeInMemoryRepository : IEmployeeRepository
{
    private List<Employee> employees = new List<Employee>();
    public Task AddAsync(Employee employee)
    {
        //TODO: jakarta
        // employee.Id = employees.Any()
        //     ? employees.Max(e => e.Id) + 1
        //     : 1;
        employees.Add(employee);
        return Task.FromResult(employee);
    }

    public Task UpdateAsync(Employee employee)
    {
        Employee? existingEmployee = employees.SingleOrDefault(e => e.Id == employee.Id);
        if (existingEmployee is null) throw new InvalidOperationException($"User({employee.Id}) not found");
        employees.Remove(existingEmployee);
        employees.Add(employee);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(long id)
    {
        Employee? employeeToRemove = employees.SingleOrDefault(e => e.Id == id);
        if (employeeToRemove is null) throw new InvalidOperationException($"User({id}) not found");
        employees.Remove(employeeToRemove);
        return Task.CompletedTask;
    }

    public IQueryable<Employee> GetManyAsync()
    {
        return employees.AsQueryable();
    }

    public Task<Employee> GetSingleAsync(int workingNumber)
    {
        Employee? employeeToReturn = employees.SingleOrDefault(u => u.WorkingNumber == workingNumber);
        // Console.WriteLine($"Attempted to find User with Id {id}, but none was found.");
        if (employeeToReturn is null) throw new InvalidOperationException($"User({workingNumber}) not found");
        return Task.FromResult(employeeToReturn);
    }
}