using System;
using System.Collections.Generic;
using System.Linq;
using Clasterization.Interfaces;

namespace Clasterization.Clasterization.KeyCollision
{
    public class KeyCollisionClasterizator : IClasterizator
    {
        private readonly IKeyCalculator _keyCalculator;

        public KeyCollisionClasterizator(IKeyCalculator keyCalculator)
        {
            _keyCalculator = keyCalculator;
        }

        public IEnumerable<ITable> Clasterize(ITable table, string keyColumn)
        {
            return Clasterize(table, table.Header.IndexOf(keyColumn));
        }

        public IEnumerable<ITable> Clasterize(ITable table, int keyColumnNumber)
        {
            return table.GroupBy(keySelector: row => _keyCalculator.CalculateKey(row.ElementAt(keyColumnNumber)))
                        .Select(groop => new Table(groop.Select(x => x.ToList() as IList<string>)
                                                        .ToList(), table.Header))
                        .Cast<ITable>()
                        .ToList();
        }
    }
}