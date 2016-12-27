using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Clasterization.Clasterization;
using Clasterization.Clasterization.Algorythms.KeyCollision;
using Clasterization.Clasterization.Algorythms.NeatrestNeighbour;
using Clasterization.Interfaces;
using Clasterization.IO;

//Investments title         11
//            description   12
//Contarcts   vendor name   16
//            investment title  4
//            investment description
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
                    _method = new Clasterizer(new FingerprintStringComparer());
                    break;
                case "ngram":
                    _method = new Clasterizer(new NGramStringComparer());
                    break;
                case "phonetic":
                    _method = new Clasterizer(new PhoneticStringComparer());
                    break;
                case "leivenstein":
                    _method = new Clasterizer(new LeivensteinStringComparer(555D));
                    break;
                case "ppm":
                    _method = new Clasterizer(new PpmStringComparer(1D));
                    break;
            }
        }

        private static void Main(string[] args)
        {
            _configuration = Args.Configuration.Configure<CommandsConfiguration>().CreateAndBind(args);

            Initialize(_configuration);

            var reader = new Reader();
            var table = reader.ReadTable(_fileinfo.FullName);

            for (_headerNumber = 0; _headerNumber < table.Header.Count; _headerNumber++)
            {
                var writer = new Writer();
                var outputDirectory = _fileinfo.Directory.CreateSubdirectory(
                    $"Results_{DateTime.Now.ToString("HH_mm DD_MM")}//{_fileinfo.Name.Split('.')[0]}_{_configuration.Algorythm}_'{table.Header[_headerNumber]}'");
                var number = 0;

                var clastersList = _method.Clasterize(table, _headerNumber).ToList();
                var notEmptyClasters = clastersList.Where(claseters => claseters.Count() > 1).ToList();

                foreach (var claster in notEmptyClasters)
                {
                    var dist = claster.GetColumn(_headerNumber).Distinct().Count() > 1 
                        ? string.Empty
                        : "//UniqueDir";
                    writer.Write(claster, $"{outputDirectory.FullName}" +
                                          $"{dist}" +
                                          $"//{++number}.csv");
                }
            }
        }
    }
}
