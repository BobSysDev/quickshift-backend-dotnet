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

    public async Task UpdateAsync(Employee employee)
    {
        await _employeeCachingRepository.UpdateAsync(employee);
        await _employeeCachingRepository.UpdateAsync(employee);
    }

    public async Task DeleteAsync(int WorkingNumber)
    {
        await _employeeCachingRepository.DeleteAsync(WorkingNumber);
        await _employeeCachingRepository.DeleteAsync(WorkingNumber);
    }

    public IQueryable<Employee> GetManyAsync()
    {
        RefreshCache();
        return _employeeCachingRepository.GetManyAsync();
    }

    public async Task<Employee> GetSingleAsync(int WorkingNumber)
    {
        RefreshCache();
        return await _employeeCachingRepository.GetSingleAsync(WorkingNumber);
    }

    public async Task<bool> IsEmployeeInRepository(long Id)
    {
        RefreshCache();
        return await _employeeCachingRepository.IsEmployeeInRepository(Id);
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