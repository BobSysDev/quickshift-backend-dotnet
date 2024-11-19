using Entities;

namespace RepositoryContracts;

public interface IEmployeeRepository
{
    Task<Employee> AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(int WorkingNumber);
    IQueryable<Employee> GetManyAsync();
    Task<Employee> GetSingleAsync(int WorkingNumber);
}