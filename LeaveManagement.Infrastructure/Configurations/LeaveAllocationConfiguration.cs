using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveManagement.Domain.LeaveAllocation;

namespace LeaveManagement.Infrastructure.Configurations
{
    public class LeaveAllocationConfiguration : IEntityTypeConfiguration<LeaveAllocation>
    {
        public void Configure(EntityTypeBuilder<LeaveAllocation> builder)
        {
            
            builder.HasKey(la => la.Id);

            
            builder.HasOne(la => la.LeaveType)
                .WithMany() 
                .HasForeignKey(la => la.LeaveTypeId)
                .IsRequired(); 

            
            builder.Property(la => la.NumberOfDays).IsRequired();
            builder.Property(la => la.Period).IsRequired();

           
            /*builder.HasOne(la => la.LeaveType)
                .WithMany() 
                .HasForeignKey(la => la.LeaveTypeId)
                .OnDelete(DeleteBehavior.Cascade); */

           
        }
    }
}
