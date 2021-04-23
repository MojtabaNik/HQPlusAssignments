namespace HQPlusAssignments.Application.Core.HtmlExtractor.Dtos
{
    public class HtmlExtractorNode
    {
        public string Name { get; set; }
        public string XPath { get; set; }
        public bool ShouldCheckInValidation { get; set; }
        public bool ShouldShowInOutPut { get; set; }
        public string Type { get; set; }
        public HtmlExtractorNode[] Childs { get; set; }
    }
}
