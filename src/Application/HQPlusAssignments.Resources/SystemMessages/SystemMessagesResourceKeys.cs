namespace HQPlusAssignments.Resources.SystemMessages
{
    public static class SystemMessagesResourceKeys
    {
        public static string DeleteSuccess = GetValue(nameof(DeleteSuccess));
        public static string GetSuccess = GetValue(nameof(GetSuccess));
        public static string PostSuccess = GetValue(nameof(PostSuccess));
        public static string PutSuccess = GetValue(nameof(PutSuccess));
        public static string SuccessMessage = GetValue(nameof(SuccessMessage));

        private static string GetValue(string key)
        {
            return SystemMessagesResource.ResourceManager.GetString(key);
        }
    }
}
