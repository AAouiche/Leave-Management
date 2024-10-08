﻿using LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType;
using LeaveManagement.Application.Logging;
using LeaveManagement.Shared.Common;
using LeaveManagement.Domain.LeaveTypes;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.Logging
{
    
        public class LoggerAdapter<T> : IAppLogger<T>
        {
            private readonly ILogger<T> _logger;

            public LoggerAdapter(ILoggerFactory loggerFactory)
            {
                _logger = loggerFactory.CreateLogger<T>();
            }

            public void LogInformation(string message, params object[] args)
            {
                _logger.LogInformation(message, args);
            }

            public void LogWarning(string message, params object[] args)
            {
                _logger.LogWarning(message, args);
            }
            public void LogError(string message, params object[] args)
            {
                _logger.LogError(message, args);
            }
        }
    
}
