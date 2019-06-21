using System;
using System.Reflection;



namespace Pivot.Accessories.Reflection
{
    using Extensions;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class AttributeReader
    {
        public T[] GetAttributes<T>(Type type, bool inherit = true)
            where T : Attribute
        {
            var attrs = type.GetCustomAttributesEx(typeof(T), inherit);
            var arr = new T[attrs.Length];

            for (var i = 0; i < attrs.Length; i++)
                arr[i] = (T)attrs[i];

            return arr;
        }

        public T[] GetAttributes<T>(Type type, MemberInfo memberInfo, bool inherit = true)
            where T : Attribute
        {
            var attrs = memberInfo.GetCustomAttributesEx(typeof(T), inherit);
            var arr = new T[attrs.Length];

            for (var i = 0; i < attrs.Length; i++)
                arr[i] = (T)attrs[i];

            return arr;
        }

    }
}