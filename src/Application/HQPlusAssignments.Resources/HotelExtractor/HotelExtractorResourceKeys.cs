using HQPlusAssignments.Resources.HotelExtractor;

namespace HQPlusAssignments.Resources.SystemErrors
{
    public static class HotelExtractorResourceKeys
    {
        public static string HtmlContentEmptyMessage = GetValue(nameof(HtmlContentEmptyMessage));
        public static string InvalidHtmlFormat = GetValue(nameof(InvalidHtmlFormat));
        public static string ReInitConfigSuccessMessage = GetValue(nameof(ReInitConfigSuccessMessage));

        private static string GetValue(string key)
        {
            return HotelExtractorResource.ResourceManager.GetString(key);
        }
    }
}
