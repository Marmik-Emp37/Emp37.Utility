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
            /// <param name="flags">Binding flags for fetching the field information.</param>
            /// <returns>Attribute of type TAttribute if found, otherwise null.</returns>
            /// <exception cref="ArgumentNullException">When the serialized property is null.</exception>
            /// <exception cref="ArgumentException">When the serialized property target object type is null.</exception>
            public static TAttribute FindAttribute<TAttribute>(this SerializedProperty property, BindingFlags flags = DEFAULT_FLAGS) where TAttribute : Attribute
            {
                  if (property == null) throw new ArgumentNullException(nameof(property));
                  Type type = property.serializedObject.targetObject.GetType();
                  FieldInfo field = FindField(property.name, type, flags);
                  return field?.GetCustomAttribute<TAttribute>();
            }
            public static bool TryGetAttribute<TAttribute>(this SerializedProperty property, out TAttribute attribute, BindingFlags flags = DEFAULT_FLAGS) where TAttribute : Attribute
            {
                  attribute = property?.FindAttribute<TAttribute>(flags);
                  return attribute != null;
            }
            public static bool HasAttribute<TAttribute>(this SerializedProperty property, BindingFlags flags = DEFAULT_FLAGS) where TAttribute : Attribute
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
                  FieldInfo field = FindField(property.name, target.GetType(), flags);
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