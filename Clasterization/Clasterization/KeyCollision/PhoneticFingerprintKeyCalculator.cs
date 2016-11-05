using Clasterization.Clasterization.Algorythms;
using Clasterization.Interfaces;
using Phonix;

namespace Clasterization.Clasterization.KeyCollision
{
    public class PhoneticFingerprintKeyCalculator : IKeyCalculator
    {
        public string CalculateKey(string value)
        {
            return new DoubleMetaphone()
                    .BuildKey(value
                        .ToLower()
                        .RemovePunctuation()
                        .RemoveLongWhiteSpaces());
        }
    }
}
