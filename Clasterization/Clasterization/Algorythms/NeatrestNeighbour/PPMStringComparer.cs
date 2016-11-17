using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Clasterization.Clasterization.Algorythms.NeatrestNeighbour
{
    public class PpmStringComparer : IEqualityComparer<string>
    {
        public PpmStringComparer(double stringComparisonAccuracy = 2d)
        {
            StringComparisonAccuracy = stringComparisonAccuracy;
        }

        private double StringComparisonAccuracy { get; }

        private static long CompressStringLength(string str)
        {
            using (var outStream = new MemoryStream())
            {
                using (var stringStream = new MemoryStream(Encoding.UTF8.GetBytes(str)))
                {
                    using (var gZipStream = new GZipStream(outStream, CompressionMode.Compress, true))
                    {
                        stringStream.CopyTo(gZipStream);
                    }
                }
                outStream.Flush();
                return outStream.Position;
            }
        }

        private static double CalculateDistance(string x, string y)
        {
            if (x.Length == 0 && y.Length == 0)
                return 0;
            var result = 1.0 * (CompressStringLength(x + y) + CompressStringLength(y + x))
                   / (CompressStringLength(x + x) + CompressStringLength(y + y));
            return result;
        }

        public bool Equals(string x, string y) =>
            CalculateDistance(x, y) <= StringComparisonAccuracy;

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}