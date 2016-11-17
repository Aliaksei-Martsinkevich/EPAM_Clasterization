using System.Collections.Generic;

namespace Clasterization.Interfaces
{
	public interface IClasterizer
	{
		IEnumerable<ITable> Clasterize(ITable table, string keyColumn);
        IEnumerable<ITable> Clasterize(ITable table, int keyColumnNumber);
    }
}
