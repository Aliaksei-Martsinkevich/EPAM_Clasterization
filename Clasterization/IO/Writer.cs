using System.IO;
using System.Linq;
using Clasterization.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;

namespace Clasterization.IO
{
	public class Writer : IWriter
	{
		public void Write(ITable table, string filename)
		{
		    using (var writer = new StreamWriter(filename))
		    {
		        var config = new CsvConfiguration {QuoteAllFields = true, QuoteNoFields = false, Quote = '\"'};			
		        var serializer = new CsvSerializer(writer, config);

                //serializer.Write(table.Header.ToArray());

                foreach (var row in table)
                {
                    serializer.Write(row.ToArray());
                }
            }	   
		}
	}
}