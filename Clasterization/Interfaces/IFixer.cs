using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clasterization.Interfaces
{
    public interface IFixer
    {
        ITable FixTable(ITable table, string columnName);
        ITable FixTable(ITable table, int columnId);
    }
}
