using HQPlusAssignments.Application.Core.HotelExtractor;
using NUnit.Extension.DependencyInjection;
using NUnit.Framework;
using System;

namespace HQPlusAssignments.Application.Test.HotelExtractor
{

    [DependencyInjectingTestFixture]
    public class HotelExtractorTest
    {
        private readonly IHotelExtractorService _hotelService;

        public HotelExtractorTest(IHotelExtractorService hotelService)
        {
            _hotelService = hotelService;
        }

        [Test]
        public void ExtractHotelInformationFromHtml_Test()
        {
            var response = _hotelService.ExtractHotelInformationFromHtml();

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<HotelExtractorOutPut>(response);

            Assert.IsNotNull(result.Name);
            Assert.IsNotEmpty(result.Name);
            Assert.That(result.Name, Has.Length.AtLeast(3));

            Assert.IsNotNull(result.Address);
            Assert.IsNotEmpty(result.Address);
            Assert.That(result.Address, Has.Length.AtLeast(3));

            Assert.That(result.Classification, Is.GreaterThan(0));

            Assert.That(result.NumberOfReviews, Is.GreaterThan(0));

            Assert.IsNotNull(result.AlternativeHotels);
            Assert.IsNotEmpty(result.AlternativeHotels);
            Assert.That(result.AlternativeHotels, Has.Count.AtLeast(1));

            Assert.IsNotNull(result.RoomCategories);
            Assert.IsNotEmpty(result.RoomCategories);
            Assert.That(result.RoomCategories, Has.Count.AtLeast(1));
        }
    }
}
