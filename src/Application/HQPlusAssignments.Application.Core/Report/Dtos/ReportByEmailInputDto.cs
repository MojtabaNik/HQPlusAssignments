using System;

namespace HQPlusAssignments.Application.Core.Report.Dtos
{
    public class ReportByEmailInputDto
    {
        public DateTime DateTime { get; set; }
        public string ToEmail { get; set; }
    }
}
