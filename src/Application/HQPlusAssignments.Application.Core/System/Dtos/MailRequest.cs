using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace HQPlusAssignments.Application.Core.System.Dtos
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<MailAttachment> Attachments { get; set; }
    }
}
