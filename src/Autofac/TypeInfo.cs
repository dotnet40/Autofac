using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if NET40
namespace System.Reflection
{
    using System.Collections.Concurrent;
    using System.Globalization;

    internal class TypeInfo
    {
        public static readonly ConcurrentDictionary<Type, TypeInfo> _typeInfoCache = new ConcurrentDictionary<Type, TypeInfo>();

        public static TypeInfo FromType(Type type)
        {
            return _typeInfoCache.GetOrAdd(type, t => new TypeInfo(t));
        }

        public IEnumerable<Type> ImplementedInterfaces => _type.GetInterfaces();

        public Type[] GenericTypeParameters
        {
            get
            {
                if (_type.IsGenericTypeDefinition)
                    return _type.GetGenericArguments();
                return Type.EmptyTypes;
            }
        }

        public Type[] GenericTypeArguments
        {
            get
            {
                if (_type.IsGenericType && !_type.IsGenericTypeDefinition)
                    return _type.GetGenericArguments();
                return Type.EmptyTypes;
            }
        }

        public virtual IEnumerable<ConstructorInfo> DeclaredConstructors =>
            _type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        public virtual IEnumerable<MemberInfo> DeclaredMembers =>
            _type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        public virtual IEnumerable<MethodInfo> DeclaredMethods =>
            _type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        public virtual IEnumerable<PropertyInfo> DeclaredProperties =>
            _type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        public virtual Guid GUID => _type.GUID;

        public virtual Module Module => _type.Module;

        public virtual Assembly Assembly => _type.Assembly;

        public virtual string FullName => _type.FullName;

        public virtual string Namespace => _type.Namespace;

        public virtual string AssemblyQualifiedName => _type.AssemblyQualifiedName;

        public virtual Type BaseType => _type.BaseType;

        public virtual Type UnderlyingSystemType => _type.UnderlyingSystemType;

        public virtual string Name => _type.Name;

        public bool IsInterface => _type.IsInterface;

        public bool IsGenericParameter => _type.IsGenericParameter;

        public bool IsValueType => _type.IsValueType;

        public bool IsGenericType => _type.IsGenericType;

        public bool IsAbstract => _type.IsAbstract;

        public bool IsClass => _type.IsClass;

        public bool IsEnum => _type.IsEnum;

        public bool IsGenericTypeDefinition => _type.IsGenericTypeDefinition;

        public bool IsSealed => _type.IsSealed;

        public bool IsPrimitive => _type.IsPrimitive;

        public virtual GenericParameterAttributes GenericParameterAttributes => _type.GenericParameterAttributes;

        private readonly Type _type;

        protected TypeInfo(Type type)
        {
            _type = type;
        }

        public Type AsType() => _type;

        public virtual PropertyInfo GetDeclaredProperty(string name) =>
            _type.GetProperty(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        public virtual object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters) =>
            _type.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);

        public virtual ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) =>
            _type.GetConstructors(bindingAttr);

        public virtual MethodInfo[] GetMethods(BindingFlags bindingAttr) =>
            _type.GetMethods(bindingAttr);

        public virtual FieldInfo GetField(string name, BindingFlags bindingAttr) =>
            _type.GetField(name, bindingAttr);

        public virtual FieldInfo[] GetFields(BindingFlags bindingAttr) =>
            _type.GetFields(bindingAttr);

        public virtual Type GetInterface(string name, bool ignoreCase) =>
            _type.GetInterface(name, ignoreCase);

        public virtual Type[] GetInterfaces() =>
            _type.GetInterfaces();

        public virtual EventInfo GetEvent(string name, BindingFlags bindingAttr) =>
            _type.GetEvent(name, bindingAttr);

        public virtual EventInfo[] GetEvents(BindingFlags bindingAttr) =>
            _type.GetEvents(bindingAttr);

        public virtual PropertyInfo[] GetProperties(BindingFlags bindingAttr) =>
            _type.GetProperties(bindingAttr);

        public virtual Type[] GetNestedTypes(BindingFlags bindingAttr) =>
            _type.GetNestedTypes(bindingAttr);

        public virtual Type GetNestedType(string name, BindingFlags bindingAttr) =>
            _type.GetNestedType(name, bindingAttr);

        public virtual MemberInfo[] GetMembers(BindingFlags bindingAttr) =>
            _type.GetMembers(bindingAttr);

        public virtual Type GetElementType() =>
            _type.GetElementType();

        public virtual object[] GetCustomAttributes(bool inherit) =>
            _type.GetCustomAttributes(inherit);

        public virtual object[] GetCustomAttributes(Type attributeType, bool inherit) =>
            _type.GetCustomAttributes(attributeType, inherit);

        public virtual bool IsDefined(Type attributeType, bool inherit) =>
            _type.IsDefined(attributeType, inherit);

        public virtual bool IsSubclassOf(Type c) =>
            _type.IsSubclassOf(c);

        public virtual Type[] GetGenericParameterConstraints() =>
            _type.GetGenericParameterConstraints();

        public virtual bool IsAssignableFrom(TypeInfo typeInfo)
        {
            if (typeInfo == null)
            {
                return false;
            }

            if (this == typeInfo)
            {
                return true;
            }

            if (typeInfo.IsSubclassOf(_type))
            {
                return true;
            }

            if (IsInterface)
            {
                return typeInfo._type.ImplementInterface(_type);
            }

            if (this.IsGenericParameter)
            {
                Type[] genericParameterConstraints = this.GetGenericParameterConstraints();
                for (int i = 0; i < genericParameterConstraints.Length; i++)
                {
                    if (!genericParameterConstraints[i].IsAssignableFrom(typeInfo._type))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public virtual bool ContainsGenericParameters
        {
            get
            {
                if (_type.HasElementType)
                {
                    return _type.GetRootElementType().ContainsGenericParameters;
                }

                if (this.IsGenericParameter)
                {
                    return true;
                }

                if (!this.IsGenericType)
                {
                    return false;
                }

                Type[] genericArguments = _type.GetGenericArguments();
                for (int i = 0; i < genericArguments.Length; i++)
                {
                    if (genericArguments[i].ContainsGenericParameters)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public virtual MethodInfo GetDeclaredMethod(string name)
        {
            return _type.GetMethod(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }
    }
}
#endif