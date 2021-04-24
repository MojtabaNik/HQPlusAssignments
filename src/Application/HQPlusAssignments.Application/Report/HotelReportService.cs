using Hangfire;
using HQPlusAssignments.Application.Core.Exceptions;
using HQPlusAssignments.Application.Core.Hotel.Dtos;
using HQPlusAssignments.Application.Core.Report;
using HQPlusAssignments.Application.Core.Report.Dtos;
using HQPlusAssignments.Application.Core.System;
using HQPlusAssignments.Common;
using HQPlusAssignments.Common.Extensions;
using HQPlusAssignments.Resources.Report;
using HQPlusAssignments.Resources.SystemErrors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HQPlusAssignments.Application.Report
{
    public class HotelReportService : IHotelReportService
    {
        private readonly IFileService _fileService;
        private readonly IMailService _mailService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public HotelReportService(IFileService fileService,
                                  IMailService mailService,
                                  IBackgroundJobClient backgroundJobClient)
        {
            _fileService = fileService;
            _mailService = mailService;
            _backgroundJobClient = backgroundJobClient;
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
                return JsonConvert.DeserializeObject<HotelInputDto>(jsonContent).HotelRates.Select(x => new HotelExcelReportDto
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

        /// <summary>
        /// Schedule 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="toEmail"></param>
        /// <returns>Job Id</returns>
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public string SendScheduleReport(DateTime dateTime, string toEmail)
        {
            if (dateTime < DateTime.Now)
            {
                throw new UserFriendlyException(HotelReportResourceKeys.InvalidDateTime);
            }

            var date = dateTime.Subtract(DateTime.Now);
            return _backgroundJobClient.Schedule(() => SendReportByEmailAsync(toEmail), date);
        }

        /// <summary>
        /// Send Generated Report To a Person
        /// </summary>
        /// <param name="ToEmail">Email of the person who you want to send the report</param>
        public async Task SendReportByEmailAsync(string ToEmail)
        {
            await _mailService.SendEmailAsync(new Core.System.Dtos.MailRequest
            {
                ToEmail = ToEmail,
                Body = "Hello, <br/> Here’s hotel rates;",
                Subject = "Hotel Rates Report",
                Attachments = new List<Core.System.Dtos.MailAttachment>
                {
                    new Core.System.Dtos.MailAttachment{
                        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        FileName = "Report.xlsx",
                        FileContent = GenerateExcelFromJsonFile()
                    }
                }
            }).ConfigureAwait(true);
        }
    }
}
