namespace HQPlusAssignments.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Get a string as input and try to extract numeric part of it
        /// </summary>
        /// <param name="input">input string which contains numbers</param>
        /// <returns>founded numbers as int</returns>
        public static int ExtractNumbers(this string input)
        {
            string b = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                    b += input[i];
            }

            if (b.Length > 0)
                return int.Parse(b);

            return 0;
        }
    }
}
