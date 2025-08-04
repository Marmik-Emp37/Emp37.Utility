using UnityEngine;

namespace Emp37.Utility.Singleton
{
      /// <summary>
      /// A dynamic MonoBehaviour-based singleton to use at runtime.
      /// </summary>
      /// <typeparam name="T">The type of the singleton, which must inherit from <see cref="Dynamic{T}"/>.</typeparam>
      /// <remarks>
      /// <b>NOTE:</b> This implementation -
      /// <br>• Requires calling <see cref="Initialize(bool)"/> to initialize the singleton instance.</br>
      /// <br>• Supports auto initialization.</br>
      /// <br>Usage example for a singleton of type <see cref="Dynamic{T}"/>:</br>
      /// <code>
      /// public class MySingleton : Dynamic&lt;MySingleton&gt;
      /// {
      ///     void Awake()
      ///     {
      ///         Initialize(true); // make this singleton persist across scenes
      ///     }
      /// }
      /// </code>
      /// </remarks>
      public abstract class Dynamic<T> : MonoBehaviour where T : Dynamic<T>
      {
            private static T instance;
            private static readonly object syncRoot = new();
            private static bool isExiting;

            public static T Instance
            {
                  get
                  {
                        if (isExiting)
                        {
                              Debug.LogWarning($"Instance of '{typeof(T).FullName}' no longer exists.");
                              return null;
                        }
                        if (instance != null) return instance;

                        lock (syncRoot)
                        {
                              return instance = FindFirstObjectByType<T>() ?? new GameObject(typeof(T).FullName).AddComponent<T>();
                        }
                  }
                  protected set => instance = value;
            }

            protected void Initialize(bool persistency)
            {
                  if (instance != null && instance != this)
                  {
                        Debug.LogWarning($"A duplicate instance of '{typeof(T).FullName}' found on gameObject '{name}'.", gameObject);
                        return;
                  }
                  instance = this as T;
                  if (persistency) DontDestroyOnLoad(this);
            }

            protected virtual void OnDestroy()
            {
                  if (instance == this) instance = null;
            }
            protected virtual void OnApplicationQuit() => isExiting = true;
      }
}