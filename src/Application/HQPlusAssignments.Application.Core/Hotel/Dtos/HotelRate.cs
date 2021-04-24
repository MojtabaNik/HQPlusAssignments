using System;
using System.Collections.Generic;

namespace HQPlusAssignments.Application.Core.Hotel.Dtos
{
    public class HotelRate
    {
        public int Adults { get; set; }
        public int Los { get; set; }
        public Price Price { get; set; }
        public string RateDescription { get; set; }
        public string RateID { get; set; }
        public string RateName { get; set; }
        public List<RateTag> RateTags { get; set; }
        public DateTime TargetDay { get; set; }
    }
}
