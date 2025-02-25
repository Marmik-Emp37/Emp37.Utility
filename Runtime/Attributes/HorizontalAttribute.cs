using System;

namespace Emp37.Utility
{
      [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
      public class HorizontalAttribute : Attribute
      {
            public readonly bool BeginGroup;

            public HorizontalAttribute(bool begin = true) => BeginGroup = begin;
      }
}