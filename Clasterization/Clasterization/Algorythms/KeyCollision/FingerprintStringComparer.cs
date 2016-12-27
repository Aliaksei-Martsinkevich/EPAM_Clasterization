using System.Collections.Generic;
using System.Linq;

namespace Clasterization.Clasterization.Algorythms.KeyCollision
{
    public class FingerprintStringComparer : IEqualityComparer<string>
    {
        private static string CalculateKey(string value)
        {
            return value
                .Trim()
                .ToLower()
                .RemovePunctuation()
                .RemoveLongWhiteSpaces()
                .Split(' ')
                .OrderBy(s => s)
                .Aggregate((result, current) => result + current);
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
//TODO:
//NUNIT
//XUNIT 

//MOQ
//Nsubstitute