namespace HQPlusAssignments.Application.Core.Report
{
    public interface IHotelReportService
    {
        byte[] GenerateExcelFromJsonFile();
    }
}
