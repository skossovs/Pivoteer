using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivoteer.MVVM.Messages
{
    public class CrossTablePopulateMessage
    {
        public CrossTablePopulateMessage(List<Cell> tableCells)
        {
            TableCells = tableCells;
        }

        public List<Cell> TableCells { get; set; }
    }
}
