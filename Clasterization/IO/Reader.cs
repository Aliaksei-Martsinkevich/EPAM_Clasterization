using System.Collections.Generic;
using System.IO;
using System.Linq;
using Clasterization.Interfaces;
using CsvHelper;

namespace Clasterization.IO
{
    internal class Reader : IReader
    {
        public ITable ReadTable(string filename)
        {
            IList<IList<string>> values;
            string[] header;
            using (var reader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                var parser = new CsvParser(reader);

                header = parser.Read();
                values = new List<IList<string>>();
                while (true)
                {
                    var strings = parser.Read();
                    if (strings == null)
                    {
                        break;
                    }
                    values.Add(strings.ToList());
                }
            }
            return new Table(values, header);
        }
    }
}
