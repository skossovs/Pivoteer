using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Pivot.Accessories.Mapping
{
    public class ValueTypeWrapper<T> where T : class
    {
        private MemberInfo PivotValueGetter;

        public decimal? GetValue(T obj)
        {
            object returnValue = null;
            if (PivotValueGetter.MemberType == MemberTypes.Field)
                returnValue = ((FieldInfo)PivotValueGetter).GetValue(obj);
            else
                throw new Exception("Wrong initialization");
            return (decimal?) returnValue;  // TODO9: Make check for correct type
        }

        public ValueTypeWrapper()
        {
            var type = typeof(T);
            PivotValueGetter = GenerateAttribute(type);
        }

        private MemberInfo GenerateAttribute(Type type)
        {
            MemberInfo[] members = type.GetMembers();
            // Get All necessary attributes and corresponding fields
            var attributeReader = new Reflection.AttributeReader();

            foreach (var member in members)
            {
                var attributesV = attributeReader.GetAttributes<Attributes.IsValues>(type, member);
                if (attributesV.Length > 0)
                    return member;
            }
            throw new Exception("Failed to find Value field");
        }
    }
}
