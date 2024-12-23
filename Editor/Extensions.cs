using System;
using System.Reflection;

using UnityEditor;

namespace Emp37.Utility.Editor
{
      using static ReflectionUtility;

      public static class Extensions
      {
            #region S E R I A L I Z E D   P R O P E R T Y
            /// <summary>
            /// Retrieves the attribute of type <typeparamref name="TAttribute"/> associated with a serialized property.
            /// </summary>
            /// <param name="property">The serialized property to inspect.</param>
            /// <param name="bindings">Binding flags for fetching the field information.</param>
            /// <returns>Attribute of type TAttribute if found, otherwise null.</returns>
            /// <exception cref="ArgumentNullException">When the serialized property is null.</exception>
            /// <exception cref="ArgumentException">When the serialized property target object type is null.</exception>
            public static TAttribute GetAttribute<TAttribute>(this SerializedProperty property, BindingFlags bindings = ReflectionFlags) where TAttribute : Attribute
            {
                  if (property == null) throw new ArgumentNullException(nameof(property), "SerializedProperty cannot be null.");
                  object target = (property.serializedObject?.targetObject) ?? throw new ArgumentException($"Serialized object or its target is null for property '{property.name}'.");
                  FieldInfo field = FetchInfo<FieldInfo>(property.name, target.GetType(), bindings) ?? throw new MissingFieldException($"Field '{property.name}' not found in target type '{target.GetType().FullName}'.");
                  return field.GetCustomAttribute<TAttribute>();
            }
            public static bool TryGetAttribute<TAttribute>(this SerializedProperty property, out TAttribute attribute, BindingFlags bindings = ReflectionFlags) where TAttribute : Attribute
            {
                  attribute = property?.GetAttribute<TAttribute>(bindings);
                  return attribute != null;
            }
            public static bool HasAttribute<TAttribute>(this SerializedProperty property, BindingFlags bindings = ReflectionFlags) where TAttribute : Attribute
            {
                  if (property == null)
                  {
                        Log($"{nameof(property)} is null.");
                        return false;
                  }
                  if (property.serializedObject.targetObject is not object target)
                  {
                        Log($"{nameof(SerializedObject)} or it's target is null.");
                        return false;
                  }
                  FieldInfo field = FetchInfo<FieldInfo>(property.name, target.GetType(), bindings);
                  if (field == null)
                  {
                        Log($"'{property.name}' not found in type '{target.GetType().FullName}'.");
                  }
                  return field.IsDefined(typeof(TAttribute), inherit: true);

                  static void Log(string message) => UnityEngine.Debug.LogWarning($"Unable to check attribute '{typeof(TAttribute).Name}': {message}");
            }
            #endregion

            #region M E M B E R   I N F O
            public static bool TryGetAttribute<TAttribute>(this MemberInfo member, out TAttribute attribute) where TAttribute : Attribute
            {
                  attribute = member?.GetCustomAttribute<TAttribute>();
                  return attribute != null;
            }
            #endregion
      }
}