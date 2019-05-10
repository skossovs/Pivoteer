using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;


namespace Pivot.Accessories.Extensions
{
  //  using Expressions;

    public static class ReflectionExtensions
    {
        #region Type extensions

        public static bool IsGenericTypeEx(this Type type)
        {
#if NETSTANDARD1_6
			return type.GetTypeInfo().IsGenericType;
#else
            return type.IsGenericType;
#endif
        }

        public static bool IsInterfaceEx(this Type type)
        {
#if NETSTANDARD1_6
			return type.GetTypeInfo().IsInterface;
#else
            return type.IsInterface;
#endif
        }

        public static Type BaseTypeEx(this Type type)
        {
#if NETSTANDARD1_6
			return type.GetTypeInfo().BaseType;
#else
            return type.BaseType;
#endif
        }

        public static Type[] GetInterfacesEx(this Type type)
        {
            return type.GetInterfaces();
        }

        public static object[] GetCustomAttributesEx(this Type type, Type attributeType, bool inherit)
        {
#if NETSTANDARD1_6
			return type.GetTypeInfo().GetCustomAttributes(attributeType, inherit).Cast<object>().ToArray();
#else
            return type.GetCustomAttributes(attributeType, inherit);
#endif
        }


        public static bool IsAssignableFromEx(this Type type, Type c)
        {
            return type.IsAssignableFrom(c);
        }

        public static Type[] GetGenericArgumentsEx(this Type type)
        {
            return type.GetGenericArguments();
        }

        public static object[] GetCustomAttributesEx(this Type type, bool inherit)
        {
#if NETSTANDARD1_6
			return type.GetTypeInfo().GetCustomAttributes(inherit).Cast<object>().ToArray();
#else
            return type.GetCustomAttributes(inherit);
#endif
        }

        public static InterfaceMapping GetInterfaceMapEx(this Type type, Type interfaceType)
        {
#if NETSTANDARD1_6
			return type.GetTypeInfo().GetRuntimeInterfaceMap(interfaceType);
#else
            return type.GetInterfaceMap(interfaceType);
#endif
        }
        

        public static object[] GetCustomAttributesEx(this MemberInfo memberInfo, Type attributeType, bool inherit)
        {
#if NETSTANDARD1_6
			return memberInfo.GetCustomAttributes(attributeType, inherit).Cast<object>().ToArray();
#else
            return memberInfo.GetCustomAttributes(attributeType, inherit);
#endif
        }


        public static bool IsGenericTypeDefinitionEx(this Type type)
        {
#if NETSTANDARD1_6
			return type.GetTypeInfo().IsGenericTypeDefinition;
#else
            return type.IsGenericTypeDefinition;
#endif
        }

        #region Attributes cache

        static readonly ConcurrentDictionary<Type, object[]> _typeAttributesInternal = new ConcurrentDictionary<Type, object[]>();

        static void GetAttributesTreeInternal(List<object> list, Type type)
        {
            var attrs = _typeAttributesInternal.GetOrAdd(type, x => type.GetCustomAttributesEx(false));

            list.AddRange(attrs);

            if (type.IsInterfaceEx())
                return;

            // Reflection returns interfaces for the whole inheritance chain.
            // So, we are going to get some hemorrhoid here to restore the inheritance sequence.
            //
            var interfaces = type.GetInterfacesEx();
            var nBaseInterfaces = type.BaseTypeEx() != null ? type.BaseTypeEx().GetInterfacesEx().Length : 0;

            for (var i = 0; i < interfaces.Length; i++)
            {
                var intf = interfaces[i];

                if (i < nBaseInterfaces)
                {
                    var getAttr = false;

                    foreach (var mi in type.GetInterfaceMapEx(intf).TargetMethods)
                    {
                        // Check if the interface is reimplemented.
                        //
                        if (mi.DeclaringType == type)
                        {
                            getAttr = true;
                            break;
                        }
                    }

                    if (getAttr == false)
                        continue;
                }

                GetAttributesTreeInternal(list, intf);
            }

            if (type.BaseTypeEx() != null && type.BaseTypeEx() != typeof(object))
                GetAttributesTreeInternal(list, type.BaseTypeEx());
        }

        #endregion
        
        /// <summary>
        /// Determines whether the <paramref name="type"/> derives from the specified <paramref name="check"/>.
        /// </summary>
        /// <remarks>
        /// This method also returns false if <paramref name="type"/> and the <paramref name="check"/> are equal.
        /// </remarks>
        /// <param name="type">The type to test.</param>
        /// <param name="check">The type to compare with. </param>
        /// <returns>
        /// true if the <paramref name="type"/> derives from <paramref name="check"/>; otherwise, false.
        /// </returns>
        internal static bool IsSubClassOf( this Type type,  Type check)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (check == null) throw new ArgumentNullException(nameof(check));

            if (type == check)
                return false;

            while (true)
            {
                if (check.IsInterfaceEx())
                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (var interfaceType in type.GetInterfaces())
                        if (interfaceType == check || interfaceType.IsSubClassOf(check))
                            return true;

                if (type.IsGenericTypeEx() && !type.IsGenericTypeDefinitionEx())
                {
                    var definition = type.GetGenericTypeDefinition();
                    if (definition == check || definition.IsSubClassOf(check))
                        return true;
                }

                type = type.BaseTypeEx();

                if (type == null)
                    return false;

                if (type == check)
                    return true;
            }
        }

        public static Type GetGenericType( this Type genericType, Type type)
        {
            if (genericType == null) throw new ArgumentNullException("genericType");

            while (type != null && type != typeof(object))
            {
                if (type.IsGenericTypeEx() && type.GetGenericTypeDefinition() == genericType)
                    return type;

                if (genericType.IsInterfaceEx())
                {
                    foreach (var interfaceType in type.GetInterfacesEx())
                    {
                        var gType = GetGenericType(genericType, interfaceType);

                        if (gType != null)
                            return gType;
                    }
                }

                type = type.BaseTypeEx();
            }

            return null;
        }

        public static Type GetItemType(this Type type)
        {
            if (type == null)
                return null;

            if (type == typeof(object))
                return type.HasElementType ? type.GetElementType() : null;

            if (type.IsArray)
                return type.GetElementType();

            if (type.IsGenericTypeEx())
                foreach (var aType in type.GetGenericArgumentsEx())
                    if (typeof(IEnumerable<>).MakeGenericType(new[] { aType }).IsAssignableFromEx(type))
                        return aType;

            var interfaces = type.GetInterfacesEx();

            if (interfaces != null && interfaces.Length > 0)
            {
                foreach (var iType in interfaces)
                {
                    var eType = iType.GetItemType();

                    if (eType != null)
                        return eType;
                }
            }

            return type.BaseTypeEx().GetItemType();
        }


        #endregion

        public static int AttributeCount<T> ( Type objectType) where T: Attribute
        {
            MemberInfo[] members = objectType.GetMembers();
            var attributeReader = new Reflection.AttributeReader();
            return members.Count(t =>
                attributeReader
                .GetAttributes<T>(objectType, t)
                .Length > 0);
        }
    }
}