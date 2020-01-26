using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.src.Extensions
{
    public static class IndicesExtensions
    {
        public static int InverseIndex(int maxValue, int index)
        {
            return maxValue - index - 1;
        }
    }
}
