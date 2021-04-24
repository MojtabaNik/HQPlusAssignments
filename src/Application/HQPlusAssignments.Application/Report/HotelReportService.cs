using HQPlusAssignments.Application.Core.Exceptions;
using HQPlusAssignments.Application.Core.Report;
using HQPlusAssignments.Application.Core.Report.Dtos;
using HQPlusAssignments.Application.Core.System;
using HQPlusAssignments.Common;
using HQPlusAssignments.Common.Extensions;
using HQPlusAssignments.Resources.Report;
using HQPlusAssignments.Resources.SystemErrors;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace HQPlusAssignments.Application.Report
{
    public class HotelReportService : IHotelReportService
    {
        private readonly IFileService _fileService;

        public HotelReportService(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Read data from hotelRates.json file and convert it to excel file
        /// </summary>
        /// <returns>byte[] of excel file</returns>
        public byte[] GenerateExcelFromJsonFile()
        {
            var jsonContent = _fileService.ReadFileContent(Path.Combine(FilePathHelper.CallingAssemblyDirectoryPath, "Assets", "hotelrates.json"));

            //Validate json
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                throw new UserFriendlyException(HotelReportResourceKeys.InvalidJsonContent);
            }

            try
            {
                return JsonConvert.DeserializeObject<HotelExcelInputDto>(jsonContent).HotelRates.Select(x => new HotelExcelReportDto
                {
                    Arrival_Date = x.TargetDay.ToString("dd'.'MM'.'yy", CultureInfo.InvariantCulture),
                    Departure_Date = x.TargetDay.AddDays(x.Los).ToString("dd'.'MM'.'yy", CultureInfo.InvariantCulture),
                    Price = x.Price.NumericFloat.ToString("n2"),
                    Currency = x.Price.Currency,
                    RateName = x.RateName,
                    Adults = x.Adults,
                    Breakfast_Included = (x
                     .RateTags
                     .Find(v => v.Name.ToLower().Equals("breakfast", StringComparison.Ordinal))?
                     .Shape ?? false)
                     ? (short)1
                     : (short)0,
                }).ToExcel();
            }
            catch (JsonSerializationException)
            {
                throw new UserFriendlyException(HotelReportResourceKeys.InvalidJsonContent);
            }
            catch
            {
                throw new UserFriendlyException(SystemErrorResourceKeys.SystemUnhandledException);
            }
        }
    }
}
