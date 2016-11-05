using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clasterization.Clasterization.Algorythms;
using Clasterization.Interfaces;

namespace Clasterization.Clasterization.KeyCollision
{
    public class NGramFingerprintKeyCalculator : IKeyCalculator
    {
        public string CalculateKey(string value)
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
    }
}