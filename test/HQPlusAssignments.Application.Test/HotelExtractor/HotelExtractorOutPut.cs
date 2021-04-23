using System.Collections.Generic;

namespace HQPlusAssignments.Application.Test.HotelExtractor
{
    public class HotelExtractorOutPut
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Classification { get; set; }
        public double ReviewPoints { get; set; }
        public int NumberOfReviews { get; set; }
        public string Description { get; set; }
        public List<RoomCategory> RoomCategories { get; set; }
        public List<AlternativeHotel> AlternativeHotels { get; set; }
    }

    public class RoomCategory
    {
        public string Max { get; set; }
        public string Name { get; set; }
    }

    public class AlternativeHotel
    {
        public string Name { get; set; }
        public int Classification { get; set; }
        public double ReviewPoints { get; set; }
        public int NumberOfReviews { get; set; }
        public int NumberOfViews { get; set; }
        public string Description { get; set; }
    }
}
