using HQPlusAssignments.Application.Core.Exceptions;
using HQPlusAssignments.Application.Core.HotelExtractor;
using HQPlusAssignments.Application.Core.System;
using HQPlusAssignments.Resources.SystemErrors;
using HtmlAgilityPack;
using HQPlusAssignments.Application.Core.HtmlExtractor;
using HQPlusAssignments.Application.Core.HtmlExtractor.Dtos;
using HQPlusAssignments.Common;
using System.IO;

namespace HQPlusAssignments.Application.HotelExtractor
{
    public class HotelExtractorService : IHotelExtractorService
    {
        #region Properties
        /// <summary>
        /// Read All Nodes which we want to validate and proccess them for our extractor from config file
        /// </summary>
        private static HtmlExtractorNode[] _hotelExtractorNodes;
        private HtmlExtractorNode[] HotelExtractorNodes => _hotelExtractorNodes
            ?? _htmlExtractorService.HtmlExtractorConfigReader(Path.Combine(FilePathHelper.CallingAssemblyDirectoryPath, "Assets", "BookingComHtmlNodes.json"));

        private readonly IFileService _fileService;
        private readonly IHtmlExtractorService _htmlExtractorService;
        #endregion

        #region ctor
        public HotelExtractorService(
            IFileService fileService,
            IHtmlExtractorService htmlExtractorService)
        {
            _fileService = fileService;
            _htmlExtractorService = htmlExtractorService;
        }
        #endregion

        /// <summary>
        /// Read html content from a file and validate it, then extract neccessary informations based on config file
        /// </summary>
        /// <returns>Json string of result, based on config file</returns>
        public string ExtractHotelInformationFromHtml()
        {
            //Read File ContentPart
            var htmlContent = _fileService.ReadFileContent(Path.Combine(FilePathHelper.CallingAssemblyDirectoryPath, "Assets", "extraction.booking.html"));

            //Validate html to see if it includes html tags which we need them.
            if (string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new UserFriendlyException(HotelExtractorResourceKeys.HtmlContentEmptyMessage);
            }

            //Read html document with the help of HtmlAgility Package
            //https://html-agility-pack.net/documentation
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            //Check if neccessary nodes with validation flags exists
            var res = _htmlExtractorService.ValidateNodes(HotelExtractorNodes, doc.DocumentNode);
            if (res != null)
            {
                throw new UserFriendlyException(string.Format(HotelExtractorResourceKeys.InvalidHtmlFormat, res));
            }

            //Proccess raw data to extract information
            return _htmlExtractorService.GetValuesFromNodes(HotelExtractorNodes, doc.DocumentNode).ToString();
        }

        /// <summary>
        /// This method is used to re-initiate config file
        /// </summary>
        public void ReInitConfig()
        {
            _hotelExtractorNodes = null;
        }
    }
}
