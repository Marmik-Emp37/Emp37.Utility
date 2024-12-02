using UnityEngine;

namespace Emp37.Utility.Singleton
{
      /// <summary>
      /// A dynamic MonoBehaviour-based singleton to use at runtime.
      /// </summary>
      /// <typeparam name="T">The type of the singleton, which must inherit from <see cref="Dynamic{T}"/>.</typeparam>
      /// <remarks>
      /// <b>NOTE:</b> This implementation -
      /// <br>• Requires calling <see cref="Initialize(bool)"/> to properly set up the singleton instance.</br>
      /// <br>• Destroys any additional instances of <typeparamref name="T"/> detected at runtime to enforce the singleton implementation.</br>
      /// <br>• Supports automatic instantiation of the singleton if uninitialized.</br>
      /// <br>• Supports implicit placement of the component T on the scene.</br>
      /// <br>• Usage example for a singleton of type <see cref="Dynamic{T}"></see>:</br>
      /// <code>
      /// public class MySingleton : Dynamic&lt;MySingleton&gt;
      /// {
      ///     void Awake()
      ///     {
      ///         Initialize(true); //make this singleton persist across scenes
      ///     }
      /// }
      /// </code>
      /// </remarks>
      public abstract class Dynamic<T> : MonoBehaviour where T : Dynamic<T>
      {
            private static T _instance;
            private static object @lock = new();
            private static bool isQuitting;

            public static T Instance
            {
                  get
                  {
                        if (isQuitting)
                        {
                              Debug.LogWarning($"Instance of '{typeof(T)}' no longer exists. Returning null.");
                              return null;
                        }
                        lock (@lock)
                        {
                              if (_instance == null)
                              {
                                    _instance = new GameObject($"{typeof(T).Name}: Singleton").AddComponent<T>();
                                    DontDestroyOnLoad(_instance);
                              }
                        }
                        return _instance;
                  }
            }


            protected virtual void OnApplicationQuit() => isQuitting = true;

            protected void Initialize(bool persistent)
            {
                  if (_instance != null && _instance != this)
                  {
                        Object context = _instance.gameObject;
                        Destroy(_instance);
                        Debug.LogWarning($"An additional instance of '{typeof(T).FullName}' was detected on '{name}'. This duplicate was destroyed to preserve the singleton implementation.", context);
                  }
                  else
                  {
                        _instance = this as T;
                        if (persistent) DontDestroyOnLoad(this);
                  }
            }
      }
}