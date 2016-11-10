using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Clasterization.Interfaces;

namespace Clasterization.Clasterization.Algorythms.NeatrestNeighbour
{
    public class PpmDistanceCalculator : IDistanceCalculator
    {
        private static BitArray CompressString(string str)
        {
            var outStream = new MemoryStream();
            using (var stringStream = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            {
                using (var gZipStream = new GZipStream(outStream, CompressionMode.Compress, true))
                {
                    stringStream.CopyTo(gZipStream);
                }
            }
            outStream.Flush();
            var pos = (int) outStream.Position;
            outStream.Seek(0, SeekOrigin.Begin);

            return new BitArray(outStream.GetBuffer().Take(pos).ToArray());

        }

        public double CalculateDistance(string x, string y) =>
            1.0 * (CompressString(x + y).Length + CompressString(y + x).Length)
               / (CompressString(x + x).Length + CompressString(y + y).Length);
    }
}