using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emp37.Utility
{
        /// <summary>
        /// A lightweight, type-keyed registry for <see cref="MonoBehaviour"/> singletons.
        /// <para>Use <see cref="Register"/> to add an instance, <see cref="Unregister"/> to remove one, and <see cref="Get{TBehaviour}"/> / <see cref="TryGet{TBehaviour}"/> to retrieve a registered instance by its concrete type.</para>
        /// </summary>
        public static class MonoBehaviourRegistry
        {
                private static readonly Dictionary<Type, MonoBehaviour> registry = new();

                /// <summary>
		/// The number of instances currently registered.
		/// </summary>
		public static int Count => registry.Count;

                /// <summary>
                /// A newline-separated list of every registered type. Intended for logging and debugging purposes.
                /// </summary>
                public static string RegisteredTypes => string.Join("\n", registry.Keys);

                /// <summary>
		/// Fetches the registered instance of a specific type.
		/// </summary>
		/// <typeparam name="TBehaviour">Type of the MonoBehaviour to retrieve.</typeparam>
		/// <remarks>Cache the return value for improved performance when conducting frequent lookups of the same type.</remarks>
		/// <returns>The instance of type <typeparamref name="TBehaviour"/> if found, otherwise <c>null</c>.</returns>
                public static TBehaviour Get<TBehaviour>() where TBehaviour : MonoBehaviour
                {
                        Type type = typeof(TBehaviour);

                        if (TryResolve(type, out MonoBehaviour instance))
                        {
                                return instance as TBehaviour;
                        }
                        Log($"No registered instance found for type '{type.FullName}'.", LogType.Warning);
                        return null;
                }
                /// <summary>
		/// Attempts to fetch the registered instance of a specific type without logging on failure.
		/// </summary>
		/// <typeparam name="TBehaviour">Type of the MonoBehaviour to retrieve.</typeparam>
		/// <param name="instance">The registered instance if found, otherwise <c>null</c>.</param>
                public static bool TryGet<TBehaviour>(out TBehaviour instance) where TBehaviour : MonoBehaviour
                {
                        _ = TryResolve(typeof(TBehaviour), out MonoBehaviour value);

                        instance = value as TBehaviour;

                        return instance != null;
                }
                /// <summary>
                /// Registers an instance under its concrete runtime type.
                /// </summary>
                public static void Register(this MonoBehaviour instance)
                {
                        if (instance == null)
                        {
                                Log("Cannot register a null instance.", LogType.Warning);
                                return;
                        }

                        Type type = instance.GetType();

                        if (registry.TryGetValue(type, out MonoBehaviour existing) && existing != null)
                        {
                                Log($"An instance of type '{type.FullName}' is already registered. The new instance on GameObject '{instance.name}' will not be registered to prevent conflicts.", LogType.Error, existing);
                                return;
                        }

                        registry[type] = instance;
                        Log($"Registered instance of type '{type.FullName}'.", context: instance);
                }
                /// <summary>
		/// Removes a previously registered instance.
		/// </summary>
		public static void Unregister(this MonoBehaviour instance)
                {
                        if (instance == null)
                        {
                                Log("Cannot unregister a null instance.", LogType.Warning);
                                return;
                        }

                        Type type = instance.GetType();

                        if (registry.TryGetValue(type, out MonoBehaviour registered))
                        {
                                if (registered != instance)
                                {
                                        Log($"The provided instance does not match the registered instance for type '{type.FullName}'.", LogType.Warning);
                                        return;
                                }

                                registry.Remove(type);
                                Log($"Unregistered instance of type '{type.FullName}'.");
                                return;
                        }
                        Log($"No registered instance found for type '{type.FullName}'. Ensure that this type has been registered before attempting to unregister it.", LogType.Warning);
                }
                /// <summary>
                /// Erases all entries from the registry, unregistering every registered instance.
                /// </summary>
                public static void Wipe()
                {
                        registry.Clear();

                        Log($"{nameof(MonoBehaviourRegistry)} has been successfully cleared. All registered instances have been removed.");
                }

                private static bool TryResolve(Type type, out MonoBehaviour instance)
                {
                        if (registry.TryGetValue(type, out instance))
                        {
                                if (instance != null) return true;
                                registry.Remove(type);
                        }
                        instance = null;
                        return false;
                }
                private static void Log(string message, LogType type = LogType.Log, UnityEngine.Object context = null) => Debug.unityLogger.Log(type, message: $"[{nameof(MonoBehaviourRegistry)}]: {message}", context);
        }
}