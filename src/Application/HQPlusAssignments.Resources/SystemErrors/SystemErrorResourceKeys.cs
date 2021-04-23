namespace HQPlusAssignments.Resources.SystemErrors
{
    public static class SystemErrorResourceKeys
    {
        public static string SystemUnhandledException = GetValue(nameof(SystemUnhandledException));
        public static string FileNotFound = GetValue(nameof(FileNotFound));
        public static string InvalidInputData = GetValue(nameof(InvalidInputData));

        private static string GetValue(string key)
        {
            return SystemErrorsResource.ResourceManager.GetString(key);
        }
    }
}
