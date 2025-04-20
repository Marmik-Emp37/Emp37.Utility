using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Emp37.Utility
{
      using Field = FieldInfo;
      using Property = PropertyInfo;
      using Method = MethodInfo;
      using Parameter = ParameterInfo;

      public static class ReflectionUtility
      {
            [Flags]
            public enum MemberTypes
            {
                  Field = 1 << 0, Property = 1 << 1, Method = 1 << 2,
                  All = Field | Property | Method
            }


            private delegate TMember FetchHandler<TMember>(Type declaringType, string memberName, Type[] parameterTypes, BindingFlags flags) where TMember : MemberInfo;

            public const BindingFlags DEFAULT_FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            private static readonly Dictionary<(Type, string, Type[]), MemberInfo> cache = new();


            private static T FetchInfo<T>(Type type, string name, Type[] parameterTypes, BindingFlags flags, FetchHandler<T> resolver) where T : MemberInfo
            {
                  if (type == null) throw new ArgumentNullException(nameof(type));
                  if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Member name cannot be null or empty.", nameof(name));

                  (Type Type, string Name, Type[] Parameters) key = (type, name, parameterTypes);
                  if (cache.TryGetValue(key, out MemberInfo member)) return member as T;
                  Type current = key.Type;

                  while (current != null)
                  {
                        member = resolver(current, key.Name, key.Parameters, flags);
                        if (member != null)
                        {
                              cache[key] = member;
                              return member as T;
                        }
                        current = current.BaseType;
                  }
                  throw new MissingMemberException($"{typeof(T).Name} '{name}' not found on '{type.FullName}' or its base types.");
            }
            private static bool TryFetchInfo<T>(Type type, string name, Type[] parameterTypes, BindingFlags flags, FetchHandler<T> resolver, out T member) where T : MemberInfo
            {
                  try
                  {
                        member = FetchInfo(type, name, parameterTypes, flags, resolver);
                  }
                  catch (Exception ex) when (ex is ArgumentException || ex is MissingMemberException)
                  {
                        member = null;
                  }
                  return member != null;
            }

            private static Field FieldHandler(Type type, string name, Type[] _, BindingFlags flags) => type.GetField(name, flags);
            private static Property PropertyHandler(Type type, string name, Type[] _, BindingFlags flags) => type.GetProperty(name, flags);
            private static Method MethodHandler(Type type, string name, Type[] parameterTypes, BindingFlags flags) => parameterTypes == null ? type.GetMethod(name, flags) : type.GetMethod(name, flags, null, parameterTypes, null);

            public static Field FetchField(string name, Type type, BindingFlags flags = DEFAULT_FLAGS) => FetchInfo(type, name, null, flags, FieldHandler);
            public static bool TryFetchField(string name, Type type, out Field field, BindingFlags flags = DEFAULT_FLAGS) => TryFetchInfo(type, name, null, flags, FieldHandler, out field);
            public static Property FetchProperty(string name, Type type, BindingFlags flags = DEFAULT_FLAGS) => FetchInfo(type, name, null, flags, PropertyHandler);
            public static bool TryFetchProperty(string name, Type type, out Property property, BindingFlags flags = DEFAULT_FLAGS) => TryFetchInfo(type, name, null, flags, PropertyHandler, out property);
            public static Method FetchMethod(string name, Type type, Type[] parameterTypes = null, BindingFlags flags = DEFAULT_FLAGS) => FetchInfo(type, name, parameterTypes, flags, MethodHandler);
            public static bool TryFetchMethod(string name, Type type, out Method method, Type[] parameterTypes = null, BindingFlags flags = DEFAULT_FLAGS) => TryFetchInfo(type, name, parameterTypes, flags, MethodHandler, out method);
            public static MemberInfo FetchAny(string name, Type type, Type[] parameterTypes = null, BindingFlags flags = DEFAULT_FLAGS, MemberTypes memberTypes = MemberTypes.All)
            {
                  if (string.IsNullOrWhiteSpace(name) || type == null) return null;

                  if (memberTypes.HasFlag(MemberTypes.Field) && TryFetchField(name, type, out Field field, flags)) return field;
                  if (memberTypes.HasFlag(MemberTypes.Property) && TryFetchProperty(name, type, out Property property, flags)) return property;
                  if (memberTypes.HasFlag(MemberTypes.Method) && TryFetchMethod(name, type, out Method method, parameterTypes, flags)) return method;

                  Debug.Log($"Failed to fetch any member named '{name}' in '{type.FullName}'. It may not match the requested member types, be inaccessible or not exist. ");

                  return null;
            }

            public static object ReadField(string name, object target, BindingFlags flags = DEFAULT_FLAGS) => TryFetchField(name, target.GetType(), out Field field, flags) ? field.GetValue(target) : null;
            public static object ReadProperty(string name, object target, BindingFlags flags = DEFAULT_FLAGS) => TryFetchProperty(name, target.GetType(), out Property property, flags) && property.CanRead ? property.GetValue(target) : null;
            public static object InvokeMethod(string name, object target, object[] parameters = null, BindingFlags flags = DEFAULT_FLAGS) => TryFetchMethod(name, target.GetType(), out Method method, parameters?.Select(p => p?.GetType() ?? typeof(object)).ToArray(), flags) ? method.Invoke(target, parameters) : null;
            public static object ReadAny(string name, object target, object[] parameters = null, BindingFlags flags = DEFAULT_FLAGS, MemberTypes memberTypes = MemberTypes.All)
            {
                  if (string.IsNullOrWhiteSpace(name) || target == null) return null;

                  if (memberTypes.HasFlag(MemberTypes.Field) && ReadField(name, target, flags) is { } field) return field;
                  if (memberTypes.HasFlag(MemberTypes.Property) && ReadProperty(name, target, flags) is { } property) return property;
                  if (memberTypes.HasFlag(MemberTypes.Method) && InvokeMethod(name, target, parameters, flags) is { } method) return method;

                  return null;
            }

            public static object AutoInvokeMethod(Method method, object target, string[] argNames = null, BindingFlags flags = DEFAULT_FLAGS)
            {
                  Parameter[] parameters = method.GetParameters();
                  int length = parameters.Length;
                  if (length == 0) return method.Invoke(target, null);

                  Assert(argNames != null && length == argNames.Length, "Argument count mismatch.");
                  object[] values = new object[length];
                  for (int i = 0; i < length; i++)
                  {
                        object value = ReadAny(argNames[i], target, null, flags, MemberTypes.Field | MemberTypes.Property);
                        Assert(value != null, $"Could not resolve value for '{argNames[i]}' from '{target.GetType().FullName}'. The member may not exist or may not be accessible.");
                        Type valueType = value.GetType(), expectedType = parameters[i].ParameterType;
                        Assert(expectedType.IsAssignableFrom(valueType), $"Type mismatch at index {i}. Expected '{expectedType}', received` '{valueType}'.");
                        values[i] = value;
                  }
                  return method.Invoke(target, values);

                  void Assert(bool condition, string message)
                  {
                        if (condition) return;
                        string signature = string.Join(", ", parameters.Select(param => param.ParameterType.Name)), info = $"{method.ReflectedType}.{method.Name}({signature})";
                        throw new ArgumentException($"Invoke failed for method '{info}'.\n-- {message}");
                  }
            }
            public static object AutoInvokeMethod(string name, object target, string[] argNames = null, BindingFlags flags = DEFAULT_FLAGS) => AutoInvokeMethod(FetchMethod(name, target.GetType()), target, argNames, flags);
      }
}