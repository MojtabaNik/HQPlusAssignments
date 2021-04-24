using HQPlusAssignments.Application.Core.Hotel;
using HQPlusAssignments.Application.Core.Hotel.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HQPlusAssignments.Services.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        [Route("{hotelId}")]
        public HotelInputDto Get(int hotelId, DateTime? dateTime)
        {
            return _hotelService.GetHotel(hotelId, dateTime);
        }
    }
}
