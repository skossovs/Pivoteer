using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Linq.Expressions;

namespace TDD_PivotStructure.Tests.Attributes
{


    [TestClass]
    public class TDD_FunctionTest
    {
        [TestMethod]
        public void ExtractMethodFromType()
        {
            var lst = new List<decimal?>() { 1, 2 };

            // 1. Reflection. Come up with function
            var input = Expression.Parameter(typeof(IEnumerable<decimal?>), "input");

            var methodStatic = typeof(DataStructures.AggregationFunctions).GetMethod("Sum");

            var methodCallExpression = Expression.Call(methodStatic, input);

            var summaryFunction = Expression.Lambda<Func<IEnumerable<decimal?>, decimal?>>(methodCallExpression, input)
                            .Compile();

            // 2. Testing summary function
            decimal? result = summaryFunction.Invoke(lst);

            Assert.AreEqual(3, result);
        }
    }
}
