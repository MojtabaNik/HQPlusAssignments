namespace HQPlusAssignments.Application.Core.HotelExtractor
{
    public interface IHotelExtractorService
    {
        string ExtractHotelInformationFromHtml();

        /// <summary>
        /// This method is used to re-initiate config file
        /// </summary>
        void ReInitConfig();
    }
}
