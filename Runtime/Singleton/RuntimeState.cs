using UnityEngine;

namespace Emp37.Utility.Singleton
{
	internal static class RuntimeState
	{
		public static bool IsQuitting { get; private set; }

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Reset()
		{
			IsQuitting = false;
			Application.quitting -= HandleApplicationQuitting;
			Application.quitting += HandleApplicationQuitting;
		}

		private static void HandleApplicationQuitting() => IsQuitting = true;
	}
}