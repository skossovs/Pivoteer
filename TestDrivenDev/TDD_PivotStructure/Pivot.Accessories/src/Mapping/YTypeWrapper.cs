using Pivot.Accessories.Extensions;
using Pivot.Accessories.Reflection;
using Pivot.Accessories.src.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Pivot.Accessories.Mapping
{
    public class YTypeWrapper<T, TAggregator> 
        where T : class
        where TAggregator : class
    {
        private MemberInfo[] PivotFieldGetters;
        private int maxDim = -1;

        public Func<List<decimal?>, decimal?>[] AggregationFunctionsByLevel;
        public int MaxDim => maxDim;
        public int IndexMaxDim => maxDim - 1;

        public string GetField(T obj, int level)
        {
            object returnValue = null;
            if (PivotFieldGetters[level].MemberType == MemberTypes.Field)
                returnValue = ((FieldInfo)PivotFieldGetters[level]).GetValue(obj);
            else
                throw new Exception("Wrong initialization");

            if (returnValue.GetType().Name == "String")
                return (string)returnValue;
            else
                throw new Exception($"Wrong type has been provided {returnValue.GetType().Name}");
        }

        public YTypeWrapper()
        {
            var type = typeof(T);

            maxDim = ReflectionExtensions.AttributeCount<Attributes.DimmensionY>(type);
            PivotFieldGetters = new MemberInfo[maxDim];
            AggregationFunctionsByLevel = new Func<List<decimal?>, decimal?>[maxDim];

            foreach (var t in GenerateAttributeList(type))
            {
                PivotFieldGetters[t.Item1.Level] = t.Item2;
                AggregationFunctionsByLevel[t.Item1.Level] = MappingUtilsExtensions.ExtractAggregationMethod(typeof(TAggregator), t.Item1.AggregationFuncName);
            }

        }
        private IEnumerable<Tuple<Attributes.DimmensionY, MemberInfo>> GenerateAttributeList(Type type)
        {
            MemberInfo[] members = type.GetMembers();
            // Get All necessary attributes and corresponding fields
            var attributeReader = new AttributeReader();

            foreach (var member in members)
            {
                var attributesY = attributeReader.GetAttributes<Attributes.DimmensionY>(type, member);
                if (attributesY.Length > 0)
                    yield return Tuple.Create(attributesY[0], member); // add attribute itself and field name
            }
        }
    }
}
