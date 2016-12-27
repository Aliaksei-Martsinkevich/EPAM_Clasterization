using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Clasterization.Interfaces;

namespace Clasterization.Clasterization
{
    public class Fixer : IFixer
    {
        IClasterizer _clasterizer;

        public Fixer(IClasterizer clasterizer)
        {
            _clasterizer = clasterizer;
        }

        public ITable FixTable(ITable table, string columnName)
        {
            var columnId = table.Header.IndexOf(columnName);
            return FixTable(table, columnId);
        }

        public ITable FixTable(ITable table, int columnId)
        {
            var clasters = _clasterizer.Clasterize(table, columnId);
            var columnsFromClasters = clasters.Select(t => t.GetColumn(columnId).ToList()).ToList();
            var dict = new Dictionary<string, string>();
            foreach (var column in columnsFromClasters)
            {
                var pop = MostPopular(column);
                foreach (var s in column.Distinct())
                {
                    dict.Add(s, pop);
                }
            }
            var fixedData = new List<IList<string>>();
            foreach (var lst in table.Select(row => row.ToList()))
            {
                lst[columnId] = dict[lst[columnId]];
                fixedData.Add(lst);
            }
            return new Table(fixedData, table.Header);
        }
        

        private static string MostPopular(IList<string> strings)
        {
            var uniqueStrings = strings.Distinct();
            var count = uniqueStrings.ToDictionary(
                elementSelector: uniqueString => uniqueString, 
                keySelector: uniqueString => strings.Count(x => x.Equals(uniqueString)));
            return count.Max().Value;
        } 
    }
}