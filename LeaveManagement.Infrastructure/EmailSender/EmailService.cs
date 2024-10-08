﻿using LeaveManagement.Domain.Entities.Email;
using LeaveManagement.Domain.Interfaces;
using LeaveManagement.Domain.Models.EmailMessage;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.EmailSender
{
    public class EmailService : IEmailSender
    {
        public EmailSettings EmailSettings { get; }
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            EmailSettings = emailSettings.Value;
        }
        public async Task<bool> SendEmail(Email email)
        {
            var client = new SendGridClient(EmailSettings.ApiKey);
            var to = new EmailAddress(email.Reciever);
            var from = new EmailAddress
            {
                Email = EmailSettings.FromAddress,
                Name = EmailSettings.FromName
            };

            var message = MailHelper.CreateSingleEmail(from, to, email.Subject, email.MessageBody, email.MessageBody);
            var response = await client.SendEmailAsync(message);

            return response.IsSuccessStatusCode;
        }
    }
}
