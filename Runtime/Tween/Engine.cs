using System.Collections.Generic;

using UnityEngine;

using Emp37.Utility.Tween;

namespace Emp37.Utility
{
      public class Engine : MonoBehaviour
      {
            private static readonly List<Element> tweens = new();


            private void Update()
            {
                  for (int i = 0; i < tweens.Count; i++)
                  {
                        var context = tweens[i];
                        context.Update();
                        if (context.IsComplete)
                        {
                              tweens.RemoveAt(i);
                        }
                  }
                  if (tweens.Count == 0)
                  {
                        enabled = false;
                  }
            }


            public void Push(Element tween)
            {
                  if (tween.IsValid && Application.isPlaying)
                  {
                        tweens.Add(tween);
                        enabled = true;
                  }
            }
      }
}
