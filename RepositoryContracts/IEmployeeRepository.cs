using Entities;

namespace RepositoryContracts;

public interface IEmployeeRepository
{
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(long id);
    IQueryable<Employee> GetManyAsync();
    Task<Employee> GetSingleAsync(int WorkingNumber);
}