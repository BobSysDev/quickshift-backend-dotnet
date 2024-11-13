using Entities;

namespace RepositoryContracts;

public interface IShiftRepository
{
    Task<Shift> AddAsync(Shift shift);
    Task UpdateAsync(Shift shift);
    Task DeleteAsync(Shift shift);
    IQueryable<Shift> GetManyAsync();
    Task<Shift> GetSingleAsync(long id);
}