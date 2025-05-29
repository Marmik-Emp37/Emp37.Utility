using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Emp37.Utility.Editor
{
      public static class AttributeCache
      {
            private static readonly ConcurrentDictionary<Type, Attribute[]> cache = new();

            public static bool Has<T>(Type type) where T : Attribute => GetAll<T>(type).Length > 0;
            public static T Get<T>(Type type) where T : Attribute => GetAll<T>(type).FirstOrDefault();
            public static T[] GetAll<T>(Type type) where T : Attribute
            {
                  if (!cache.TryGetValue(type, out var attributes))
                  {
                        attributes = type.GetCustomAttributes(false).OfType<Attribute>().ToArray();
                        cache[type] = attributes;
                  }
                  return attributes.OfType<T>().ToArray();
            }
      }
}
