using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class EmployeeInMemoryRepository : IEmployeeRepository
{
    private List<Employee> _employees = new List<Employee>();

    public async Task<Employee> AddAsync(Employee employee)
    {
        try
        {
            if (_employees.Any(e => e.Id == employee.Id))
            {
                throw new InvalidOperationException($"Employee with ID {employee.Id} already exists.");
            }

            _employees.Add(employee);
            return employee;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the employee.", ex);
        }
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        
        try
        {
            Employee? existingEmployee = _employees.SingleOrDefault(e => e.Id == employee.Id);
            if (existingEmployee is null) throw new InvalidOperationException($"User({employee.Id}) not found");
            _employees.Remove(existingEmployee);
            _employees.Add(employee);
            return employee;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the employee.", ex);
        }
    }

    public Task DeleteAsync(long id)
    {
        try
        {
            Employee? employeeToRemove = _employees.SingleOrDefault(e => e.Id == id);
            if (employeeToRemove is null) throw new InvalidOperationException($"User({id}) not found");
            _employees.Remove(employeeToRemove);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the employee.", ex);
        }
    }

    public IQueryable<Employee> GetManyAsync()
    {
        return _employees.AsQueryable();
    }

    public async Task<Employee> GetSingleAsync(long id)
    {
        try
        {
            Employee? employeeToReturn = _employees.SingleOrDefault(u => u.Id == id);
            if (employeeToReturn is null) throw new InvalidOperationException($"User({id}) not found");
            return employeeToReturn;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the employee.", ex);
        }
    }

    public async Task<Employee> GetSingleEmployeeByWorkingNumberAsync(int WorkingNumber)
    {
        try
        {
            Employee? employeeToReturn = _employees.SingleOrDefault(u => u.WorkingNumber == WorkingNumber);
            if (employeeToReturn is null) throw new InvalidOperationException($"User({WorkingNumber}) not found");
            return employeeToReturn;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the employee by working number.", ex);
        }
    }

    public async Task<bool> IsEmployeeInRepository(long id)
    {
        try
        {
            return _employees.Any(u => u.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while checking if the employee is in the repository.", ex);
        }
    }
}