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


            private delegate TMember Resolver<TMember>(Type declaringType, string memberName, Type[] parameterTypes, BindingFlags flags) where TMember : MemberInfo;

            public const BindingFlags DEFAULT_FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            private static readonly Dictionary<(Type, string, Type[]), MemberInfo> cache = new();


            private static T FetchInfo<T>(Type type, string name, Type[] parameterTypes, BindingFlags flags, Resolver<T> resolver) where T : MemberInfo
            {
                  if (type == null) throw new ArgumentNullException(nameof(type));
                  if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Member name cannot be null or empty.", nameof(name));

                  (Type Type, string Name, Type[] Parameters) key = (type, name, parameterTypes);
                  if (!cache.TryGetValue(key, out MemberInfo member))
                  {
                        Type current = key.Type;
                        while (current != null)
                        {
                              member = resolver(current, key.Name, key.Parameters, flags);
                              if (member != null)
                              {
                                    cache[key] = member;
                                    break;
                              }
                              current = current.BaseType;
                        }
                  }
                  return member as T;
            }

            public static Field FindField(string name, Type type, BindingFlags flags = DEFAULT_FLAGS) => FetchInfo(type, name, null, flags, static (t, n, _, f) => t.GetField(n, f));
            public static Property FindProperty(string name, Type type, BindingFlags flags = DEFAULT_FLAGS) => FetchInfo(type, name, null, flags, static (t, n, _, f) => t.GetProperty(n, f));
            public static Method FindMethod(string name, Type type, Type[] parameterTypes = null, BindingFlags flags = DEFAULT_FLAGS) => FetchInfo(type, name, parameterTypes, flags, static (t, n, p, f) => p == null ? t.GetMethod(n, f) : t.GetMethod(n, f, null, p, null));
            public static MemberInfo FindMember(string name, Type type, Type[] parameterTypes = null, BindingFlags flags = DEFAULT_FLAGS, MemberTypes targetMembers = MemberTypes.All)
            {
                  if (string.IsNullOrWhiteSpace(name) || type == null) return null;
                  if (targetMembers.HasFlag(MemberTypes.Field) && FindField(name, type, flags) is { } f) return f;
                  if (targetMembers.HasFlag(MemberTypes.Property) && FindProperty(name, type, flags) is { } p) return p;
                  if (targetMembers.HasFlag(MemberTypes.Method) && FindMethod(name, type, parameterTypes, flags) is { } m) return m;
                  Debug.Log($"Unable to find member '{name}' in {type.FullName}. Verify the member name and it's accessibility.");
                  return null;
            }

            public static object ReadField(string name, object target, BindingFlags flags = DEFAULT_FLAGS) => FindField(name, target.GetType(), flags) is { } obj ? obj.GetValue(target) : null;
            public static object ReadProperty(string name, object target, BindingFlags flags = DEFAULT_FLAGS) => FindProperty(name, target.GetType(), flags) is { } obj && obj.CanRead ? obj.GetValue(target) : null;
            public static object InvokeMethod(string name, object target, object[] parameters = null, BindingFlags flags = DEFAULT_FLAGS) => FindMethod(name, target.GetType(), parameters?.Select(p => p?.GetType() ?? typeof(object)).ToArray(), flags) is { } obj ? obj.Invoke(target, parameters) : null;
            public static object ReadAny(string name, object target, object[] parameters = null, BindingFlags flags = DEFAULT_FLAGS, MemberTypes memberTypes = MemberTypes.All)
            {
                  if (string.IsNullOrWhiteSpace(name) || target == null) return null;
                  if (memberTypes.HasFlag(MemberTypes.Field) && ReadField(name, target, flags) is { } f) return f;
                  if (memberTypes.HasFlag(MemberTypes.Property) && ReadProperty(name, target, flags) is { } p) return p;
                  if (memberTypes.HasFlag(MemberTypes.Method) && InvokeMethod(name, target, parameters, flags) is { } m) return m;
                  Debug.Log($"Unable to find member '{name}' in {target.GetType().FullName}. Verify the member name and it's accessibility.");
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
      }
}