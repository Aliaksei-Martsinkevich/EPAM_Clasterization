using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clasterization.Clasterization.Algorythms.KeyCollision
{
    public class NGramStringComparer : IEqualityComparer<string>
    {
        private static string CalculateKey(string value)
        {
            return value
                .Trim()
                .ToLower()
                .RemovePunctuation()
                .RemoveLongWhiteSpaces()
                .Split(' ')
                .Select(x => x.SplitToNGramms())
                .Aggregate(new List<string>() as IEnumerable<string>, (result, word) => result.Concat(word))
                .OrderBy(x => x)
                .Aggregate(new StringBuilder(), (builder, s) => builder.Append(s), builder => builder.ToString());
        }

        public bool Equals(string x, string y)
        {
            return CalculateKey(x).Equals(CalculateKey(y));
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}