using HQPlusAssignments.Application.Core.Exceptions;
using HQPlusAssignments.Application.Core.HtmlExtractor;
using HQPlusAssignments.Application.Core.HtmlExtractor.Dtos;
using HQPlusAssignments.Common;
using HQPlusAssignments.Resources.HtmlExtractor;
using HQPlusAssignments.Resources.SystemErrors;
using HtmlAgilityPack;
using NUnit.Extension.DependencyInjection;
using NUnit.Framework;
using System.IO;

namespace HQPlusAssignments.Application.Test.HtmlExtractor
{
    [DependencyInjectingTestFixture]
    public class HtmlExtractorTest
    {
        private string HtmlNode { get; }
        private string ContentTestHtmlNode { get; }
        private readonly IHtmlExtractorService _htmlExtractorService;

        public HtmlExtractorTest(IHtmlExtractorService htmlExtractorService)
        {
            _htmlExtractorService = htmlExtractorService;
            HtmlNode = File.ReadAllText(FilePathHelper.GetTestDataFolder("HtmlExtractor") + "\\testHtml.html");
            ContentTestHtmlNode = @"<html>
                                <head>test</head>
                                <body>
                                    <h1>Mojtaba1234</h1>
                                    <h2>12.55</h2>
                                </body>
                            </html>";

        }

        #region ConfigReaderTests
        [Test]
        public void HtmlExtractorConfigReader_InvalidPath_Test()
        {
            Assert.Throws(
                Is.TypeOf<UserFriendlyException>().And.Message.EqualTo(HtmlExtractorResourceKeys.ConfigFileNotFound),
                () => _htmlExtractorService.HtmlExtractorConfigReader("unknown"));
        }

        [Test]
        public void HtmlExtractorConfigReader_EmptyConfig_Test()
        {
            var filePath = FilePathHelper.GetTestDataFolder("HtmlExtractor") + "\\emptyConfig.json";

            Assert.Throws(
                Is.TypeOf<UserFriendlyException>().And.Message.EqualTo(HtmlExtractorResourceKeys.InvalidHtmlNodeConfig),
                () => _htmlExtractorService.HtmlExtractorConfigReader(filePath));
        }

        [Test]
        public void HtmlExtractorConfigReader_InvalidJsonFormat_Test()
        {
            var filePath = FilePathHelper.GetTestDataFolder("HtmlExtractor") + "\\invalidConfig.json";

            Assert.Throws(
                Is.TypeOf<UserFriendlyException>().And.Message.EqualTo(HtmlExtractorResourceKeys.InvalidHtmlNodeConfig),
                () => _htmlExtractorService.HtmlExtractorConfigReader(filePath));
        }

        [Test]
        public void HtmlExtractorConfigReader_ValidButEmptyJsonFormat_Test()
        {
            var filePath = FilePathHelper.GetTestDataFolder("HtmlExtractor") + "\\validButEmpty.json";

            Assert.Throws(
                Is.TypeOf<UserFriendlyException>().And.Message.EqualTo(HtmlExtractorResourceKeys.InvalidHtmlNodeConfig),
                () => _htmlExtractorService.HtmlExtractorConfigReader(filePath));
        }

        [Test]
        public void HtmlExtractorConfigReader_ValidJsonFormat_Test()
        {
            var filePath = FilePathHelper.GetTestDataFolder("HtmlExtractor") + "\\validConfig.json";
            var result = _htmlExtractorService.HtmlExtractorConfigReader(filePath);
            Assert.That(result, Has.Length.EqualTo(11));
            Assert.That(result[0].Name, Is.EqualTo("HtmlHead"));

            Assert.That(result[9].Name, Is.EqualTo("RoomCategories"));
            Assert.That(result[9].Childs, Has.Length.EqualTo(2));
            Assert.That(result[9].Childs[0].Name, Is.EqualTo("Max"));


            Assert.That(result[10].Name, Is.EqualTo("AlternativeHotels"));
            Assert.That(result[10].Childs, Has.Length.EqualTo(6));
            Assert.That(result[10].Childs[0].Name, Is.EqualTo("Name"));
        }
        #endregion


        [Test]
        public void ContentFromOneNode_GetString_Test()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(ContentTestHtmlNode);

            var result = _htmlExtractorService.ContentFromOneNode(doc.DocumentNode, "html/head", null);
            Assert.AreEqual("test", result);
        }

        [Test]
        public void ContentFromOneNode_GetInt_Test()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(ContentTestHtmlNode);

            var result = _htmlExtractorService.ContentFromOneNode(doc.DocumentNode, "html/body/h1", "int");
            Assert.AreEqual(1234, result);
        }

        [Test]
        public void ContentFromOneNode_GetFloat_Test()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(ContentTestHtmlNode);

            var result = _htmlExtractorService.ContentFromOneNode(doc.DocumentNode, "html/body/h2", "float");
            Assert.AreEqual(12.55f, result);
        }

        [Test]
        public void ContentFromOneNode_GetStringAsFloat_Test()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(ContentTestHtmlNode);

            var result = _htmlExtractorService.ContentFromOneNode(doc.DocumentNode, "html/body/h1", "float");
            Assert.AreEqual(0, result);
        }

        [Test]
        public void ContentFromOneNode_InvalidType_Test()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(ContentTestHtmlNode);

            //Invalid input
            var result = _htmlExtractorService.ContentFromOneNode(doc.DocumentNode, "html/body/h3", "test");
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void ContentFromOneNode_InvalidNode_Test()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(ContentTestHtmlNode);

            Assert.Throws(
              Is.TypeOf<UserFriendlyException>().And.Message.EqualTo(SystemErrorResourceKeys.InvalidInputData),
              () => _htmlExtractorService.ContentFromOneNode(null, "html/body/h3", "test"));
        }

        [Test]
        public void ContentFromOneNode_InvalidXpath_Test()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(ContentTestHtmlNode);

            Assert.Throws(
            Is.TypeOf<UserFriendlyException>().And.Message.EqualTo(SystemErrorResourceKeys.InvalidInputData),
            () => _htmlExtractorService.ContentFromOneNode(doc.DocumentNode, "111.", null));
        }

        [Test]
        public void GetValuesFromNodes_Test()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(HtmlNode);

            var nodes = new HtmlExtractorNode[] {
                new HtmlExtractorNode{ Name ="Head",ShouldShowInOutPut=true,XPath="html/head" },
                new HtmlExtractorNode{ Name ="H1Text",ShouldShowInOutPut=true,XPath="html/body/h1" },
                 new HtmlExtractorNode{ Name ="H2Text",ShouldShowInOutPut=true,XPath="html/body/h2",Type="float" },
                new HtmlExtractorNode{ Name ="Cars",ShouldShowInOutPut=true,XPath="./html/body/div/div",Childs= new HtmlExtractorNode[]{
                       new HtmlExtractorNode{ Name ="Name",ShouldShowInOutPut=true,XPath="./span[1]" },
                       new HtmlExtractorNode{ Name ="Year",ShouldShowInOutPut=true,XPath="./span[2]" },
                       new HtmlExtractorNode{ Name ="CarModels",ShouldShowInOutPut=true,XPath="./div/span",Childs=new HtmlExtractorNode[]{
                            new HtmlExtractorNode{ Name ="Name",ShouldShowInOutPut=true,XPath="./text()" },
                       } },
                  } }
            };

            var response = _htmlExtractorService.GetValuesFromNodes(nodes, doc.DocumentNode);


            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GetValuesFromNodesOutPut>(response.ToString());

            Assert.AreEqual("test", result.Head);
            StringAssert.Contains("Mojtaba", result.H1Text);
            Assert.AreEqual(12.1f, result.H2Text);
            Assert.That(result.Cars, Has.Count.EqualTo(3));
            Assert.That(result.Cars[0].CarModels, Is.Null);
            Assert.That(result.Cars[1].CarModels, Has.Count.EqualTo(2));
            Assert.That(result.Cars[2].CarModels, Has.Count.EqualTo(4));

        }

        [Test]
        public void ValidateNodes_ValidData_Test()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(HtmlNode);

            var validNodes = new HtmlExtractorNode[] {
                new HtmlExtractorNode{ Name ="Head",ShouldShowInOutPut=true,XPath="html/head",ShouldCheckInValidation=true },
                new HtmlExtractorNode{ Name ="H1Text",ShouldShowInOutPut=true,XPath="html/body/h1",ShouldCheckInValidation=true },
                 new HtmlExtractorNode{ Name ="H2Text",ShouldShowInOutPut=true,XPath="html/body/h2",Type="float",ShouldCheckInValidation=true },
                new HtmlExtractorNode{ Name ="Cars",ShouldShowInOutPut=true,XPath="./html/body/div/div",Childs= new HtmlExtractorNode[]{
                       new HtmlExtractorNode{ Name ="Name",ShouldShowInOutPut=true,XPath="./span[1]",ShouldCheckInValidation=true },
                       new HtmlExtractorNode{ Name ="Year",ShouldShowInOutPut=true,XPath="./span[2]",ShouldCheckInValidation=true },
                       new HtmlExtractorNode{ Name ="CarModels",ShouldShowInOutPut=true,XPath="./div/span",Childs=new HtmlExtractorNode[]{
                            new HtmlExtractorNode{ Name ="Name",ShouldShowInOutPut=true,XPath="./text()" },
                            new HtmlExtractorNode { Name = "Model", ShouldShowInOutPut = true, XPath = "./span/div", ShouldCheckInValidation = false }
                       } },
                  } }
            };

            var response = _htmlExtractorService.ValidateNodes(validNodes, doc.DocumentNode);
            Assert.IsNull(response);
        }

        [Test]
        public void ValidateNodes_InValidData_Test()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(HtmlNode);

            var validNodes = new HtmlExtractorNode[] {
                new HtmlExtractorNode{ Name ="Head",ShouldShowInOutPut=true,XPath="html/head",ShouldCheckInValidation=true },
                new HtmlExtractorNode{ Name ="H1Text",ShouldShowInOutPut=true,XPath="html/body/h1",ShouldCheckInValidation=true },
                 new HtmlExtractorNode{ Name ="H2Text",ShouldShowInOutPut=true,XPath="html/body/h2",Type="float",ShouldCheckInValidation=true },
                new HtmlExtractorNode{ Name ="Cars",ShouldShowInOutPut=true,XPath="./html/body/div/div",Childs= new HtmlExtractorNode[]{
                       new HtmlExtractorNode{ Name ="Name",ShouldShowInOutPut=true,XPath="./span[1]",ShouldCheckInValidation=true },
                       new HtmlExtractorNode{ Name ="Year",ShouldShowInOutPut=true,XPath="./span[2]",ShouldCheckInValidation=true },
                       new HtmlExtractorNode{ Name ="CarModels",ShouldShowInOutPut=true,XPath="./div/span",Childs=new HtmlExtractorNode[]{
                            new HtmlExtractorNode{ Name ="Name",ShouldShowInOutPut=true,XPath="./text()" },
                            new HtmlExtractorNode { Name = "Model", ShouldShowInOutPut = true, XPath = "./span/div", ShouldCheckInValidation = true }
                       } },
                  } }
            };

            var response = _htmlExtractorService.ValidateNodes(validNodes, doc.DocumentNode);
            Assert.IsNotNull(response);
        }
    }
}
