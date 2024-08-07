﻿using LeaveManagement.Application.EmailService;
using LeaveManagement.Application.Logging;
using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.Entities.Email;
using LeaveManagement.Domain.LeaveAllocations;
using LeaveManagement.Domain.LeaveRequests;
using LeaveManagement.Domain.LeaveTypes;
using LeaveManagement.Domain.Models.EmailMessage;
using LeaveManagement.Infrastructure.DatabaseContext;
using LeaveManagement.Infrastructure.EmailSender;
using LeaveManagement.Infrastructure.Logging;
using LeaveManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure
{
    public static class InfrastructureServiceDependency
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LRDataBaseContext>(options =>
                options.UseSqlServer("Server=DESKTOP-60B87CQ\\MSSQLSERVER01;Database=LeaveManagementDB;Trusted_Connection=True;"));

            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
            services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailService>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            return services;
        }
    }
}
