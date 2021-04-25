namespace HQPlusAssignments.Resources.Hotel
{
    public static class HotelResourceKeys
    {
        public static string InvalidJsonContent = GetValue(nameof(InvalidJsonContent));
        public static string HotelNotFound = GetValue(nameof(HotelNotFound));

        private static string GetValue(string key)
        {
            return HotelResource.ResourceManager.GetString(key);
        }
    }
}
