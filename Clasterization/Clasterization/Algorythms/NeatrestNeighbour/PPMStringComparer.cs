using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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

        private static BitArray CompressString(string str)
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
                var pos = (int)outStream.Position;
                outStream.Seek(0, SeekOrigin.Begin);

                var buffer = outStream.GetBuffer().Take(pos).ToArray();

                return new BitArray(buffer);
            }
        }

        private static double CalculateDistance(string x, string y)
        {
            if (x.Length == 0 && y.Length == 0)
                return 0;
            var result = 1.0 * (CompressString(x + y).Length + CompressString(y + x).Length)
                   / (CompressString(x + x).Length + CompressString(y + y).Length);
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