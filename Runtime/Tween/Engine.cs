using System.Collections.Generic;

using UnityEngine;

namespace Emp37.Utility.Tweening
{
      public class Engine : MonoBehaviour
      {
            private static Engine instance;

            private static readonly Dictionary<object, Element> tweens = new();


            private void Awake()
            {
                  if (instance != null && instance != this)
                  {
                        Destroy(gameObject);
                        return;
                  }
                  DontDestroyOnLoad(gameObject);
                  enabled = false;
            }
            private void LateUpdate()
            {
                  if (tweens.Count == 0)
                  {
                        enabled = false;
                        return;
                  }
                  List<object> keysToRemove = new();
                  foreach (var pair in tweens)
                  {
                        Element element = pair.Value;
                        if (!element.IsComplete)
                        {
                              element.Update();
                        }
                        else
                        {
                              keysToRemove.Add(pair.Key);
                        }
                  }
                  foreach (var key in keysToRemove)
                  {
                        tweens.Remove(key);
                  }
                  if (tweens.Count == 0)
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

                  if (tween.Key != null)
                  {
                        tweens[tween.Key] = tween;
                  }
                  instance.enabled = true;
                  return true;
            }
      }
}