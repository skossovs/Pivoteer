using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.PivotCoordinates
{
    public class GeneratedData
    {
        public string[,] Matrix { get; set; }
        public int Column_Hierarchy_Depth { get; set; }
        public int Row_Hierarchy_Depth { get; set; }
        public List<HeaderNode> ColumnHeaderTree { get; set; }
        public List<HeaderNode> RowHeaderTree { get; set; }
    }
}
