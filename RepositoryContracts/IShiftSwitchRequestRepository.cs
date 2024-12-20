﻿using Entities;
namespace RepositoryContracts;

    public interface IShiftSwitchRequestRepository
    {
        Task<ShiftSwitchRequest> AddAsync(ShiftSwitchRequest request);
        Task<ShiftSwitchRequest> UpdateAsync(ShiftSwitchRequest request);
        Task DeleteAsync(long id);
        IQueryable<ShiftSwitchRequest> GetManyAsync();
        Task<ShiftSwitchRequest> GetSingleAsync(long id);
        Task<List<ShiftSwitchRequest>> GetByEmployeeAsync(long employeeId);
        Task<List<ShiftSwitchRequest>> GetByShiftAsync(long shiftId);
    }