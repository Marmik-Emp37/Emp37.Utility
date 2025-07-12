using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      using static ReflectionUtility;

      public static class SerializedPropertyUtility
      {
            private readonly static Dictionary<SerializedProperty, FieldInfo> propertyCache = new();
            public static FieldInfo GetField(this SerializedProperty property, BindingFlags flags = DEFAULT_FLAGS)
            {
                  if (property == null) throw new ArgumentNullException(nameof(property));

                  if (!propertyCache.TryGetValue(property, out FieldInfo field))
                  {
                        Type currentType = property.serializedObject.targetObject.GetType();
                        string[] path = property.propertyPath.Replace(".Array.data", string.Empty).Split('.');

                        for (int last = path.Length - 1, i = 0; i <= last; i++)
                        {
                              string segment = path[i];
                              string name = segment.Contains('[') ? segment[..segment.IndexOf('[')] : segment;

                              field = FindField(name, currentType, flags);
                              if (field != null)
                              {
                                    propertyCache[property] = field;
                                    break;
                              }

                              if (i == last) break;

                              Type next = field.FieldType;
                              currentType = next.IsArray ? next.GetElementType() : next.IsGenericType ? next.GetGenericArguments().FirstOrDefault() : next;
                        }
                  }
                  return field;
            }
            public static bool HasAttribute<TAttribute>(this SerializedProperty property, BindingFlags flags = DEFAULT_FLAGS) where TAttribute : Attribute => GetField(property, flags) is { } field && field.IsDefined(typeof(TAttribute), true);
            public static bool TryGetAttribute<TAttribute>(this SerializedProperty property, out TAttribute attribute, BindingFlags flags = DEFAULT_FLAGS) where TAttribute : Attribute
            {
                  attribute = GetField(property, flags)?.GetCustomAttribute<TAttribute>(true);
                  return attribute != null;
            }
      }
}