using System.Collections.Generic;
using Phonix;

namespace Clasterization.Clasterization.Algorythms.KeyCollision
{
    public class PhoneticStringComparer : IEqualityComparer<string>
    {
        private static string CalculateKey(string value)
        {
            return new DoubleMetaphone()
                    .BuildKey(value
                        .ToLower()
                        .RemovePunctuation()
                        .RemoveLongWhiteSpaces());
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