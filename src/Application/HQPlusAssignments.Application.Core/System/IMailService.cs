using HQPlusAssignments.Application.Core.System.Dtos;
using System.Threading.Tasks;

namespace HQPlusAssignments.Application.Core.System
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
