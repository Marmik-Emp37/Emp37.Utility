using UnityEngine;

namespace Emp37.Utility.Singleton
{
      /// <summary>
      /// A dynamic MonoBehaviour-based singleton to use at runtime.
      /// </summary>
      /// <remarks>
      /// <b>NOTE:</b> This implementation -
      /// <br>• destroys any duplicate gameObject of type T instantiated at runtime.</br>
      /// <br>• supports implicit placement of the component T on the scene.</br>
      /// </remarks>
      public abstract class Dynamic<T> : MonoBehaviour where T : Dynamic<T>
      {
            private static T _instance; public static T Instance
            {
                  get
                  {
                        if (!Application.isPlaying) return null;

                        _instance = FindObjectOfType<T>(includeInactive: false);
                        if (_instance == null)
                        {
                              _instance = new GameObject($"{typeof(T).Name} : Singleton").AddComponent<T>();
                        }
                        return _instance;
                  }
            }


            /// <summary>
            /// Initializes the singleton instance of type <typeparamref name="T"/>.
            /// </summary>
            /// <param name="persistent">If true, the singleton instance will persist across scene loads using <see cref="Object.DontDestroyOnLoad"/>.</param>
            /// <remarks>If a duplicate instance is detected, the current gameObject will be destroyed, and the existing singleton instance will remain.</remarks>
            protected void Initialize(bool persistent)
            {
                  if (_instance == null || _instance == this)
                  {
                        _instance = this as T;
                        if (persistent) DontDestroyOnLoad(gameObject);
                        return;
                  }
                  Debug.LogWarning($"Duplicate instance of type '{typeof(T).FullName}' detected. Destroying gameObject '{name}'.");
                  Destroy(gameObject);
            }

            protected virtual void OnDestroy() => _instance = _instance == this ? null : _instance;
      }
}