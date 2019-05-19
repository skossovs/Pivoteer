using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories
{
    public class MatrixManipulator
    {
        public Func<List<decimal?>, decimal?>[]        AggregationFunctionVector; // this is the right way to implement aggregation
        public Func  <int     , decimal?>              getValue; // second coordinate changes when traversing through index
        public Action<int     , decimal?>              setValue; // second coordinate changes when traversing through index
    }
}
