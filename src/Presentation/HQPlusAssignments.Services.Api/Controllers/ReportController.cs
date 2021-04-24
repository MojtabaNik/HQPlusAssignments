using HQPlusAssignments.Application.Core.Report;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HQPlusAssignments.Services.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IHotelReportService _hotelReportService;

        public ReportController(IHotelReportService hotelReportService)
        {
            _hotelReportService = hotelReportService;
        }

        [HttpGet]
        [Route("HotelExcelFile")]
        public ActionResult Get()
        {
            var result = _hotelReportService.GenerateExcelFromJsonFile();

            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
        }
    }
}
