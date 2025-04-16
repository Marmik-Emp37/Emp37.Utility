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
                  Component source = command.context as Component;
                  string name = Utility.ToTitleCase(source.GetType().Name);
                  int undoGroup = Undo.GetCurrentGroup();
                  Undo.IncrementCurrentGroup();

                  GameObject child = new(name);
                  child.transform.SetParent(mode switch { Mode.Child => source.transform, _ => null });
                  Undo.RegisterCreatedObjectUndo(child, "Extracted object.");

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