using System.Collections.Generic;
using System.Linq;
using Clasterization.Clasterization.Algorythms;
using Clasterization.Interfaces;

namespace Clasterization.Clasterization.NeatrestNeighbour
{
    public class NearestNeghbourClasterizator : IClasterizator
    {
        private readonly IDistanceCalculator _distanceCalculator;

        public NearestNeghbourClasterizator(IDistanceCalculator distanceCalculator)
        {
            _distanceCalculator = distanceCalculator;
        }

        public IEnumerable<ITable> Clasterize(ITable table, string keyColumn)
            => Clasterize(table, table.Header.IndexOf(keyColumn));

        public IEnumerable<ITable> Clasterize(ITable table, int keyColumnNumber)
        {
            return table.GroupBy(keySelector: row => row.ElementAt(keyColumnNumber),
                                 comparer: new StringComparer(_distanceCalculator, 3))
                        .Select(groop => new Table(groop.Select(x => x.ToList() as IList<string>)
                                                         .ToList(), table.Header))
                        .Cast<ITable>()
                        .ToList();
        }
    }
}