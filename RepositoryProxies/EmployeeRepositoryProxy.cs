using GrpcClient;
using InMemoryRepositories;
using RepositoryContracts;
using Employee = Entities.Employee;
using Shift = Entities.Shift;

namespace RepositoryProxies;

public class EmployeeRepositoryProxy : IEmployeeRepository
{
    private IEmployeeRepository _employeeCachingRepository { get; set; } //caching = inmemory
    private IEmployeeRepository _employeeStorageRepository { get; set; }//storage = grpc(java) -> DB
    private DateTime _lastCacheUpdate { get; set; }
    
    public EmployeeRepositoryProxy()
    {
        _employeeCachingRepository = new EmployeeInMemoryRepository();
        _employeeStorageRepository = new EmployeeGrpcRepository();
        List<Employee> employees = _employeeStorageRepository.GetManyAsync().ToList();
        employees.ForEach(employee => _employeeCachingRepository.AddAsync(employee));
        _lastCacheUpdate = DateTime.Now;
    }
    public async Task<Employee> AddAsync(Employee employee)
    {
        Employee addedEmployee = await _employeeStorageRepository.AddAsync(employee);
        await _employeeCachingRepository.AddAsync(employee);
        return addedEmployee;
    }

    public Task UpdateAsync(Employee employee)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int WorkingNumber)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Employee> GetManyAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Employee> GetSingleAsync(int WorkingNumber)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsEmployeeInRepository(long Id)
    {
        throw new NotImplementedException();
    }
    
    private async void RefreshCache()
    {
        if (_lastCacheUpdate.AddMinutes(2).CompareTo(DateTime.Now) > 0)
        {
            List<Employee> employees =_employeeStorageRepository.GetManyAsync().ToList();
            employees.ForEach(async employee =>
            {
                if (await _employeeCachingRepository.IsEmployeeInRepository(employee.Id))
                {
                    await _employeeCachingRepository.UpdateAsync(employee);
                }
                else
                {
                    await _employeeCachingRepository.AddAsync(employee);
                }
            });
            _lastCacheUpdate = DateTime.Now;
        }
    }
}