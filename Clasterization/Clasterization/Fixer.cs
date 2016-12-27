using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Clasterization.Interfaces;

namespace Clasterization.Clasterization
{
    public class Fixer : IFixer
    {
        readonly IClasterizer _clasterizer;

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
            var columnsFromClasters = _clasterizer
                .Clasterize(table, columnId)
                .Select(t => t.GetColumn(columnId).ToList())
                .ToList();

            var dict = CreateReplaceDict(columnsFromClasters);

            var fixedData = FixDataWithDict(table, columnId, dict);

            return new Table(fixedData, table.Header);
        }

        private static List<IList<string>> FixDataWithDict(ITable table, int columnId, Dictionary<string, string> dict)
        {
            var fixedData = new List<IList<string>>();
            foreach (var lst in table.Select(row => row.ToList()))
            {
                lst[columnId] = dict[lst[columnId]];
                fixedData.Add(lst);
            }
            return fixedData;
        }

        private static Dictionary<string, string> CreateReplaceDict(List<List<string>> columnsFromClasters)
        {
            var dict = new Dictionary<string, string>();
            foreach (var column in columnsFromClasters)
            {
                var pop = MostPopular(column);
                foreach (var s in column.Distinct())
                {
                    dict.Add(s, pop);
                }
            }
            return dict;
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