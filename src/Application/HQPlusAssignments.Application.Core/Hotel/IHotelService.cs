using HQPlusAssignments.Application.Core.Hotel.Dtos;
using System;

namespace HQPlusAssignments.Application.Core.Hotel
{
    public interface IHotelService
    {
        /// <summary>
        /// Read data from hotelRatesList.json file and convert it to C# List, Then Filter it.
        /// </summary>
        /// <param name="hotelId">Hotel Id</param>
        /// <param name="arrivalDate">Arrival Date</param>
        /// <returns>List of HotelInputDto</returns>
        HotelInputDto GetHotel(int hotelId, DateTime? arrivalDate);
    }
}
