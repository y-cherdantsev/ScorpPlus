﻿using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using ScorpPlus.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ScorpPlus.Services.Notifications
{
    /// <summary>
    /// Email notification system
    /// </summary>
    public class EmailNotificator : INotificator
    {
        /// <summary>
        /// SmtpClient for sending mail notifications
        /// </summary>
        private readonly SmtpClient _smtpClient;

        /// <summary>
        /// Mailbox for sending notifications
        /// </summary>
        private readonly MailboxAddress _mailboxAddress;

        /// <summary>
        /// Just template with topic and message
        /// </summary>
        private class MailTemplate
        {
            /// <summary>
            /// Topic of notification
            /// </summary>
            public string Topic;

            /// <summary>
            /// Message test of notification
            /// </summary>
            public string MessageText;
        }

        /// <summary>
        /// Templates for different types of notifications
        /// </summary>
        /// \todo(Rewrite templates for each type of notifications)
        private readonly Dictionary<NotificationType, MailTemplate> _templates =
            new Dictionary<NotificationType, MailTemplate>
            {
                {
                    NotificationType.General, new MailTemplate
                    {
                        Topic = "General",
                        MessageText = "<b>_MESSAGE</b>"
                    }
                },
                {
                    NotificationType.EmployeeEntered, new MailTemplate
                    {
                        Topic = "Employee Entered",
                        MessageText = "<b>_MESSAGE</b>"
                    }
                },
                {
                    NotificationType.EmployeeEnterForbidden, new MailTemplate
                    {
                        Topic = "Employee Enter Forbidden",
                        MessageText = "<b>_MESSAGE</b>"
                    }
                },
                {
                    NotificationType.EmployeeExited, new MailTemplate
                    {
                        Topic = "Employee Exited",
                        MessageText = "<b>_MESSAGE</b>"
                    }
                },
                {
                    NotificationType.UserAuthorized, new MailTemplate
                    {
                        Topic = "User Authorized",
                        MessageText = "<b>_MESSAGE</b>"
                    }
                },
                {
                    NotificationType.UserCreated, new MailTemplate
                    {
                        Topic = "User Created",
                        MessageText = "<b>_MESSAGE</b>"
                    }
                },
                {
                    NotificationType.EmployeeNotInOffice, new MailTemplate
                    {
                        Topic = "Employee Not In Office",
                        MessageText = "<b>_MESSAGE</b>"
                    }
                },
                {
                    NotificationType.TemperatureDecreasedCritically, new MailTemplate
                    {
                        Topic = "Temperature Decreased Critically",
                        MessageText = "<b>_MESSAGE</b>"
                    }
                },
                {
                    NotificationType.TemperatureIncreasedCritically, new MailTemplate
                    {
                        Topic = "Temperature Increased Critically",
                        MessageText = "<b>_MESSAGE</b>"
                    }
                }
            };

        /// <summary>
        /// Email Notificator constructor
        /// </summary>
        public EmailNotificator(string mailServerUsername, string mailServerPassword, string mailServerName, string mailServerAddress, string mailServeHost, int mailServerPort)
        {
            _smtpClient = new SmtpClient {ServerCertificateValidationCallback = (s, c, h, e) => true};
            _smtpClient.Connect(mailServeHost, mailServerPort, SecureSocketOptions.SslOnConnect);
            _smtpClient.Authenticate(mailServerUsername, mailServerPassword);
            _mailboxAddress = new MailboxAddress(mailServerName, mailServerAddress);
        }

        /// <inheritdoc />
        public async Task Notify(User user, string message, NotificationType notificationType)
        {
            var mailTemplate = _templates[notificationType];

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(_mailboxAddress);
            mimeMessage.To.Add(new MailboxAddress(user.Email, user.Email));
            mimeMessage.Subject = mailTemplate.Topic;

            var builder = new BodyBuilder {HtmlBody = mailTemplate.MessageText.Replace("_MESSAGE", message)};
            mimeMessage.Body = builder.ToMessageBody();

            await _smtpClient.SendAsync(mimeMessage);
        }
    }
}