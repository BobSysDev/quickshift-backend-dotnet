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
    private static string _grpcAddress = "http://localhost:50051";
    
    public EmployeeRepositoryProxy()
    {
        _employeeCachingRepository = new EmployeeInMemoryRepository();
        _employeeStorageRepository = new EmployeeGrpcRepository(_grpcAddress);
        List<Employee> employees = _employeeStorageRepository.GetManyAsync().ToList();
        employees.ForEach(employee => _employeeCachingRepository.AddAsync(employee));
        _lastCacheUpdate = DateTime.Now;
    }
    public async Task<Employee> AddAsync(Employee employee)
    {
        Employee addedEmployee = await _employeeStorageRepository.AddAsync(employee);
        await _employeeCachingRepository.AddAsync(addedEmployee);
        return addedEmployee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        Employee e =  await _employeeStorageRepository.UpdateAsync(employee);
        await _employeeCachingRepository.UpdateAsync(e);
        
        return e;
    }

    public async Task DeleteAsync(long id)
    {
        await _employeeStorageRepository.DeleteAsync(id);
        await _employeeCachingRepository.DeleteAsync(id);
    }

    public IQueryable<Employee> GetManyAsync()
    {
        RefreshCache();
        return _employeeCachingRepository.GetManyAsync();
    }

    public async Task<Employee> GetSingleAsync(long id)
    {
        await RefreshCache();
        return await _employeeCachingRepository.GetSingleAsync(id);
    }

    public async Task<Employee> GetSingleEmployeeByWorkingNumberAsync(int WorkingNumber)
    {
        await RefreshCache();
        return await _employeeCachingRepository.GetSingleEmployeeByWorkingNumberAsync(WorkingNumber);
    }
    
    
    public async Task<bool> IsEmployeeInRepository(long id)
    {
        await RefreshCache();
        return await _employeeCachingRepository.IsEmployeeInRepository(id);
    }
    
    private async Task RefreshCache()
    {
        if (_lastCacheUpdate.AddMinutes(2).CompareTo(DateTime.Now) <= 0)
        {
            List<Employee> employees = _employeeStorageRepository.GetManyAsync().ToList();
            _employeeCachingRepository = new EmployeeInMemoryRepository();
            employees.ForEach(employee => _employeeCachingRepository.AddAsync(employee));
            _lastCacheUpdate = DateTime.Now;
        }
    }
}