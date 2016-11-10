using Clasterization.Interfaces;
using Phonix;

namespace Clasterization.Clasterization.Algorythms.KeyCollision
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
