using HQPlusAssignments.Application.Core.Report;
using HQPlusAssignments.Common;
using NUnit.Extension.DependencyInjection;
using NUnit.Framework;

namespace HQPlusAssignments.Application.Test.Report
{
    [DependencyInjectingTestFixture]
    public class HotelReportTest
    {
        private readonly IHotelReportService _hotelReportService;

        public HotelReportTest(IHotelReportService hotelReportService)
        {
            _hotelReportService = hotelReportService;
        }

        [Test]
        public void GenerateExcelFromJsonFile_Test()
        {
            var result = _hotelReportService.GenerateExcelFromJsonFile();
            var dataTable = ExcelHelper.GetDataTableFromExcel(result, true);
            Assert.That(dataTable.Rows.Count, Is.GreaterThan(0));
        }
    }
}
