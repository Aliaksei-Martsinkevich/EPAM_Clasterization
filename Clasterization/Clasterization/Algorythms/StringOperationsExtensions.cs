using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clasterization.Clasterization.Algorythms
{
    public static class StringOperationsExtensions
    {
        public static string RemovePunctuation(this string str)
        {
            return str.Aggregate(new StringBuilder(str.Length),
                (builder, c) =>
                {
                    if (!char.IsPunctuation(c))
                        builder.Append(c);
                    return builder;
                }, builder => builder.ToString());
        }

        public static IEnumerable<string> SplitToNGramms(this string str, int n = 2)
        {
            if (str.Length < n) 
                return new List<string>();
            var result = new List<string>(str.Length - 1);
            for (var i = 0; i <= str.Length - n; i++)
            {
                result.Add(str.Substring(i, n));
            }
            return result;
        }

        public static string RemoveLongWhiteSpaces(this string str)
        {
            return str.Aggregate(new StringBuilder(str.Length),
                (builder, c) =>
                {
                    if (char.IsWhiteSpace(c))
                    {
                        if (builder.Length <= 0) return builder;
                        if (!char.IsWhiteSpace(builder[builder.Length - 1]))
                            builder.Append(' ');
                    }
                    else
                    {
                        builder.Append(c);
                    }
                    return builder;
                }, builder => builder.ToString());
        }
    }
}
