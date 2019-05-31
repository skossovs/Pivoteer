using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.Mapping
{
    public class TypeWrapper<T, TAggregator>
        where T:class
        where TAggregator:class
    {
        public XTypeWrapper<T, TAggregator>     XType;
        public YTypeWrapper<T, TAggregator>     YType;
        public ValueTypeWrapper<T> VType;

        public TypeWrapper()
        {
            XType = new XTypeWrapper    <T, TAggregator>();
            YType = new YTypeWrapper    <T, TAggregator>();
            VType = new ValueTypeWrapper<T>();
        }
    }
}
