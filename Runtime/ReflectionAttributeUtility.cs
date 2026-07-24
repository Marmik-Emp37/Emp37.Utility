using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Emp37.Utility
{
	public static partial class ReflectionUtility
	{
		private readonly struct AttributeKey : IEquatable<AttributeKey>
		{
			public readonly ICustomAttributeProvider Provider;
			public readonly Type AttributeType;
			public readonly bool Inherit;

			public AttributeKey(ICustomAttributeProvider provider, Type attributeType, bool inherit)
			{
				Provider = provider;
				AttributeType = attributeType;
				Inherit = inherit;
			}

			public bool Equals(AttributeKey other) => Provider == other.Provider && AttributeType == other.AttributeType && Inherit == other.Inherit;
			public override bool Equals(object obj) => obj is AttributeKey other && Equals(other);
			public override int GetHashCode() => HashCode.Combine(RuntimeHelpers.GetHashCode(Provider), AttributeType, Inherit);
		}

		private static readonly ConcurrentDictionary<AttributeKey, Attribute[]> attributeCache = new();

		private static T[] ResolveAttributes<T>(ICustomAttributeProvider provider, bool inherit) where T : Attribute
		{
			AttributeKey key = new(provider, typeof(T), inherit);
			if (attributeCache.TryGetValue(key, out Attribute[] cached)) return (T[]) cached;

			object[] raw = provider.GetCustomAttributes(typeof(T), inherit);
			T[] attributes = new T[raw.Length];

			for (int i = 0; i < raw.Length; i++) attributes[i] = (T) raw[i];

			attributeCache[key] = attributes;
			return attributes;
		}

		public static T[] GetAttributes<T>(ICustomAttributeProvider provider, bool inherit = false) where T : Attribute => ResolveAttributes<T>(provider, inherit);
		public static T GetAttribute<T>(ICustomAttributeProvider provider, bool inherit = false) where T : Attribute
		{
			T[] attributes = ResolveAttributes<T>(provider, inherit);
			return attributes.Length > 0 ? attributes[0] : null;
		}
		public static bool TryGetAttribute<T>(ICustomAttributeProvider provider, out T attribute, bool inherit = false) where T : Attribute
		{
			T[] attributes = ResolveAttributes<T>(provider, inherit);
			attribute = attributes.Length > 0 ? attributes[0] : null;
			return attribute != null;
		}
		public static bool HasAttribute<T>(ICustomAttributeProvider provider, bool inherit = false) where T : Attribute => ResolveAttributes<T>(provider, inherit).Length > 0;
		public static void ClearAttributeCache() => attributeCache.Clear();
	}
}