using HQPlusAssignments.Application.Core.HtmlExtractor.Dtos;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace HQPlusAssignments.Application.Core.HtmlExtractor
{
    public interface IHtmlExtractorService
    {
        /// <summary>
        /// This method read nodes from html extractor config file
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns>array of html extractor nodes for further validation and proccessing purposes</returns>
        HtmlExtractorNode[] HtmlExtractorConfigReader(string configPath);

        /// <summary>
        /// Getting value out from an html node based on XPath and Type
        /// This method used because with selecting other nodes from HtmlNode we can not get attribute values like src,title etc.
        /// </summary>
        /// <param name="node">HtmlNode which we want to select from it.</param>
        /// <param name="xPath">Xpath to element which we want to extract</param>
        /// <param name="type">Type of result</param>
        /// <returns>an object with correct type based on input type</returns>
        dynamic ContentFromOneNode(HtmlNode node, string xPath, string type);

        /// <summary>
        /// Proccess raw data to extract information based on config file
        /// Recursively get value of nodes which are exist in config
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="doc"></param>
        /// <returns>Dynamic Json Object</returns>
        JObject GetValuesFromNodes(HtmlExtractorNode[] nodes, HtmlNode doc);

        /// <summary>
        /// Get all nodes which has validation flag of true and check if they are exist.
        /// </summary>
        /// <param name="nodes">Node array which we want to validate them</param>
        /// <param name="doc">current html node we are working on</param>
        /// <returns>name of nodes which are not exist</returns>
        string ValidateNodes(HtmlExtractorNode[] nodes, HtmlNode doc);
    }
}
