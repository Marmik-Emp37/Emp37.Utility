using System;
using System.Collections.Generic;

using UnityEngine;

namespace Emp37.Utility.Singleton
{
      /// <summary>
      ///A registry for managing <see cref="MonoBehaviour"/> instances.
      /// <para>Use <see cref="Register"/> to add an instance, <see cref="Unregister"/> to remove an instance, and <see cref="Get{TBehaviour}"/> to retrieve a registered MonoBehaviour.</para>
      /// </summary>
      public static class MonoBehaviourRegistry
      {
            private static readonly Dictionary<Type, MonoBehaviour> database = new();

            /// <summary>
            /// Returns a formatted list of registered types from this registry.
            /// <br>Intended for logging and debugging purposes.</br>
            /// </summary>
            /// <remarks>This property provides a newline-separated string containing all registered types on this registry.</remarks>
            public static string RegisteredTypes => string.Join("\n", database.Keys);

            /// <summary>
            /// Retrieves the registered instance of a specific type.
            /// </summary>
            /// <typeparam name="TBehaviour">Type of the MonoBehaviour to retrieve.</typeparam>
            /// <returns>Instance of type T if found, otherwise null.</returns>
            /// <remarks>It is recommended to cache the return value for improved performance when conducting frequent lookups of the same type.</remarks>
            public static TBehaviour Get<TBehaviour>() where TBehaviour : MonoBehaviour
            {
                  Type type = typeof(TBehaviour);
                  if (database.TryGetValue(type, out MonoBehaviour instance))
                  {
                        return instance as TBehaviour;
                  }
                  Log($"No registered instance found for type '{type.FullName}'. Ensure that this type has been registered before attempting to retrieve it.", LogType.Warning);
                  return null;
            }

            /// <summary>
            /// Registers a MonoBehaviour instance in the registry.
            /// </summary>
            /// <param name="instance">The MonoBehaviour instance to register.</param>
            public static void Register(this MonoBehaviour instance)
            {
                  if (instance == null)
                  {
                        Log("Cannot register a null instance.", LogType.Warning);
                        return;
                  }
                  Type type = instance.GetType();
                  if (database.ContainsKey(type))
                  {
                        Log($"An instance of type '{type.FullName}' is already registered. The new instance on GameObject '{instance.name}' will not be registered to prevent conflicts.", LogType.Error, database[type]);
                  }
                  else
                  {
                        database.Add(type, instance);
                        Log($"Registered instance of type '{type.FullName}'.", context: instance);
                  }
            }

            /// <summary>
            /// Unregisters a MonoBehaviour instance from the registry.
            /// </summary>
            /// <param name="instance">The MonoBehaviour instance to unregister.</param>
            public static void Unregister(this MonoBehaviour instance)
            {
                  if (instance == null)
                  {
                        Log("Cannot unregister a null instance.", LogType.Warning);
                        return;
                  }
                  Type type = instance.GetType();
                  if (database.ContainsKey(type))
                  {
                        if (database[type] != instance)
                        {
                              Log($"The provided instance does not match the registered instance for type '{type.FullName}'.", LogType.Warning);
                              return;
                        }
                        database.Remove(type);
                        Log($"Unregistered instance of type '{type.FullName}'.");
                  }
                  else
                  {
                        Log($"No registered instance found for type '{type.FullName}'. Ensure that this type has been registered before attempting to unregister it.", LogType.Warning);
                  }
            }

            /// <summary>
            /// Erases all entries from the registry, unregistering all registered instances.
            /// </summary>
            public static void Wipe()
            {
                  database.Clear();
                  Log("All registered instances have been removed.");
            }

            private static void Log(string message, LogType type = LogType.Log, UnityEngine.Object context = null) => Debug.unityLogger.Log(type, message: '[' + nameof(MonoBehaviourRegistry) + "]: " + message, context);
      }
}