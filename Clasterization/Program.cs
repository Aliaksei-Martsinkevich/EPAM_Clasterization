using System;
using System.IO;
using System.Linq;
using Clasterization.Clasterization;
using Clasterization.Clasterization.Algorythms;
using Clasterization.Clasterization.Algorythms.KeyCollision;
using Clasterization.Clasterization.Algorythms.NeatrestNeighbour;
using Clasterization.Interfaces;
using Clasterization.IO;


namespace Clasterization
{
    internal class Program
    {
        private static FileInfo _fileinfo;
        private static IClasterizer _method;
        private static int _headerNumber;
        private static CommandsConfiguration _configuration;

        private static void Initialize(CommandsConfiguration configuration)
        {
            _fileinfo = new FileInfo(configuration.Filename);
            _headerNumber = configuration.TargetColumn;
            switch (configuration.Algorythm)
            {
                case "fingerprint":
                default:
                    _method = new Clasterizer(
                        new KeyCollisionStringComparer(
                            new FingerprintKeyCalculator()));
                    break;
                case "ngram":
                    _method = new Clasterizer(
                        new KeyCollisionStringComparer(
                            new NGramFingerprintKeyCalculator()));
                    break;
                case "phonetic":
                    _method = new Clasterizer(
                        new KeyCollisionStringComparer(
                            new PhoneticFingerprintKeyCalculator()));
                    break;
                case "leivenstein":
                    _method = new Clasterizer(
                        new DistanceStringComparer(
                            new LeivensteinDistanceCalculator()));
                    break;
                case "ppm":
                    _method = new Clasterizer(
                        new DistanceStringComparer(
                            new PpmDistanceCalculator()));
                    break;
            }
        }

        private static void Main(string[] args)
        {
            _configuration = Args.Configuration.Configure<CommandsConfiguration>().CreateAndBind(args);
            Initialize(_configuration);
            var reader = new Reader();
            var table = reader.ReadTable(_fileinfo.FullName);

            var writer = new Writer();
            var outputDirectory = _fileinfo.Directory.CreateSubdirectory($"{_fileinfo.Name.Split('.')[0]}_{_configuration.Algorythm}_'{table.Header[_headerNumber]}'");
            var number = 0;
            foreach (var claseters in _method.Clasterize(table, _headerNumber).Where(claseters => claseters.Count() > 1))
            {
                writer.Write(claseters, $"{outputDirectory.FullName}//{++number}.csv");
            }
        }
    }
}
