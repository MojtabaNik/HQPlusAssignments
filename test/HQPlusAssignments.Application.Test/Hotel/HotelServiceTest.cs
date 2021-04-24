using HQPlusAssignments.Application.Core.Exceptions;
using HQPlusAssignments.Application.Core.Hotel;
using HQPlusAssignments.Resources.Hotel;
using NUnit.Extension.DependencyInjection;
using NUnit.Framework;
using System;

namespace HQPlusAssignments.Application.Test.Hotel
{
    [DependencyInjectingTestFixture]
    public class HotelServiceTest
    {
        private readonly IHotelService _hotelService;

        public HotelServiceTest(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [Test]
        public void GetHotel_NotFound_Test()
        {
            Assert.Throws(Is.TypeOf<UserFriendlyException>().And.Message.EqualTo(HotelResourceKeys.HotelNotFound),
         () => _hotelService.GetHotel(1, null));
        }

        [Test]
        public void GetHotel_Test()
        {
            var result = _hotelService.GetHotel(8759, DateTime.Parse("03/15/2016"));
            Assert.IsNotNull(result);
            Assert.AreEqual(8759, result.Hotel.HotelID);
            Assert.That(result.HotelRates.Count, Is.GreaterThan(0));
        }
    }
}
