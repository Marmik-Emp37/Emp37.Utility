using System.Collections.Generic;

using UnityEngine;

namespace Emp37.Utility.Tween
{
      public class Engine : MonoBehaviour
      {
            private static Engine instance;
            private static readonly List<Element> tweens = new();


            private void LateUpdate()
            {
                  int count = tweens.Count;
                  for (int i = count - 1; i >= 0; i--)
                  {
                        var context = tweens[i];
                        context.Update();
                        if (context.IsComplete)
                        {
                              tweens.RemoveAt(i);
                        }
                  }
                  if (count == 0)
                  {
                        enabled = false;
                  }
            }
            private void OnDestroy()
            {
                  instance = null;
            }

            private static void Initialize()
            {
                  instance = FindFirstObjectByType<Engine>();
                  if (instance == null)
                  {
                        instance = new GameObject(typeof(Engine).Name).AddComponent<Engine>();
                  }
                  DontDestroyOnLoad(instance);
            }
            public static void Push(Element tween)
            {
                  if (!Application.isPlaying || tween.IsInvalid) return;

                  if (instance == null) Initialize();

                  int index = tweens.FindIndex(existingTween => existingTween.ConflictsWith(tween));
                  if (index != -1)
                  {
                        tweens.RemoveAt(index);
                  }
                  tweens.Add(tween);
                  instance.enabled = true;
            }
      }
}