using System;
using System.Collections.Generic;
using Clasterization.Interfaces;

namespace Clasterization.Clasterization.Algorythms
{
    public class StringComparer : IEqualityComparer<string>
    {
        private int StringComparisonAccuracy { get; }

        readonly IDistanceCalculator _distanceCalculator;

        public StringComparer(IDistanceCalculator distanceCalculator, int stringComparisonAccuracy = 3)
        {
            _distanceCalculator = distanceCalculator;
            StringComparisonAccuracy = stringComparisonAccuracy;
        }

        public bool Equals(string x, string y) => _distanceCalculator.CalculateDistance(x, y) <= StringComparisonAccuracy;

        public int GetHashCode(string obj) => obj.GetHashCode();
    }
}