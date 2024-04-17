using FluentValidation;
using LeaveManagement.Application.Behaviours;
using LeaveManagement.Application.Features.LeaveTypes.Commands.DeleteLeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveDetails;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application
{
    public static class ApplicationServiceDepenedency
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //validation
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));
            services.AddTransient<IValidator<DeleteLeaveTypeCommand>, DeleteLeaveTypeCommandValidator>();
            services.AddTransient<IValidator<UpdateLeaveTypeCommand>, UpdateLeaveTypeCommandValidator>();
            services.AddTransient<IValidator<GetLeaveTypesDetailsQuery>, GetLeaveTypesDetailsQueryValidator>();




            return services;
        }
    }
}
