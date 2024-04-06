using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Emp37.Utility
{
      public static class ReflectionUtility
      {
            [Flags]
            public enum MemberType
            {
                  Field = 1,
                  Property = 2,
                  Method = 4,
                  All = 7,
            }

            public const BindingFlags DEFAULT_FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            private static readonly Dictionary<(Type, string), MemberInfo> cachedMembers = new();

            public static T FetchInfo<T>(string name, Type type, BindingFlags bindings = DEFAULT_FLAGS) where T : MemberInfo
            {
                  if (type == null) throw new ArgumentNullException(nameof(type));
                  if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));

                  var key = (type, name);
                  if (!cachedMembers.TryGetValue(key, out var member))
                  {
                        while (type != null)
                        {
                              member = typeof(T) switch
                              {
                                    var field when field == typeof(FieldInfo) => type.GetField(name, bindings),
                                    var property when property == typeof(PropertyInfo) => type.GetProperty(name, bindings),
                                    var method when method == typeof(MethodInfo) => type.GetMethod(name, bindings),
                                    _ => throw new NotSupportedException()
                              };
                              if (member != null)
                              {
                                    cachedMembers[key] = member;
                                    break;
                              }
                              type = type.BaseType;
                        }
                  }
                  return member as T;
            }
            public static object FetchValue(string name, object target, MemberType allowedTypes = MemberType.All, BindingFlags bindings = DEFAULT_FLAGS, object[] parameters = null)
            {
                  if (target == null) throw new ArgumentNullException(nameof(target));

                  object value = null;
                  Type type = target.GetType();

                  if (allowedTypes.HasFlag(MemberType.Field))
                  {
                        var field = FetchInfo<FieldInfo>(name, type, bindings);
                        if (field != null)
                        {
                              return field.GetValue(target);
                        }
                  }
                  if (allowedTypes.HasFlag(MemberType.Property))
                  {
                        var property = FetchInfo<PropertyInfo>(name, type, bindings);
                        if (property != null && property.CanRead)
                        {
                              return property.GetValue(target);
                        }
                  }
                  if (allowedTypes.HasFlag(MemberType.Method))
                  {
                        var method = FetchInfo<MethodInfo>(name, type, bindings);
                        if (method != null)
                        {
                              return method.Invoke(target, parameters);
                        }
                  }
                  return value;
            }

            public static object InvokeWithNamedParametersAndReflection(MethodInfo method, object target, string[] names = null, BindingFlags bindings = DEFAULT_FLAGS)
            {
                  List<object> values = new();
                  ParameterInfo[] parameters = method.GetParameters();

                  if (parameters.Length > 0)
                  {
                        Assert(names != null && parameters.Length == names.Length, "Number of parameters specified does not match the expected number.");

                        for (byte i = 0; i < parameters.Length; i++)
                        {
                              object value = FetchValue(names[i], target, MemberType.Field | MemberType.Property, bindings);

                              Assert(value != null, $"Unable to fetch value for '{names[i]}' in type '{target.GetType().FullName}'. The member may not exist or may not be accessible.");

                              Type parameterType = value.GetType(), expectedType = parameters[i].ParameterType;

                              Assert(expectedType == parameterType, $"Parameter type mismatch at index {i}. Expected type '{expectedType}' but recieved '{parameterType}'.");

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