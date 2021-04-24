using HQPlusAssignments.Application.Core.Exceptions;
using HQPlusAssignments.Application.Core.Report;
using HQPlusAssignments.Application.Core.Report.Dtos;
using HQPlusAssignments.Application.Core.System;
using HQPlusAssignments.Application.Core.System.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HQPlusAssignments.Services.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IHotelReportService _hotelReportService;
        private readonly IMailService _mailService;

        public ReportController(IHotelReportService hotelReportService, IMailService mailService)
        {
            _hotelReportService = hotelReportService;
            _mailService = mailService;
        }

        [HttpGet]
        [Route("HotelExcelFile")]
        public ActionResult Get()
        {
            var result = _hotelReportService.GenerateExcelFromJsonFile();

            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
        }

        [HttpPost]
        [Route("SendReportByEmail")]
        public string SendReportByEmail(ReportByEmailInputDto reportByEmailInputDto)
        {
            return _hotelReportService.SendScheduleReport(reportByEmailInputDto.DateTime, reportByEmailInputDto.ToEmail);
        }
    }
}
