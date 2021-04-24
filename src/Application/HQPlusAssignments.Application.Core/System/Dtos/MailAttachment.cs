namespace HQPlusAssignments.Application.Core.System.Dtos
{
    public class MailAttachment
    {
        public byte[] FileContent { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
