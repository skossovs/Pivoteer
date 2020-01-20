using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListToGrid
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int XSpan { get; set; }
        public int YSpan { get; set; }
        public string Value { get; set; }
    }
}
