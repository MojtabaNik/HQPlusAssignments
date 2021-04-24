using System.Collections.Generic;

namespace HQPlusAssignments.Application.Core.Report.Dtos
{

    public class HotelExcelInputDto
    {
        public Hotel Hotel { get; set; }
        public List<HotelRate> HotelRates { get; set; }
    }


}
