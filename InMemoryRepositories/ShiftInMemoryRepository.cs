using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class ShiftInMemoryRepository : IShiftRepository
{
    private List<Shift> shifts = new List<Shift>();
    
    public Task<Shift> AddAsync(Shift shift)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Shift shift)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Shift shift)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Shift> GetManyAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Shift> GetSingleAsync(int id)
    {
        throw new NotImplementedException();
    }
}