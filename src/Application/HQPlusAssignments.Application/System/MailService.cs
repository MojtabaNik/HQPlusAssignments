using HQPlusAssignments.Application.Core.Exceptions;
using HQPlusAssignments.Application.Core.Settings;
using HQPlusAssignments.Application.Core.System;
using HQPlusAssignments.Application.Core.System.Dtos;
using HQPlusAssignments.Resources.SystemErrors;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace HQPlusAssignments.Application.System
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            //Create Email
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail),
                Subject = mailRequest.Subject
            };

            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.FileContent.Length > 0)
                    {
                        builder.Attachments.Add(file.FileName, file.FileContent, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            //Create Smtp
            using var smtp = new SmtpClient();

            try
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            }
            catch (SocketException)
            {
                throw new UserFriendlyException("Can not connect to email host.");
            }
            catch
            {
                throw new UserFriendlyException(SystemErrorResourceKeys.SystemUnhandledException);
            }

            try
            {
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            }
            catch (AuthenticationException)
            {
                throw new UserFriendlyException("Smtp authenticate information is invalid!");
            }
            catch
            {
                throw new UserFriendlyException(SystemErrorResourceKeys.SystemUnhandledException);
            }

            //Send Email through smtp
            await smtp
                .SendAsync(email)
                .ConfigureAwait(false);

            smtp.Disconnect(true);
        }
    }
}
