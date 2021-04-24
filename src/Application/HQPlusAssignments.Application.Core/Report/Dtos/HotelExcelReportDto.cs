namespace HQPlusAssignments.Application.Core.Report.Dtos
{
    public class HotelExcelReportDto
    {
        public string Arrival_Date { get; set; }
        public string Departure_Date { get; set; }
        public string Price { get; set; }
        public string Currency { get; set; }
        public string RateName { get; set; }
        public int Adults { get; set; }
        public short Breakfast_Included { get; set; }
    }
}
