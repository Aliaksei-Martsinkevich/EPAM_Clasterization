using System.Collections.Generic;
using Clasterization.Interfaces;

namespace Clasterization.Clasterization.Algorythms
{
    public class KeyCollisionStringComparer : IEqualityComparer<string>
    {
        private readonly IKeyCalculator _keyCalculator;

        public KeyCollisionStringComparer(IKeyCalculator keyCalculator)
        {
            _keyCalculator = keyCalculator;
        }

        public bool Equals(string x, string y)
        {
            return _keyCalculator.CalculateKey(x).Equals(_keyCalculator.CalculateKey(y));
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}