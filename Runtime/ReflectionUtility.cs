using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Emp37.Utility
{
      using Field = FieldInfo;
      using Property = PropertyInfo;
      using Method = MethodInfo;


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
                  if (type == null) throw new ArgumentNullException(nameof(type));
                  if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));

                  (Type, string) key = (type, name);
                  if (!cachedMembers.TryGetValue(key, out MemberInfo member))
                  {
                        Type returnType = typeof(T);
                        while (type != null)
                        {
                              member = returnType switch
                              {
                                    _ when returnType == typeof(Field) => type.GetField(name, bindings),
                                    _ when returnType == typeof(Property) => type.GetProperty(name, bindings),
                                    _ when returnType == typeof(Method) => type.GetMethod(name, bindings),
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
            public static bool TryFetchInfo<T>(string name, Type type, out T value, BindingFlags bindings = DefaultFlags) where T : MemberInfo
            {
                  try
                  {
                        value = FetchInfo<T>(name, type, bindings);
                  }
                  catch (Exception)
                  {
                        value = null;
                  }
                  return value != null;
            }
            public static object FetchValue(string name, object target, MemberTypes enabled = MemberTypes.All, BindingFlags bindings = DefaultFlags, params object[] parameters)
            {
                  if (target == null) throw new ArgumentNullException(nameof(target));

                  Type type = target.GetType();

                  if (enabled.HasFlag(MemberTypes.Field) && TryFetchInfo(name, type, out Field field, bindings))
                  {
                        return field.GetValue(target);
                  }
                  if (enabled.HasFlag(MemberTypes.Property) && TryFetchInfo(name, type, out Property property, bindings) && property.CanRead)
                  {
                        return property.GetValue(target);
                  }
                  if (enabled.HasFlag(MemberTypes.Method) && TryFetchInfo(name, type, out Method method, bindings))
                  {
                        return method.Invoke(target, parameters);
                  }
                  return null;
            }
            public static object InvokeMethod(Method method, object target, string[] args = null, BindingFlags bindings = DefaultFlags)
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