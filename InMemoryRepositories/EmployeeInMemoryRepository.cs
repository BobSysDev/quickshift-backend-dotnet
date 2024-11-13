using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class EmployeeInMemoryRepository : IEmployeeRepository
{
    public Task AddAsync(Employee employee)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Employee employee)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Employee employee)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Employee> GetManyAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Employee> GetSingleAsync(int workingNumber)
    {
        throw new NotImplementedException();
    }
}