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
                  Field = 1 << 0, Property = 1 << 1, Method = 1 << 2,
                  All = Field | Property | Method
            }

            public const BindingFlags DEFAULT_FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            private static readonly Dictionary<(string, Type), MemberInfo> reflectionCache = new();


            public static T FetchInfo<T>(string name, Type type, BindingFlags flags = DEFAULT_FLAGS) where T : MemberInfo
            {
                  if (type == null) throw new ArgumentNullException(nameof(type));
                  if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Member name cannot be null or empty.", nameof(name));

                  (string name, Type type) key = (name, type);
                  Type target = typeof(T);

                  if (!reflectionCache.TryGetValue(key, out MemberInfo member))
                  {
                        while (type != null)
                        {
                              member = target switch
                              {
                                    _ when target == typeof(FieldInfo) => type.GetField(name, flags),
                                    _ when target == typeof(PropertyInfo) => type.GetProperty(name, flags),
                                    _ when target == typeof(MethodInfo) => type.GetMethod(name, flags),
                                    _ when target == typeof(EventInfo) => type.GetEvent(name, flags),
                                    _ => throw new NotSupportedException($"'{target.Name}' is not supported.")
                              };

                              if (member != null)
                              {
                                    reflectionCache[key] = member;
                                    break;
                              }

                              type = type.BaseType;
                        }
                  }
                  return member as T ?? throw new MissingMemberException($"{target.Name} '{name}' not found on '{key.type.FullName}' or its base types.");
            }
            public static bool TryFetchInfo<T>(string name, Type type, out T value, BindingFlags flags = DEFAULT_FLAGS) where T : MemberInfo
            {
                  try
                  {
                        value = FetchInfo<T>(name, type, flags);
                  }
                  catch (Exception ex)
                  {
                        value = null;

                        Debug.LogWarning($"Failed to fetch {typeof(T).Name} '{name}' on {type.FullName}: {ex.Message}");
                  }
                  return value != null;
            }
            public static object FetchValue(string name, object target, MemberTypes searchTargets = MemberTypes.All, BindingFlags flags = DEFAULT_FLAGS, params object[] arguments)
            {
                  if (target == null) throw new ArgumentNullException(nameof(target));

                  Type type = target.GetType();

                  if (searchTargets.HasFlag(MemberTypes.Field) && TryFetchInfo(name, type, out FieldInfo field, flags)) return field.GetValue(target);
                  if (searchTargets.HasFlag(MemberTypes.Property) && TryFetchInfo(name, type, out PropertyInfo property, flags) && property.CanRead) return property.GetValue(target);
                  if (searchTargets.HasFlag(MemberTypes.Method) && TryFetchInfo(name, type, out MethodInfo method, flags)) return method.Invoke(target, arguments);

                  return null;
            }
            public static object InvokeMethod(MethodInfo method, object target, string[] argNames = null, BindingFlags flags = DEFAULT_FLAGS)
            {
                  ParameterInfo[] parameters = method.GetParameters();
                  if (parameters.Length == 0) return method.Invoke(target, null);

                  Assert(argNames != null && parameters.Length == argNames.Length, "Argument count mismatch.");

                  object[] values = new object[parameters.Length];

                  for (int length = parameters.Length, i = 0; i < length; i++)
                  {
                        object value = FetchValue(argNames[i], target, MemberTypes.Field | MemberTypes.Property, flags);

                        Assert(value != null, $"Could not resolve value for '{argNames[i]}' from '{target.GetType().FullName}'. The member may not exist or may not be accessible.");

                        Type valueType = value.GetType(), expectedType = parameters[i].ParameterType;

                        Assert(expectedType.IsAssignableFrom(valueType), $"Type mismatch at index {i}. Expected '{expectedType}', received` '{valueType}'.");

                        values[i] = value;
                  }
                  return method.Invoke(target, values);

                  void Assert(bool condition, string message)
                  {
                        if (condition) return;

                        string signature = parameters.Length > 0 ? string.Join(", ", parameters.Select(param => param.ParameterType.Name)) : string.Empty;
                        string info = $"{method.ReflectedType}.{method.Name}({signature})";

                        throw new ArgumentException($"Invoke failed for method '{info}'.\n-- {message}");
                  }
            }
      }
}