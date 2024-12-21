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

            public const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            private static readonly Dictionary<(Type, string), MemberInfo> cachedMembers = new();

            public static T FetchInfo<T>(string name, Type type, BindingFlags bindings = DefaultFlags) where T : MemberInfo
            {
                  if (type == null) throw new ArgumentNullException(nameof(type), "The provided type cannot be null.");
                  if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Member name cannot be empty.", nameof(name));

                  (Type, string) key = (type, name);
                  if (!cachedMembers.TryGetValue(key, out MemberInfo member))
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
                                    cachedMembers[key] = member;
                                    break;
                              }
                              type = type.BaseType;
                        }
                  }
                  return member as T ?? throw new MissingMemberException($"Member '{name}' of type '{typeof(T).Name}' not found in the type hierarchy of '{key.Item1.FullName}'."); ;
            }
            public static bool TryFetchInfo<T>(string name, Type type, out T value, BindingFlags bindings = DefaultFlags) where T : MemberInfo
            {
                  value = null;

                  if (type == null || string.IsNullOrWhiteSpace(name))
                  {
                        Debug.LogWarning($"Invalid input: Type '{type}' or name '{name}' is null or empty.");
                        return false;
                  }

                  try
                  {
                        value = FetchInfo<T>(name, type, bindings);
                  }
                  catch (Exception ex)
                  {
                        Debug.LogWarning(ex.Message);
                  }

                  return value != null;
            }
            public static object FetchValue(string name, object target, MemberTypes enabled = MemberTypes.All, BindingFlags bindings = DefaultFlags, params object[] parameters)
            {
                  if (target == null) throw new ArgumentNullException(nameof(target));

                  Type type = target.GetType();

                  if (enabled.HasFlag(MemberTypes.Field) && TryFetchInfo(name, type, out FieldInfo field, bindings))
                  {
                        return field.GetValue(target);
                  }
                  if (enabled.HasFlag(MemberTypes.Property) && TryFetchInfo(name, type, out PropertyInfo property, bindings) && property.CanRead)
                  {
                        return property.GetValue(target);
                  }
                  if (enabled.HasFlag(MemberTypes.Method) && TryFetchInfo(name, type, out MethodInfo method, bindings))
                  {
                        return method.Invoke(target, parameters);
                  }
                  return null;
            }
            public static object InvokeMethod(MethodInfo method, object target, string[] args = null, BindingFlags bindings = DefaultFlags)
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