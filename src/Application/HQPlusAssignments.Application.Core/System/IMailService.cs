using HQPlusAssignments.Application.Core.System.Dtos;
using System.Threading.Tasks;

namespace HQPlusAssignments.Application.Core.System
{
    public interface IMailService
    {
        /// <summary>
        /// This method is used to send email with attachments using smtp protocol
        /// </summary>
        /// <param name="mailRequest">Mail Request</param>
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
