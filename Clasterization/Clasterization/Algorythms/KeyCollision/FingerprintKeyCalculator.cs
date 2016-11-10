using System.Linq;
using Clasterization.Interfaces;

namespace Clasterization.Clasterization.Algorythms.KeyCollision
{
    public class FingerprintKeyCalculator : IKeyCalculator
    {
        public string CalculateKey(string value)
        {
            return value
                .Trim()
                .ToLower()
                .RemovePunctuation()
                .RemoveLongWhiteSpaces()
                .Split(' ')
                .OrderBy(s => s)
                .Aggregate((result, current) => result + ' ' + current);
        }
    }
}