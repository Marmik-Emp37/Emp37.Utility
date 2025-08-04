using UnityEngine;

using UnityEditor;
using UnityEditorInternal;

namespace Emp37.Utility.Editor
{
	internal static class ExtractComponent //~Warped Imagination
	{
		private enum Mode { Detached, Child }

		[MenuItem("CONTEXT/Component/Extract To Child", priority = 525)] private static void ExtractToChildMenuOption(MenuCommand command) => Extract(command, Mode.Child);
		[MenuItem("CONTEXT/Component/Extract", priority = 524)] private static void ExtractMenuOption(MenuCommand command) => Extract(command, Mode.Detached);

		private static void Extract(MenuCommand command, Mode mode)
		{
			if (command.context is not Component source || source is Transform) return;

			string name = source.GetType().Name.ToTitleCase();

			Undo.IncrementCurrentGroup();
			int undoGroup = Undo.GetCurrentGroup();

			GameObject child = new(name);
			Undo.RegisterCreatedObjectUndo(child, "Extracted Component");
			child.transform.SetParent(mode switch { Mode.Child => source.transform, _ => null }, worldPositionStays: false);

			if (!ComponentUtility.CopyComponent(source) || !ComponentUtility.PasteComponentAsNew(child))
			{
				Debug.LogError($"Unable to extract component '{name}' from object '{source.name}'.", source.gameObject);
				Undo.CollapseUndoOperations(undoGroup);
				return;
			}

			Undo.DestroyObjectImmediate(source);
			Undo.CollapseUndoOperations(undoGroup);
		}
	}
}