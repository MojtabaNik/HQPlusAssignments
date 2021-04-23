namespace HQPlusAssignments.Resources.HtmlExtractor
{
    public static class HtmlExtractorResourceKeys
    {
        public static string InvalidHtmlNodeConfig = GetValue(nameof(InvalidHtmlNodeConfig));
        public static string ConfigFileNotFound = GetValue(nameof(ConfigFileNotFound));

        private static string GetValue(string key)
        {
            return HtmlExtractorResource.ResourceManager.GetString(key);
        }
    }
}
