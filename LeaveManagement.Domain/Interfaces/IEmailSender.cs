﻿using LeaveManagement.Domain.Entities.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(Email email);
    }
}
