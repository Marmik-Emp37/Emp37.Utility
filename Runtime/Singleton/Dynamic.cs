using UnityEngine;

namespace Emp37.Utility.Singleton
{
	/// <summary>
	/// Determines what happens to a second instance of a singleton when one already exists.
	/// </summary>
	public enum DuplicateAction : byte
	{
		DestroyGameObject,
		DestroyComponent,
		KeepAndWarn,
	}

	/// <summary>
	/// A dynamic MonoBehaviour based singleton to use at runtime.
	/// </summary>
	/// <remarks>
	/// <b>NOTE:</b> This implementation -
	/// <br>• Requires calling <see cref="Initialize(bool, DuplicateAction)"/> to register the singleton instance.</br>
	/// <br>• Resolves an unregistered instance on first access, creating one if none is found.</br>
	/// <br>• Is main-thread only, like the rest of the Unity API.</br>
	/// <br></br>
	/// <br>Usage example for a singleton of type <see cref="Dynamic{T}"/>:</br>
	/// <code>
	/// public class MySingleton : Dynamic&lt;MySingleton&gt;
	/// {
	///     private void Awake()
	///     {
	///         Initialize(persistent: true); // make this singleton persist across scenes
	///     }
	/// }
	/// </code>
	/// </remarks>
	[DisallowMultipleComponent]
	public abstract class Dynamic<T> : MonoBehaviour where T : Dynamic<T>
	{
		private static T instance;

		/// <summary>
		/// Returns the registered instance if it exists, otherwise searches the scenes (including inactive objects) and, failing that, creates a new GameObject to host one.
		/// Returns <see langword="null"/> once the application has begun quitting.
		/// </summary>
		public static T Instance
		{
			get
			{
				if (instance != null) return instance;
				if (RuntimeState.IsQuitting)
				{
					Debug.LogWarning($"Cannot access '{typeof(T).Name}' because the application is quitting.");
					return null;
				}
				instance = FindAnyObjectByType<T>(FindObjectsInactive.Include) ?? new GameObject(typeof(T).Name).AddComponent<T>();
				return instance;
			}
		}

		protected virtual void OnDestroy()
		{
			if (ReferenceEquals(instance, this)) instance = null;
		}

		/// <summary>
		/// Registers this object as the singleton instance, or disposes of it if one is already registered.
		/// </summary>
		/// <param name="persistent">
		/// When <see langword="true"/>, keeps the object alive across scene loads via <see cref="Object.DontDestroyOnLoad(Object)"/>.
		/// <br>Unity ignores this and logs a warning if the GameObject has a parent.</br>
		/// </param>
		/// <param name="actionOnDuplicate">How to dispose of this object if an instance is already registered.</param>
		protected void Initialize(bool persistent = false, DuplicateAction actionOnDuplicate = 0)
		{
			T self = this as T;

			if (instance != null && !ReferenceEquals(instance, self))
			{
				switch (actionOnDuplicate)
				{
					case DuplicateAction.DestroyGameObject:
						Warn("Destroying the GameObject."); Destroy(gameObject);
						break;
					case DuplicateAction.DestroyComponent:
						Warn("Destroying the component."); Destroy(this);
						break;
					case DuplicateAction.KeepAndWarn:
						Warn("The duplicate will remain active.");
						break;
				}
				return;

				void Warn(string message) => Debug.LogWarning($"Found duplicate '{typeof(T).Name}' on '{name}'. {message}", gameObject);
			}
			instance = self;

			if (persistent) DontDestroyOnLoad(this);
		}
	}
}