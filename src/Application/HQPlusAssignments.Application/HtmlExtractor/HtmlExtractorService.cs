using HQPlusAssignments.Application.Core.Exceptions;
using HQPlusAssignments.Application.Core.HtmlExtractor;
using HQPlusAssignments.Application.Core.HtmlExtractor.Dtos;
using HQPlusAssignments.Application.Core.System;
using HQPlusAssignments.Common.Extensions;
using HQPlusAssignments.Resources.HtmlExtractor;
using HQPlusAssignments.Resources.SystemErrors;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.XPath;

namespace HQPlusAssignments.Application.HtmlExtractor
{
    public class HtmlExtractorService : IHtmlExtractorService
    {
        private readonly IFileService _fileService;

        public HtmlExtractorService(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// This method read nodes from html extractor config file
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns>array of html extractor nodes for further validation and proccessing purposes</returns>
        public HtmlExtractorNode[] HtmlExtractorConfigReader(string configPath)
        {
            if (!File.Exists(configPath))
            {
                throw new UserFriendlyException(HtmlExtractorResourceKeys.ConfigFileNotFound);
            }

            var rawConfig = _fileService.ReadFileContent(configPath);

            if (string.IsNullOrWhiteSpace(rawConfig))
            {
                throw new UserFriendlyException(HtmlExtractorResourceKeys.InvalidHtmlNodeConfig);
            }

            try
            {
                var schema = NJsonSchema.JsonSchema.FromType<HtmlExtractorNode[]>();
                var schemaObject = JSchema.Parse(schema.ToJson());

                var result = JArray.Parse(rawConfig);

                return result == null || result.Count == 0 || !result.IsValid(schemaObject)
                    ? throw new UserFriendlyException(HtmlExtractorResourceKeys.InvalidHtmlNodeConfig)
                    : result.ToObject<HtmlExtractorNode[]>();
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (JsonReaderException)
            {
                throw new UserFriendlyException(HtmlExtractorResourceKeys.InvalidHtmlNodeConfig);
            }
            catch
            {
                throw new UserFriendlyException(SystemErrorResourceKeys.SystemUnhandledException);
            }
        }

        /// <summary>
        /// Getting value out from an html node based on XPath and Type
        /// This method used because with selecting other nodes from HtmlNode we can not get attribute values like src,title etc.
        /// </summary>
        /// <param name="node">HtmlNode which we want to select from it.</param>
        /// <param name="xPath">Xpath to element which we want to extract</param>
        /// <param name="type">Type of result</param>
        /// <returns>an object with correct type based on input type</returns>
        public dynamic ContentFromOneNode(HtmlNode node, string xPath, string type)
        {
            if (node == null || string.IsNullOrEmpty(xPath))
            {
                throw new UserFriendlyException(SystemErrorResourceKeys.InvalidInputData);
            }

            try
            {
                var result = (HtmlNodeNavigator)node.CreateNavigator().SelectSingleNode(xPath);
                //Check type of value and do the right action
                if (type == HtmlExtractorNodeType.Int.Value)
                {
                    return result == null ? (dynamic)0 : (dynamic)result.Value.ExtractNumbers();
                }
                else if (type == HtmlExtractorNodeType.Float.Value)
                {
                    return result == null ? (dynamic)0 : float.TryParse(
                        result.Value,
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out float value) ? value : 0;
                }

                //Take Type as string in all other cases
                return result == null ? (dynamic)string.Empty : (dynamic)HttpUtility.HtmlDecode(result.Value.Replace("\n", "").Replace("\r", "").Trim());
            }
            catch (XPathException)
            {
                throw new UserFriendlyException(SystemErrorResourceKeys.InvalidInputData);
            }
            catch
            {
                throw new UserFriendlyException(SystemErrorResourceKeys.SystemUnhandledException);
            }
        }

        /// <summary>
        /// Proccess raw data to extract information based on config file
        /// Recursively get value of nodes which are exist in config
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="doc"></param>
        /// <returns>Dynamic Json Object</returns>
        public JObject GetValuesFromNodes(HtmlExtractorNode[] nodes, HtmlNode doc)
        {
            //Input Validation Part
            if (doc == null || nodes == null || nodes.Length == 0)
            {
                throw new UserFriendlyException(SystemErrorResourceKeys.InvalidInputData);
            }

            var result = new JObject();

            foreach (var htmlNode in nodes.Where(c => c.ShouldShowInOutPut))
            {
                //If this is last node so return JsonObject
                if (htmlNode.Childs == null || htmlNode.Childs.Length == 0)
                {
                    result.Add(htmlNode.Name, ContentFromOneNode(doc, htmlNode.XPath, htmlNode.Type));
                }
                else
                {
                    var rows = doc.SelectNodes(htmlNode.XPath);

                    if (rows == null)
                    {
                        continue;
                    }

                    var jArray = new JArray();

                    foreach (var row in rows)
                    {
                        //This Method is recursive because we don't know depth of nested node arrays
                        jArray.Add(GetValuesFromNodes(htmlNode.Childs, row));
                    }

                    result.Add(htmlNode.Name, jArray);
                }
            }

            return result;
        }

        /// <summary>
        /// Get all nodes which has validation flag of true and check if they are exist.
        /// </summary>
        /// <param name="nodes">Node array which we want to validate them</param>
        /// <param name="doc">current html node we are working on</param>
        /// <returns>name of nodes which are not exist</returns>
        public string ValidateNodes(HtmlExtractorNode[] nodes, HtmlNode doc)
        {
            //Check if Nodes With validation flags exists
            foreach (var htmlNode in nodes)
            {
                if (htmlNode.Name == "Model")
                {
                    var test = doc.SelectSingleNode(htmlNode.XPath);
                }

                if ((htmlNode.Childs == null || htmlNode.Childs.Length == 0) && htmlNode.ShouldCheckInValidation && doc.SelectSingleNode(htmlNode.XPath) == null)
                {
                    return htmlNode.Name;
                }
                else if (htmlNode.Childs?.Length > 0)
                {


                    var rows = doc.SelectNodes(htmlNode.XPath);

                    if (rows == null)
                    {
                        continue;
                    }

                    foreach (var row in rows)
                    {
                        var res = ValidateNodes(htmlNode.Childs, row);

                        if (res != null)
                        {
                            return $"[{htmlNode.Name}->{res}]";
                        }
                    }
                }
            }

            return null;
        }
    }
}
