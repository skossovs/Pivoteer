using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD_PivotStructure.DataStructures
{
    public class AggregationFunctions
    {
        public static decimal? Sum(IEnumerable<decimal?> l)
        {
            decimal? acc = null;
            l.ToList().ForEach(t => acc = (acc ?? 0) + (t ?? 0));
            return acc;
        }
        public static decimal? Count(IEnumerable<decimal?> l)
        {
            decimal? acc = null;
            l.ToList().ForEach(t => acc = (acc ?? 0) + (t != null ? 1 : 0));
            return acc;
        }

        public static decimal? Average(IEnumerable<decimal?> l)
        {
            decimal? count = Count(l);
            if (count > 0)
                return Sum(l) / Count(l);
            else
                return null;
        }
    }
}
