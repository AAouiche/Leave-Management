﻿using LeaveManagement.Shared.Common;
using LeaveManagement.Domain.LeaveAllocations;

using LeaveManagement.Domain.LeaveRequests;
using LeaveManagement.Domain.LeaveTypes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Authentication;

namespace LeaveManagement.Infrastructure.DatabaseContext
{
    public class LRDataBaseContext : IdentityDbContext<ApplicationUser>
    {

        

        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveAllocation> LeaveAllocation { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public LRDataBaseContext(DbContextOptions<LRDataBaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.ApplyConfigurationsFromAssembly(typeof(LRDataBaseContext).Assembly);

            modelBuilder.Entity<LeaveType>().HasData(
                new LeaveType
                {
                    Id = 99,
                    Name = "Holiday",
                    Days = 10,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                }*//*
            );*/

            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
                                     .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
            {
                entry.Entity.DateModified = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DateCreated = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
