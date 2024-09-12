﻿
using LeaveManagement.Application.Logging;

using LeaveManagement.Shared.Common;

using LeaveManagement.Domain.Interfaces;
using LeaveManagement.Domain.LeaveAllocations;
using LeaveManagement.Domain.LeaveRequests;
using LeaveManagement.Domain.LeaveTypes;
using LeaveManagement.Domain.Models.EmailMessage;
using LeaveManagement.Infrastructure.DatabaseContext;
using LeaveManagement.Infrastructure.EmailSender;
using LeaveManagement.Infrastructure.Logging;
using LeaveManagement.Infrastructure.Repositories;
using LeaveManagement.Infrastructure.Security;
using LeaveManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Interfaces;
using LeaveManagement.Identity.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using LeaveManagement.Application.Interfaces;

namespace LeaveManagement.Infrastructure
{
    public static class InfrastructureServiceDependency
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LRDataBaseContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("LeaveManagementConnectionString")));

            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
            services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailService>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddScoped<IAccessUser, AccessUser>();
            services.AddScoped<IS3Service, S3Service>();

            services.AddSingleton<DriveService>(provider =>
            {
                UserCredential credential;
                using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        new[] { DriveService.Scope.Drive },
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }

                return new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Leave Management App",  
                });
            });

           
            services.AddScoped<ICloudStorageService, GoogleDriveService>();
            return services;
        }
    }
}
