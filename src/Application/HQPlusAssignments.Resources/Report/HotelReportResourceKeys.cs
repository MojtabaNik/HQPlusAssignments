namespace HQPlusAssignments.Resources.Report
{
    public static class HotelReportResourceKeys
    {
        public static string InvalidJsonContent = GetValue(nameof(InvalidJsonContent));

        private static string GetValue(string key)
        {
            return HotelReportResource.ResourceManager.GetString(key);
        }
    }
}
