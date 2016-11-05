using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clasterization.Interfaces
{
	public interface IClasterizator
	{
		IEnumerable<ITable> Clasterize(ITable table, string keyColumn);
        IEnumerable<ITable> Clasterize(ITable table, int keyColumnNumber);
    }
}
