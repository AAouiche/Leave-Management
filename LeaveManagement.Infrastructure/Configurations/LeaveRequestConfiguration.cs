using LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.Configurations
{
    public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            
            builder.HasKey(lr => lr.Id);

            
            builder.HasOne(lr => lr.LeaveType) 
                .WithMany()
                .HasForeignKey(lr => lr.LeaveTypeId)
                .IsRequired();

           
            builder.Property(lr => lr.StartDate).IsRequired();
            builder.Property(lr => lr.EndDate).IsRequired();
            builder.Property(lr => lr.DateRequested).IsRequired();
            builder.Property(lr => lr.RequestingEmployeeId).IsRequired();

           
            builder.Property(lr => lr.RequestComments).HasMaxLength(500);

           
            builder.Property(lr => lr.Approved).IsRequired(false);

            

           
        }
    }
}
