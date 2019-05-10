using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Aggregators : Attribute
    {
        public Type AggregatorType { get; set; }
    }
}
