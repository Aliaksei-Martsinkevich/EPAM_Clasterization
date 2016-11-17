using System.Collections.Generic;
using System.Linq;
using Clasterization.Clasterization.Algorythms;
using Clasterization.Interfaces;

namespace Clasterization.Clasterization
{
    public class Clasterizer : IClasterizer
    {
        private readonly IEqualityComparer<string> _comparer;

        public Clasterizer(IEqualityComparer<string> comparer)
        {
            _comparer = comparer;
        }

        public IEnumerable<ITable> Clasterize(ITable table, string keyColumn)
        {
            return Clasterize(table, table.Header.IndexOf(keyColumn));
        }

        public IEnumerable<ITable> Clasterize(ITable table, int keyColumnNumber)
        {
            return table.GroupBy(keySelector: row => row.ElementAt(keyColumnNumber),
                                 comparer: _comparer)
                        .Select(groop => new Table(groop.Select(x => x.ToList() as IList<string>)
                                                         .ToList(), table.Header))
                        .Cast<ITable>()
                        .ToList();
        }
    }
}