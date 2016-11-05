using System;
using System.Collections;
using System.IO;
using System.Text;
using Clasterization.Interfaces;
using LZW_LIB;

namespace Clasterization.Clasterization.NeatrestNeighbour
{
    public class PpmDistanceCalculator : IDistanceCalculator
    {
        private static BitArray CompressString(string str)
        {
            var lzw = new LZWCompressor();
            var reader = new BinaryReader(new MemoryStream(Encoding.UTF8.GetBytes(str)));
            var writer = new BinaryWriter(new MemoryStream());

            lzw.BrInput = reader;
            lzw.BwOutput = writer;

            lzw.Compress();

            reader.Close();

            lzw.BwOutput.Flush();
            lzw.BwOutput.Seek(0, SeekOrigin.Begin);

            BitArray result;
            using (var resulReader = new BinaryReader(lzw.BwOutput.BaseStream))
            {
                result = new BitArray(resulReader.ReadBytes(Convert.ToInt32(resulReader.BaseStream.Length)));
                resulReader.Close();
            }
            writer.Close();
            return result;
        }


        public double CalculateDistance(string x, string y) =>
            1.0 * (CompressString(x + y).Length + CompressString(y + x).Length)
               / (CompressString(x + x).Length + CompressString(y + y).Length);
    }
}