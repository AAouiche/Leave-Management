using AutoMapper;
using LeaveManagement.Application.Features.LeaveTypes.Dtos;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypes;
using LeaveManagement.Domain.LeaveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.MappingProfiles
{
    public class LeaveTypeProfile : Profile
    {
    
        public LeaveTypeProfile()
        { 
           CreateMap<LeaveTypeDto, LeaveTypeProfile>().ReverseMap();
           CreateMap<LeaveType, LeaveTypeDetailsDto>();
        }
    }
}
