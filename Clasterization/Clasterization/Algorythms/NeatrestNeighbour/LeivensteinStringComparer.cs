using System;
using System.Collections.Generic;

namespace Clasterization.Clasterization.Algorythms.NeatrestNeighbour
{
    public class LeivensteinStringComparer :IEqualityComparer<string>
    {
        public LeivensteinStringComparer(double stringComparisonAccuracy = 2d)
        {
            StringComparisonAccuracy = stringComparisonAccuracy;
        }

        private static double CalculateDistance(string x, string y)
        {
            if (x.Length == 0)
                return y.Length;

            if (y.Length == 0)
                return x.Length;

            var distTable = new double[x.Length + 1, y.Length + 1];

            for (var i = 0; i <= x.Length; i++)
            {
                distTable[i, 0] = i;
            }
            for (var j = 0; j <= y.Length; j++)
            {
                distTable[0, j] = j;
            }

            for (var i = 1; i <= x.Length; i++)
            {
                for (var j = 1; j <= y.Length; j++)
                {
                    if (x[i - 1] == y[j - 1])
                    {
                        distTable[i, j] = distTable[i - 1, j - 1];
                        continue;
                    }

                    distTable[i, j] = Math.Min(Math.Min(distTable[i - 1, j], distTable[i, j - 1]), distTable[i - 1, j - 1]) + 1;
                }
            }
            return distTable[x.Length, y.Length];
        }

        private double StringComparisonAccuracy { get; }

        public bool Equals(string x, string y) =>
            CalculateDistance(x, y) <= StringComparisonAccuracy;

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
