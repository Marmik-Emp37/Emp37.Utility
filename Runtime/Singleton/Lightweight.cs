using UnityEngine;

namespace Emp37.Utility.Singleton
{
	/// <summary>
	/// A simple MonoBehaviour-based singleton to use at runtime.
	/// </summary>
	/// <typeparam name="T">The type of the singleton, which must inherit from <see cref="Lightweight{T}"/>.</typeparam>
	/// <remarks>
	/// <b>NOTE:</b> This implementation -
	/// <br>• Loses its instance if the domain reloads mid-play (Preferences → General → Script Changes While Playing → 'Recompile And Continue Playing'). The static is cleared but Awake does not re-run, so Instance stays null until the next scene load.</br>
	/// <br>• Requires explicit placement of component T on the scene.</br>
	/// <br>• Registers during <c>Awake</c>, so <see cref="Instance"/> may still be <see langword="null"/> for scripts that read it from their own <c>Awake</c>. Read it from <c>Start</c>, or give the subclass a negative <see cref="DefaultExecutionOrder"/>.</br>
	/// <br></br>
	/// <br>Usage example for a singleton of type <see cref="Lightweight{T}"/>:</br>
	/// <code>
	/// public class MySingleton : Lightweight&lt;MySingleton&gt;
	/// {
	///     protected override void Awake()
	///     {
	///         base.Awake(); // required, this is what registers the instance
	///     }
	/// }
	/// </code>
	/// </remarks>
	public abstract class Lightweight<T> : MonoBehaviour where T : Lightweight<T> // ~Hextant Studios
	{
		public static T Instance { get; private set; }

		protected virtual void Awake()
		{
			Debug.Assert(condition: Instance == null, message: $"Multiple instances of type '{typeof(T).Name}' exist on this scene.", context: this);

			Instance = this as T;
		}
		protected virtual void OnDestroy()
		{
			if (Instance == this) Instance = null;
		}
	}
}