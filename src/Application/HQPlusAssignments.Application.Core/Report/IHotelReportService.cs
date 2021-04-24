using Hangfire;
using System;
using System.Threading.Tasks;

namespace HQPlusAssignments.Application.Core.Report
{
    public interface IHotelReportService
    {
        /// <summary>
        /// Read data from hotelRates.json file and convert it to excel file
        /// </summary>
        /// <returns>byte[] of excel file</returns>
        byte[] GenerateExcelFromJsonFile();

        /// <summary>
        /// Send report by email to a person
        /// </summary>
        /// <param name="ToEmail">Email of the person</param>
        Task SendReportByEmailAsync(string ToEmail);

        /// <summary>
        /// This method is used to send hotel rates report at time x to a custom person
        /// </summary>
        /// <param name="dateTime">Time you want the email to send</param>
        /// <param name="toEmail">Email of the person who you want to send email to</param>
        string SendScheduleReport(DateTime dateTime, string toEmail);


    }
}
