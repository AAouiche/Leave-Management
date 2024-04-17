using LeaveManagement.Domain.LeaveTypes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.Configurations
{
    public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            builder.HasKey(lt => lt.Id);

            builder.Property(lt => lt.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(lt => lt.Days)
                .IsRequired();

            
        }
    }
}
