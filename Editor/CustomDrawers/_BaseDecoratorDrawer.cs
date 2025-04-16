using UnityEngine;

namespace Emp37.Utility.Editor
{
      internal abstract class BaseDecoratorDrawer : UnityEditor.DecoratorDrawer
      {
            private bool hasInitialized;


            public virtual void Initialize() { }
            public abstract void Draw(Rect position);

            public sealed override void OnGUI(Rect position)
            {
                  if (!hasInitialized)
                  {
                        Initialize();
                        hasInitialized = true;
                  }
                  Draw(position);
            }
      }
}