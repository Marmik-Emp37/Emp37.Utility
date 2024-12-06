using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace Emp37.Utility
{
      public static class ReflectionUtility
      {
            [Flags]
            public enum MemberTypes
            {
                  Field = 1,
                  Property = 2,
                  Method = 4,
                  All = 7,
            }

            public const BindingFlags ReflectionFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            private static readonly Dictionary<(Type, string), MemberInfo> cachedInfo = new();

            public static T FetchInfo<T>(string name, Type type, BindingFlags bindings = ReflectionFlags) where T : MemberInfo
            {
                  if (type == null) throw new ArgumentNullException(nameof(type), "The provided type cannot be null.");
                  if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Member name cannot be empty.", nameof(name));

                  (Type, string) key = (type, name);
                  if (!cachedInfo.TryGetValue(key, out MemberInfo member))
                  {
                        Type memberType = typeof(T);
                        while (type != null)
                        {
                              member = memberType switch
                              {
                                    Type when memberType == typeof(FieldInfo) => type.GetField(name, bindings),
                                    Type when memberType == typeof(PropertyInfo) => type.GetProperty(name, bindings),
                                    Type when memberType == typeof(MethodInfo) => type.GetMethod(name, bindings),
                                    _ => throw new NotSupportedException($"Unsupported member type: {memberType.FullName}")
                              };
                              if (member != null)
                              {
                                    cachedInfo[key] = member;
                                    break;
                              }
                              type = type.BaseType;
                        }
                  }
                  return member as T;
            }
            public static bool TryFetchInfo<T>(string name, Type type, out T value, BindingFlags bindings = ReflectionFlags) where T : MemberInfo
            {
                  try
                  {
                        value = FetchInfo<T>(name, type, bindings);
                  }
                  catch (Exception ex)
                  {
                        value = null;

                        Debug.LogWarning(ex.Message);
                  }
                  return value != null;
            }
            public static object FetchValue(string name, object target, MemberTypes enabled = MemberTypes.All, BindingFlags bindings = ReflectionFlags, params object[] parameters)
            {
                  if (target != null)
                  {
                        Type type = target.GetType();
                        if (enabled.HasFlag(MemberTypes.Field) && TryFetchInfo(name, type, out FieldInfo field, bindings)) return field.GetValue(target);
                        if (enabled.HasFlag(MemberTypes.Property) && TryFetchInfo(name, type, out PropertyInfo property, bindings) && property.CanRead) return property.GetValue(target);
                        if (enabled.HasFlag(MemberTypes.Method) && TryFetchInfo(name, type, out MethodInfo method, bindings)) return method.Invoke(target, parameters);
                  }
                  return null;
            }
            public static object InvokeMethod(MethodInfo method, object target, string[] args = null, BindingFlags bindings = ReflectionFlags)
            {
                  List<object> values = new();
                  ParameterInfo[] parameters = method.GetParameters();

                  if (parameters.Length > 0)
                  {
                        Assert(args != null && parameters.Length == args.Length, "Number of parameters specified does not match the expected number.");

                        for (byte i = 0; i < parameters.Length; i++)
                        {
                              object value = FetchValue(args[i], target, MemberTypes.Field | MemberTypes.Property, bindings);

                              Assert(value != null, $"Unable to fetch value for '{args[i]}' in type '{target.GetType().FullName}'. The member may not exist or may not be accessible.");

                              Type parameterType = value.GetType(), expectedType = parameters[i].ParameterType;

                              Assert(expectedType == parameterType, $"Parameter type mismatch at index {i}. Expected type '{expectedType}' but received '{parameterType}'.");

                              values.Add(value);
                        }

                        void Assert(bool condition, string message)
                        {
                              if (condition) return;

                              string signature = parameters.Length > 0 ? string.Join(", ", parameters.Select(param => param.ParameterType.Name)) : string.Empty;
                              string info = $"{method.ReflectedType}.{method.Name}({signature})";

                              throw new ArgumentException($"Couldn't invoke method '{info}'.\n -- {message}");
                        }
                  }
                  return method.Invoke(target, values.ToArray());
            }
      }
}