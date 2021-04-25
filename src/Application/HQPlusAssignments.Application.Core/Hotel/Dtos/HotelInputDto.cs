using System.Collections.Generic;

namespace HQPlusAssignments.Application.Core.Hotel.Dtos
{
    public class HotelInputDto
    {
        public Hotel Hotel { get; set; }
        public List<HotelRate> HotelRates { get; set; }
    }
}
