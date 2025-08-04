using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace Emp37.Utility.Editor
{
	using static ReflectionUtility;

	public static class SerializedPropertyUtility
	{
		private readonly struct FieldKey : IEquatable<FieldKey>
		{
			public readonly Type RootType;
			public readonly string Path;
			public readonly BindingFlags Flags;

			public FieldKey(Type rootType, string path, BindingFlags flags)
			{
				RootType = rootType;
				Path = path;
				Flags = flags;
			}

			public bool Equals(FieldKey other) => RootType == other.RootType && Path == other.Path && Flags == other.Flags;
			public override bool Equals(object obj) => obj is FieldKey other && Equals(other);
			public override int GetHashCode() => HashCode.Combine(RootType, Path, Flags);
		}

		private readonly static Dictionary<FieldKey, FieldInfo> propertyCache = new();

		public static FieldInfo GetField(this SerializedProperty property, BindingFlags flags = DEFAULT_FLAGS)
		{
			if (property == null) throw new ArgumentNullException(nameof(property));

			Type type = property.serializedObject.targetObject.GetType();
			string path = property.propertyPath.Replace(".Array.data", string.Empty);

			FieldKey key = new(type, path, flags);

			if (propertyCache.TryGetValue(key, out FieldInfo field)) return field;

			Type current = type;
			string[] segments = path.Split('.');

			for (int last = segments.Length - 1, i = 0; i <= last; i++)
			{
				string segment = segments[i];
				string name = segment.Contains('[') ? segment[..segment.IndexOf('[')] : segment;

				field = FindField(name, current, flags);
				if (field == null || i == last) break;

				Type next = field.FieldType;
				current = next.IsArray ? next.GetElementType() : next.IsGenericType ? next.GetGenericArguments()[0] : next;
			}
			propertyCache[key] = field;
			return field;
		}
	}
}