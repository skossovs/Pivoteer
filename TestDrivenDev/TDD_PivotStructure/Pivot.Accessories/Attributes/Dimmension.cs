using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.Attributes
{
    using Pivot.Accessories.Common;

    [AttributeUsage(AttributeTargets.Field)]
    public class DimmensionX : Attribute
    {
        // Count is default aggregation function
        //private Func<IEnumerable<decimal?>, decimal?> DefaultAggregationFunc = Composition.Compose<int, decimal?>(Enumerable.Count, t => t);
        //private Func<IEnumerable<decimal?>, decimal?> _aggregationFunc;

        public int Level { get; set; }
        public string AggregationFuncName {  get; set;  }

    }

    [AttributeUsage(AttributeTargets.Field)]
    public class DimmensionY : Attribute
    {
        // Count is default aggregation function
        //private Func<IEnumerable<decimal?>, decimal?> DefaultAggregationFunc = Composition.Compose<int, decimal?>(Enumerable.Count, t => t);
        //private Func<IEnumerable<decimal?>, decimal?> _aggregationFunc;

        public int Level { get; set; }
        public string AggregationFuncName { get; set; }
        //public Func<IEnumerable<decimal?>, decimal?> AggregationFunc
        //{
        //    get
        //    {
        //        if (_aggregationFunc == null)
        //            _aggregationFunc = DefaultAggregationFunc;
        //        return _aggregationFunc;
        //    }
        //    set { _aggregationFunc = value; }
        //}

    }

}
