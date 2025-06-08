using System.Collections.Generic;

using UnityEngine;

namespace Emp37.Utility.Tweening
{
      public class Engine : MonoBehaviour
      {
            private static Engine instance;

            private static readonly List<Element> tweens = new();


            private void Awake()
            {
                  if (instance != null && instance != this)
                  {
                        Destroy(gameObject);
                        return;
                  }
                  instance = this;
                  DontDestroyOnLoad(gameObject);
                  enabled = false;
            }
            private void LateUpdate()
            {
                  int count = tweens.Count;
                  for (int i = count - 1; i >= 0; i--)
                  {
                        Element element = tweens[i];
                        if (element.IsComplete)
                        {
                              int lastIndex = count - 1;
                              if (i != lastIndex) tweens[i] = tweens[lastIndex];
                              tweens.RemoveAt(lastIndex);
                        }
                        else
                        {
                              element.Update();
                        }
                  }
                  if (count == 0)
                  {
                        enabled = false;
                  }
            }
            private void OnDestroy()
            {
                  if (instance == this) instance = null;
            }

            public static bool Add(Element tween)
            {
                  if (!Application.isPlaying || tween.IsInvalid) return false;

                  instance ??= new GameObject(typeof(Engine).Name).AddComponent<Engine>();

                  tweens.Add(tween);
                  instance.enabled = true;

                  return true;
            }
      }
}