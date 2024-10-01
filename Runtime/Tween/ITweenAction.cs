using UnityEngine;

namespace Emp37.Utility.Tween
{
#pragma warning disable IDE1006 // Naming Styles
      public interface ITweenAction
      {
            public enum Type
            {
                  None,
                  Move, MoveLocal, Rotate, Scale, // transform
                  CanvasAlpha, // canvasGroup
                  SpriteAlpha, SpriteTint // spriteRenderer
            }

            public Element executeMove(Vector3? value = null);
            public Element executeMoveLocal(Vector3? value = null);
            public Element executeRotate(Vector3? value = null);
            public Element executeScale(Vector3? value = null);
            public Element executeCanvasAlpha(float? value = null);
            public Element executeSpriteAlpha(float? value = null);
            public Element executeSpriteTint(Color? value = null);
      }
#pragma warning restore IDE1006
}