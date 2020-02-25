using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.src.Extensions
{
    internal static class MappingUtilsExtensions
    {
        public static Func<IEnumerable<decimal?>, decimal?> ExtractAggregationMethod(Type t, string methodName)
        {
            var inputList = Expression.Parameter(typeof(IEnumerable<decimal?>), "inputList");
            var methodStatic = t.GetMethod(methodName);
            var methodCallExpression = Expression.Call(methodStatic, inputList);
            var aggregationFunction = Expression.Lambda<Func<IEnumerable<decimal?>, decimal?>>(methodCallExpression, inputList)
                            .Compile();
            return aggregationFunction;
        }

    }
}
