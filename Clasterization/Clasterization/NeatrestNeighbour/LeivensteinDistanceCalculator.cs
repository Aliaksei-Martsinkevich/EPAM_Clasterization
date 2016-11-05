using System;
using Clasterization.Interfaces;

namespace Clasterization.Clasterization.NeatrestNeighbour
{
    public class LeivensteinDistanceCalculator : IDistanceCalculator
    {
        public double CalculateDistance(string x, string y)
        {
            if (x.Length == 0)
                Console.WriteLine(y.Length.ToString());
            if (y.Length == 0)
                Console.WriteLine(x.Length.ToString());

            var distTable = new double[x.Length + 1, y.Length + 1];

            for (int i = 0; i <= x.Length; i++)
            {
                distTable[i, 0] = i;
            }
            for (int j = 0; j <= y.Length; j++)
            {
                distTable[0, j] = j;
            }

            for (int i = 1; i <= x.Length; i++)
            {
                for (int j = 1; j <= y.Length; j++)
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
    }
}