using System.Collections.Generic;

namespace Clasterization.Interfaces
{
	public interface ITable : IEnumerable<IEnumerable<string>>
	{
		IEnumerable<string> GetRow(int index);
		IEnumerable<string> GetColumn(int index);
		IEnumerable<string> GetColumn(string header);

		IList<string> Header { get; } 
	}
}
