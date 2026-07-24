using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static Emp37.Utility.ReflectionUtility;

namespace Emp37.Utility
{
	public static partial class ReflectionUtility
	{
		[Flags]
		public enum MemberTypes
		{
			Field = 1 << 0, Property = 1 << 1, Method = 1 << 2,
			All = Field | Property | Method
		}

		private delegate TMember Resolver<TMember>(Type declaringType, string memberName, Type[] parameterTypes, BindingFlags flags) where TMember : MemberInfo;

		public const BindingFlags DEFAULT_FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		private readonly struct MemberKey : IEquatable<MemberKey>
		{
			public readonly Type DeclaringType;
			public readonly string Name;
			public readonly Type[] ParameterTypes;
			public readonly BindingFlags Flags;

			public MemberKey(Type type, string name, Type[] parameterTypes, BindingFlags flags)
			{
				DeclaringType = type;
				Name = name;
				ParameterTypes = parameterTypes;
				Flags = flags;
			}

			public bool Equals(MemberKey other)
			{
				if (DeclaringType != other.DeclaringType || !string.Equals(Name, other.Name, StringComparison.Ordinal)) return false;

				Type[] a = ParameterTypes, b = other.ParameterTypes;

				if (ReferenceEquals(a, b)) return true;
				if (a == null || b == null || a.Length != b.Length) return false;
				for (int i = 0; i < a.Length; i++)
				{
					if (a[i] != b[i]) return false;
				}
				return true;
			}
			public override bool Equals(object obj) => obj is MemberKey other && Equals(other);
			public override int GetHashCode()
			{
				HashCode hash = new();

				hash.Add(DeclaringType);
				hash.Add(Name, StringComparer.Ordinal);
				if (ParameterTypes != null) foreach (Type type in ParameterTypes) hash.Add(type);

				return hash.ToHashCode();
			}
		}

		private static readonly ConcurrentDictionary<MemberKey, MemberInfo> memberCache = new();

		private static T FetchInfo<T>(Type type, string name, Type[] parameterTypes, BindingFlags flags, Resolver<T> resolver) where T : MemberInfo
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Member name cannot be null or empty.", nameof(name));

			MemberKey key = new(type, name, parameterTypes, flags);
			if (!memberCache.TryGetValue(key, out MemberInfo member))
			{
				for (Type current = type; current != null; current = current.BaseType)
				{
					member = resolver(current, name, parameterTypes, flags);
					if (member != null) break;
				}
				memberCache[key] = member;
			}
			return member as T;
		}
		private static Type[] ResolveArguments(object[] args)
		{
			if (args == null) return null;

			Type[] types = new Type[args.Length];

			for (int i = 0; i < args.Length; i++)
			{
				object argument = args[i];
				types[i] = argument == null ? typeof(object) : argument.GetType();
			}
			return types;
		}

		public static FieldInfo FindField(string name, Type type, BindingFlags flags = DEFAULT_FLAGS) => FetchInfo(type, name, null, flags, static (t, n, _, f) => t.GetField(n, f));
		public static PropertyInfo FindProperty(string name, Type type, BindingFlags flags = DEFAULT_FLAGS) => FetchInfo(type, name, null, flags, static (t, n, _, f) => t.GetProperty(n, f));
		public static MethodInfo FindMethod(string name, Type type, Type[] parameterTypes = null, BindingFlags flags = DEFAULT_FLAGS) => FetchInfo(type, name, parameterTypes, flags, static (t, n, p, f) => p == null ? t.GetMethod(n, f) : t.GetMethod(n, f, null, p, null));

		/// <summary>
		/// Finds the first matching member of one of the requested member types.
		/// </summary>
		/// <remarks>Members are searched in field, property, then method order. A diagnostic is logged when no matching member is found.</remarks>
		/// <param name="parameterTypes">The parameter types used for method overload resolution. Ignored when resolving fields and properties.</param>
		/// <returns>The first matching <see cref="MemberInfo"/>, or <c>null</c> when no match is found.</returns>
		public static MemberInfo FindMember(string name, Type type, Type[] parameterTypes = null, BindingFlags flags = DEFAULT_FLAGS, MemberTypes members = MemberTypes.All)
		{
			if (string.IsNullOrWhiteSpace(name) || type == null) return null;

			if ((members & MemberTypes.Field) != 0 && FindField(name, type, flags) is { } field) return field;
			if ((members & MemberTypes.Property) != 0 && FindProperty(name, type, flags) is { } property) return property;
			if ((members & MemberTypes.Method) != 0 && FindMethod(name, type, parameterTypes, flags) is { } method) return method;

			Debug.Log($"Unable to find member '{name}' in {type.FullName}. Verify the member name and its accessibility.");
			return null;
		}

		public static object ReadField(string name, object target, BindingFlags flags = DEFAULT_FLAGS) => target != null && FindField(name, target.GetType(), flags) is { } field ? field.GetValue(target) : null;
		public static object ReadProperty(string name, object target, BindingFlags flags = DEFAULT_FLAGS) => target != null && FindProperty(name, target.GetType(), flags) is { } property && property.CanRead ? property.GetValue(target) : null;
		public static object InvokeMethod(string name, object target, object[] parameters = null, BindingFlags flags = DEFAULT_FLAGS) => target != null && FindMethod(name, target.GetType(), ResolveArguments(parameters), flags) is { } method ? method.Invoke(target, parameters) : null;

		/// <summary>
		/// Attempts to read the value of a field, property, or method member named <paramref name="name"/> on <paramref name="target"/>. Members are resolved in order: field, then property, then method. The first match whose value can be read is returned.
		/// </summary>
		/// <remarks>
		/// This method only guards against <i>member resolution</i> failure; it returns <see langword="false"/> when no matching readable member is found.
		/// <br>It does <b>not</b> guard against exceptions raised by the member itself i.e. a throwing property getter, or an exception from an invoked method (surfaced as <see cref="TargetInvocationException"/>), will propagate to the caller.</br>
		/// <br>Reflection may also throw before invocation (e.g. on argument count or type mismatch for the method branch).</br>
		/// <para>A return value of <see langword="true"/> with <paramref name="value"/> set to <see langword="null"/> is valid and indicates the member was found and genuinely holds <see langword="null"/>, distinct from a <see langword="false"/> return, which indicates no matching member.</para>
		/// </remarks>
		/// <param name="name">The name of the member to read. Whitespace or <see langword="null"/> yields <see langword="false"/>.</param>
		/// <param name="target">The instance to read from. <see langword="null"/> yields <see langword="false"/>.</param>
		/// <param name="parameters">Arguments passed to the member when it is a method, ignored for fields and properties. Defaults to <see langword="null"/> (parameterless).</param>
		/// <param name="members">A mask restricting which member kinds are considered. Defaults to <see cref="MemberTypes.All"/>.</param>
		/// <returns><see langword="true"/> if a matching readable member was found and its value read, otherwise <see langword="false"/>.</returns>
		public static bool TryReadMember(string name, object target, out object value, object[] parameters = null, BindingFlags flags = DEFAULT_FLAGS, MemberTypes members = MemberTypes.All)
		{
			value = null;
			if (string.IsNullOrWhiteSpace(name) || target == null) return false;

			Type type = target.GetType();
			if ((members & MemberTypes.Field) != 0 && FindField(name, type, flags) is { } field) { value = field.GetValue(target); return true; }
			if ((members & MemberTypes.Property) != 0 && FindProperty(name, type, flags) is { } property && property.CanRead) { value = property.GetValue(target); return true; }
			if ((members & MemberTypes.Method) != 0 && FindMethod(name, type, ResolveArguments(parameters), flags) is { } method) { value = method.Invoke(target, parameters); return true; }

			return false;
		}

		/// <summary>
		/// Removes all cached reflection member lookups.
		/// </summary>
		public static void ClearMemberCache() => memberCache.Clear();

		/// <summary>
		/// Invokes a method using argument values automatically read from fields or properties on the target object.
		/// </summary>
		/// <remarks>
		/// Each entry in <paramref name="argNames"/> corresponds by position to a method parameter.
		/// The named value is resolved from a field or readable property on <paramref name="target"/> and validated against the expected parameter type before invocation.
		/// </remarks>
		/// <param name="method">The method to invoke.</param>
		/// <param name="target">The object used both as the invocation target and as the source of argument values.</param>
		/// <param name="argNames">Field or property names corresponding to the method parameters, in parameter order.</param>
		/// <param name="flags">The binding flags used to resolve argument members.</param>
		/// <returns>The value returned by the invoked method.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="method"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">Thrown when argument names do not match the method's parameter count, an argument member cannot be resolved, or a resolved value is incompatible with its corresponding parameter.</exception>
		public static object AutoInvokeMethod(MethodInfo method, object target, string[] argNames = null, BindingFlags flags = DEFAULT_FLAGS)
		{
			if (method == null) throw new ArgumentNullException(nameof(method));

			ParameterInfo[] parameters = method.GetParameters();
			int length = parameters.Length;

			if (length == 0) return method.Invoke(target, null);

			Assert(argNames != null && length == argNames.Length, "Argument count mismatch.");

			object[] values = new object[length];

			for (int i = 0; i < length; i++)
			{
				Assert(TryReadMember(argNames[i], target, out object value, null, DEFAULT_FLAGS, MemberTypes.Field | MemberTypes.Property), $"Could not resolve a field or property named '{argNames[i]}' on '{target.GetType().FullName}'. The member may not exist or may not be accessible.");

				Type expectedType = parameters[i].ParameterType;
				if (value == null)
				{
					Assert(!expectedType.IsValueType || Nullable.GetUnderlyingType(expectedType) != null, $"Parameter {i} ('{parameters[i].Name}') expects non-nullable '{expectedType}' but '{argNames[i]}' resolved to null.");
				}
				else
				{
					Assert(expectedType.IsAssignableFrom(value.GetType()), $"Type mismatch at index {i}. Expected '{expectedType}', received '{value.GetType()}'.");
				}
				values[i] = value;
			}
			return method.Invoke(target, values);

			void Assert(bool condition, string message)
			{
				if (condition) return;
				string signature = string.Join(", ", Array.ConvertAll(parameters, param => param.ParameterType.Name)), info = $"{method.ReflectedType}.{method.Name}({signature})";
				throw new ArgumentException($"Invoke failed for method '{info}'.\n-- {message}");
			}
		}
	}
}