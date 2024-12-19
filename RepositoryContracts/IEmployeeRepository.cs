using Entities;

namespace RepositoryContracts;

public interface IEmployeeRepository
{
    Task<Employee> AddAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task DeleteAsync(long id);
    IQueryable<Employee> GetManyAsync();
    Task<Employee> GetSingleAsync(long id);
    Task<Employee> GetSingleEmployeeByWorkingNumberAsync(int WorkingNumber);
    Task<Boolean> IsEmployeeInRepository(long id);
}