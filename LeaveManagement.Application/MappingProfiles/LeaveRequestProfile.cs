using AutoMapper;
using LeaveManagement.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequests.Commands.UpdateLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequests.Dtos;
using LeaveManagement.Domain.LeaveRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Authentication;
using Identity.Dtos;

namespace LeaveManagement.Application.MappingProfiles
{
    public class LeaveRequestProfile : Profile
    {
        public LeaveRequestProfile()
        {
            // Mapping for LeaveRequest -> LeaveRequestListDto
            CreateMap<LeaveRequest, LeaveRequestListDto>()
                .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee != null ? src.Employee : null)) // Handle possible null
                .ForMember(dest => dest.LeaveType, opt => opt.MapFrom(src => src.LeaveType != null ? src.LeaveType : null)) // Handle possible null
                .ReverseMap();

            // Mapping for LeaveRequest -> LeaveRequestDetailsDto
            CreateMap<LeaveRequest, LeaveRequestDetailsDto>()
                .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee != null ? src.Employee : null)) // Handle possible null
                .ForMember(dest => dest.LeaveType, opt => opt.MapFrom(src => src.LeaveType != null ? src.LeaveType : null)) // Handle possible null
                .ReverseMap();

            // Mapping for CreateLeaveRequestCommand -> LeaveRequest
            CreateMap<CreateLeaveRequestCommand, LeaveRequest>();

            // Mapping for UpdateLeaveRequestCommand -> LeaveRequest
            CreateMap<UpdateLeaveRequestCommand, LeaveRequest>();

            // Mapping for ApplicationUser -> UserDto
            CreateMap<ApplicationUser, UserDto>();
        }
    }
}
