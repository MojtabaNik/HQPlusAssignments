namespace HQPlusAssignments.Application.Core.HtmlExtractor.Dtos
{
    public sealed class HtmlExtractorNodeType
    {
        private HtmlExtractorNodeType(string value) { Value = value; }
        public string Value { get; }
        public static HtmlExtractorNodeType Int { get { return new HtmlExtractorNodeType("int"); } }
        public static HtmlExtractorNodeType Float { get { return new HtmlExtractorNodeType("float"); } }
    }
}
