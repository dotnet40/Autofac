using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Reflection
{
    internal static class CustomAttributeExtensions
    {
        public static T GetCustomAttribute<T>(this MemberInfo element, bool inherit)
            where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(element, typeof(T), inherit);
        }

        public static T GetCustomAttribute<T>(this TypeInfo element, bool inherit)
            where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(element.AsType(), typeof(T), inherit);
        }

        public static T GetCustomAttribute<T>(this TypeInfo element)
            where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(element.AsType(), typeof(T));
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this TypeInfo element)
            where T : Attribute
        {
            return Attribute.GetCustomAttributes(element.AsType(), typeof(T)).Cast<T>();
        }

        public static T GetCustomAttribute<T>(this ParameterInfo element)
            where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(element, typeof(T));
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this ParameterInfo element, bool inherit)
            where T : Attribute
        {
            return Attribute.GetCustomAttributes(element, typeof(T), inherit).Cast<T>();
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this PropertyInfo element)
            where T : Attribute
        {
            return Attribute.GetCustomAttributes(element, typeof(T)).Cast<T>();
        }
    }
}
