using System;

namespace Emp37.Utility
{
      [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
      public class MethodAttribute : Attribute { }
}