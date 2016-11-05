using System;
using System.IO;
using System.Linq;
using Clasterization.Clasterization.KeyCollision;
using Clasterization.Clasterization.NeatrestNeighbour;
using Clasterization.Interfaces;
using Clasterization.IO;

namespace Clasterization
{
    internal class Program
    {
        private static string _filename;

        private static IClasterizator _method;

        private static void Initialize(string[] args)
        {
            _filename = args[1];
            switch (args[0])
            {
                case "fingerprint":
                default:
                    _method = new KeyCollisionClasterizator(new FingerprintKeyCalculator());
                    break;
                case "ngram":
                    _method = new KeyCollisionClasterizator(new NGramFingerprintKeyCalculator());
                    break;
                case "phonetic":
                    _method = new KeyCollisionClasterizator(new PhoneticFingerprintKeyCalculator());
                    break;
                case "leivenstein":
                    _method = new NearestNeghbourClasterizator(new LeivensteinDistanceCalculator());
                    break;
                case "ppm":
                    _method = new NearestNeghbourClasterizator(new PpmDistanceCalculator());
                    break;
            }
        }

        private static void Main(string[] args)
        {
            Initialize(args);
            var reader = new Reader();
            var table = reader.ReadTable(_filename);

            var writer = new Writer();
            var number = 0;
            foreach (var claseters in _method.Clasterize(table, 15).Where(claseters => claseters.Count() > 1))
            {
                writer.Write(claseters, _filename.Split('.')[0] + ++number + ".csv");
            }
        }
    }
}
