using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Reflection
{
    internal static class RuntimeReflectionExtensions
    {
        public static IEnumerable<MethodInfo> GetRuntimeMethods(this Type type) =>
            type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        public static IEnumerable<PropertyInfo> GetRuntimeProperties(this Type type) =>
             type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        public static FieldInfo GetRuntimeField(this Type type, string name) =>
             type.GetField(name);

        public static EventInfo GetRuntimeEvent(this Type type, string name) =>
            type.GetEvent(name);

        public static MethodInfo GetRuntimeMethod(this Type type, string name, Type[] parameters) =>
            type.GetMethod(name, parameters);

        public static TypeInfo GetTypeInfo(this Type type)
        {
            return TypeInfo.FromType(type);
        }

        public static IEnumerable<TypeInfo> GetDefinedTypes(this Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            TypeInfo[] array = new TypeInfo[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                TypeInfo typeInfo = types[i].GetTypeInfo();
                array[i] = typeInfo ?? throw new NotSupportedException();
            }

            return array;
        }

        public static bool GetHasDefaultValue(this ParameterInfo info) =>
            info.DefaultValue != DBNull.Value;

        public static bool GetIsConstructedGenericType(this Type type) =>
            type.IsGenericType && !type.IsGenericTypeDefinition;

        public static bool ImplementInterface(this Type type, Type ifaceType)
        {
            while (type != null)
            {
                Type[] interfaces = type.GetInterfaces();
                if (interfaces != null)
                {
                    for (int i = 0; i < interfaces.Length; i++)
                    {
                        if (interfaces[i] == ifaceType || (interfaces[i] != null && interfaces[i].ImplementInterface(ifaceType)))
                        {
                            return true;
                        }
                    }
                }

                type = type.BaseType;
            }

            return false;
        }

        public static Type GetRootElementType(this Type type)
        {
            while (type.HasElementType)
            {
                type = type.GetElementType();
            }

            return type;
        }

        public static Delegate CreateDelegate(this MethodInfo methodInfo, Type delegateType, object target)
        {
            return Delegate.CreateDelegate(delegateType, target, methodInfo);
        }

        public static Delegate CreateDelegate(this MethodInfo methodInfo, Type delegateType)
        {
            return Delegate.CreateDelegate(delegateType, methodInfo);
        }
    }
}
