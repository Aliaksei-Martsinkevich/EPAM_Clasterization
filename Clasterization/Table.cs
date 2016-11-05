using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Clasterization.Interfaces;

namespace Clasterization
{
	internal class Table : ITable
	{
		private readonly IList<IList<string>> _table;


		public IList<string> Header { get; }

		public Table(IList<IList<string>> data, IList<string> header)
		{
			Header = header;
			_table = data;
		}

		public IEnumerable<string> GetRow(int index)
		{
			return _table.ElementAt(index).AsEnumerable();
		}     

		public IEnumerable<string> GetColumn(int index)
		{
			return _table.Select(row => row.ElementAt(index)).ToList();
		}

		public IEnumerable<string> GetColumn(string header)
		{
			return GetColumn(header.IndexOf(header, StringComparison.Ordinal));
		}

		public IEnumerator<IEnumerable<string>> GetEnumerator()
		{
			return _table.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
