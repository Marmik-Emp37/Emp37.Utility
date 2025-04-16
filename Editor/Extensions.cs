using System;
using System.Linq;
using System.Reflection;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      using static ReflectionUtility;

      public static class Extensions
      {
            public static FieldInfo GetField(this SerializedProperty property, BindingFlags flags = DEFAULT_FLAGS)
            {
                  if (property == null) throw new ArgumentNullException(nameof(property));

                  Type currentType = property.serializedObject.targetObject.GetType();
                  string[] path = property.propertyPath.Replace(".Array.data", string.Empty).Split('.');
                  FieldInfo field = null;

                  for (int last = path.Length - 1, i = 0; i <= last; i++)
                  {
                        string segment = path[i];
                        int index = segment.IndexOf('[');
                        string name = index >= 0 ? segment[..index] : segment;

                        field = FindField(name, currentType, flags);
                        if (field == null || i == last) break;

                        Type type = field.FieldType;
                        currentType = type.IsArray ? type.GetElementType() : type.IsGenericType ? type.GetGenericArguments().FirstOrDefault() : type;
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