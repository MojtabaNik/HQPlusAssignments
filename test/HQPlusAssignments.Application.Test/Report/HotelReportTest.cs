using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using HQPlusAssignments.Application.Core.Exceptions;
using HQPlusAssignments.Application.Core.Report;
using HQPlusAssignments.Application.Core.System;
using HQPlusAssignments.Common;
using HQPlusAssignments.Resources.Report;
using Moq;
using NUnit.Extension.DependencyInjection;
using NUnit.Framework;
using System;

namespace HQPlusAssignments.Application.Test.Report
{
    [DependencyInjectingTestFixture]
    public class HotelReportTest
    {
        private readonly IHotelReportService _hotelReportService;
        private readonly IMailService _mailService;
        private readonly IBackgroundJobClient _client;

        public HotelReportTest(IHotelReportService hotelReportService, IMailService mailService, IBackgroundJobClient client)
        {
            _hotelReportService = hotelReportService;
            _mailService = mailService;
            _client = client;
        }

        [Test]
        public void GenerateExcelFromJsonFile_Test()
        {
            var result = _hotelReportService.GenerateExcelFromJsonFile();
            var dataTable = ExcelHelper.GetDataTableFromExcel(result, true);
            Assert.That(dataTable.Rows.Count, Is.GreaterThan(0));
        }

        [Test]
        public void SendReportByEmail_Test()
        {
            _hotelReportService.SendReportByEmailAsync("m0jt0ba@gmail.com").GetAwaiter().GetResult();
        }

        [Test]
        public void SendScheduleReport_Test()
        {
            // Arrange
            var jobId = _hotelReportService.SendScheduleReport(DateTime.Now.AddMinutes(1), "m0jt0ba@gmail.com");

            _client.Create(It.Is<Job>(job => job
                 .Method
                 .Name == "SendReportByEmailAsync" && job
                     .Args[0]
                     .ToString() == jobId), It.IsAny<EnqueuedState>());
        }

        [Test]
        public void SendScheduleReport_InvalidData_Test()
        {
            Assert.Throws(
                Is.TypeOf<UserFriendlyException>().And.Message.EqualTo(HotelReportResourceKeys.InvalidDateTime),
                () => _hotelReportService.SendScheduleReport(DateTime.Now.AddDays(-2), "m0jt0ba@gmail.com"));
        }
    }
}
