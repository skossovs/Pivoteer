using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.PivotCoordinates
{
    public class HeaderNode
    {
        public int Index;
        public int Level;
        public int Length;
        public string Text;
        public List<HeaderNode> Children;
    }
}
