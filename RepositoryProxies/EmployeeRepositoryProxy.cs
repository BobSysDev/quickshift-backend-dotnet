﻿using GrpcClient;
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

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        Employee e =  await _employeeStorageRepository.UpdateAsync(employee);
        await _employeeCachingRepository.UpdateAsync(employee);
        
        return e;
    }

    public async Task DeleteAsync(long id)
    {
        await _employeeCachingRepository.DeleteAsync(id);
        await _employeeStorageRepository.DeleteAsync(id);
    }

    public IQueryable<Employee> GetManyAsync()
    {
        RefreshCache();
        return _employeeCachingRepository.GetManyAsync();
    }

    public async Task<Employee> GetSingleAsync(long id)
    {
        await RefreshCache();
        //return await _employeeCachingRepository.GetSingleAsync(id);
        return await _employeeStorageRepository.GetSingleAsync(id);
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