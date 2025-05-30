using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace Emp37.Utility
{
      /// <summary>
      /// Efficiently retrieves and caches attributes applied to types.
      /// </summary>
      public static class AttributeCache
      {
            private static readonly ConcurrentDictionary<(ICustomAttributeProvider, Type), Attribute[]> _cache = new();

            private static bool TryFetchAll<TAttribute>(ICustomAttributeProvider provider, out TAttribute[] results, bool inherit = false) where TAttribute : Attribute
            {
                  var key = (provider, typeof(TAttribute));
                  if (!_cache.TryGetValue(key, out Attribute[] attributes))
                  {
                        attributes = provider switch
                        {
                              Type t => Attribute.GetCustomAttributes(t, typeof(TAttribute), inherit),
                              MemberInfo m => Attribute.GetCustomAttributes(m, typeof(TAttribute), inherit),
                              _ => Array.Empty<Attribute>()
                        };
                        _cache[key] = attributes;
                        Debug.Log(provider);
                  }
                  results = (TAttribute[]) attributes;
                  return results.Length > 0;
            }

            public static T[] FetchAll<T>(ICustomAttributeProvider provider, bool inherit = false) where T : Attribute => TryFetchAll(provider, out T[] results, inherit) ? results : Array.Empty<T>();
            public static T FetchFirst<T>(ICustomAttributeProvider provider, bool inherit = false) where T : Attribute => FetchAll<T>(provider, inherit).FirstOrDefault();
            public static bool Contains<T>(ICustomAttributeProvider provider, bool inherit = false) where T : Attribute => TryFetchAll<T>(provider, out _, inherit);
      }
}