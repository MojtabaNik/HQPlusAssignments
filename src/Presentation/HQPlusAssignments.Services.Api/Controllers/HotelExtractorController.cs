using HQPlusAssignments.Application.Core.HotelExtractor;
using HQPlusAssignments.Resources.SystemErrors;
using Microsoft.AspNetCore.Mvc;

namespace HQPlusAssignments.Services.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelExtractorController : ControllerBase
    {
        private readonly IHotelExtractorService _hotelExtractorService;

        public HotelExtractorController(IHotelExtractorService hotelExtractorService)
        {
            _hotelExtractorService = hotelExtractorService;
        }

        [HttpGet]
        public string Get()
        {
            return _hotelExtractorService.ExtractHotelInformationFromHtml();
        }

        [HttpPut]
        [Route("re-initconfig")]
        public string ReIniitConfig()
        {
            _hotelExtractorService.ReInitConfig();

            return HotelExtractorResourceKeys.ReInitConfigSuccessMessage;
        }
    }
}
