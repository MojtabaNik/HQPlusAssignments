using HQPlusAssignments.Application.Core.Exceptions;
using HQPlusAssignments.Application.Core.Hotel;
using HQPlusAssignments.Application.Core.Hotel.Dtos;
using HQPlusAssignments.Application.Core.System;
using HQPlusAssignments.Common;
using HQPlusAssignments.Resources.Hotel;
using HQPlusAssignments.Resources.SystemErrors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HQPlusAssignments.Application.Hotel
{
    public class HotelService : IHotelService
    {
        private readonly IFileService _fileService;

        public HotelService(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Read data from hotelRatesList.json file and convert it to C# List, Then Filter it.
        /// </summary>
        /// <param name="hotelId">Hotel Id</param>
        /// <param name="arrivalDate">Arrival Date</param>
        /// <returns>List of HotelInputDto</returns>
        public HotelInputDto GetHotel(int hotelId, DateTime? arrivalDate)
        {
            var jsonContent = _fileService.ReadFileContent(Path.Combine(FilePathHelper.CallingAssemblyDirectoryPath, "Assets", "hotelRatesList.json"));

            //Validate json
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                throw new UserFriendlyException(HotelResourceKeys.InvalidJsonContent);
            }

            try
            {
                //Filter List By Id
                var result = JsonConvert.DeserializeObject<IEnumerable<HotelInputDto>>(jsonContent).FirstOrDefault(c => c.Hotel.HotelID == hotelId);

                if (result == null)
                {
                    throw new UserFriendlyException(HotelResourceKeys.HotelNotFound);
                }

                //Filter List by arivalDate
                if (arrivalDate.HasValue)
                {
                    result.HotelRates = result
                        .HotelRates
                        .Where(c => DateTime.Compare(c.TargetDay.Date, arrivalDate.Value.Date) == 0)
                        .ToList();
                }

                return result;
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (JsonSerializationException)
            {
                throw new UserFriendlyException(HotelResourceKeys.InvalidJsonContent);
            }
            catch
            {
                throw new UserFriendlyException(SystemErrorResourceKeys.SystemUnhandledException);
            }
        }
    }
}
