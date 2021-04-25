namespace HQPlusAssignments.Application.Core.HotelExtractor
{
    public interface IHotelExtractorService
    {
        /// <summary>
        /// Read html content from a file and validate it, then extract neccessary informations based on config file
        /// </summary>
        /// <returns>Json string of result, based on config file</returns>
        string ExtractHotelInformationFromHtml();

        /// <summary>
        /// This method is used to re-initiate config file
        /// </summary>
        void ReInitConfig();
    }
}
