using System;

namespace HQPlusAssignments.Common.Extensions
{
    public static class StringExtensions
    {
        public static int ExtractNumbers(this string input)
        {
            string b = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]))
                    b += input[i];
            }

            if (b.Length > 0)
                return int.Parse(b);

            return 0;
        }
    }
}
