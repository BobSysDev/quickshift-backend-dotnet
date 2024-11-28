using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class EmployeeInMemoryRepository : IEmployeeRepository
{
    private List<Employee> _employees = new List<Employee>();

    public async Task<Employee> AddAsync(Employee employee)
    {
        _employees.Add(employee);
        return employee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        Employee? existingEmployee = _employees.SingleOrDefault(e => e.Id == employee.Id);
        if (existingEmployee is null) throw new InvalidOperationException($"User({employee.Id}) not found");
        _employees.Remove(existingEmployee);
        _employees.Add(employee);
        return employee;
    }

    public Task DeleteAsync(long id)
    {
        Employee? employeeToRemove = _employees.SingleOrDefault(e => e.Id == id);
        if (employeeToRemove is null) throw new InvalidOperationException($"User({id}) not found");
        _employees.Remove(employeeToRemove);
        return Task.CompletedTask;
    }

    public IQueryable<Employee> GetManyAsync()
    {
        return _employees.AsQueryable();
    }

    public async Task<Employee> GetSingleAsync(long id)
    {
        Employee? employeeToReturn = _employees.SingleOrDefault(u => u.Id == id);
        // Console.WriteLine($"Attempted to find User with Id {id}, but none was found.");
        if (employeeToReturn is null) throw new InvalidOperationException($"User({id}) not found");
        Console.Write("inmemrepoemp getsingle shifts: "+employeeToReturn.PrintShifts());
        return employeeToReturn;
    }

    public async Task<Employee> GetSingleEmployeeByWorkingNumberAsync(int WorkingNumber)
    {
        Employee? employeeToReturn = _employees.SingleOrDefault(u => u.WorkingNumber == WorkingNumber);
        //Console.WriteLine(_employees.ToString());
        if (employeeToReturn is null) throw new InvalidOperationException($"User({WorkingNumber}) not found");
        return employeeToReturn;
    }

    public async Task<bool> IsEmployeeInRepository(long id)
    {
        Employee? employeeToReturn = _employees.SingleOrDefault(u => u.Id == id);
        if (employeeToReturn==null)
        {
            return false;
        }
        return true;
    }
}