﻿using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.LeaveTypes;


namespace LeaveManagement.Domain.Repositories
{
    public interface ILeaveAllocationRepository : IGenericRepository<LeaveAllocation>
    {
        Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id);
        Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails();
        Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(int userId);
        Task<bool> AllocationExists(int userId, int leaveTypeId, int period);
        Task AddAllocations(List<LeaveAllocation> allocations);
        Task<LeaveAllocation> GetUserAllocations(int userId, int leaveTypeId);
    }
}
