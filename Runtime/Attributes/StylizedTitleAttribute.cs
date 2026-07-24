using System;

namespace Emp37.Utility
{
        /// <summary>
        /// Attribute for displaying stylized titles above fields in the inspector.
        /// </summary>
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
        public class StylizedTitleAttribute : TitleAttribute
      {
            public StylizedTitleAttribute(string title) : base(title.ToStylizedTitleCase()) { }
            public StylizedTitleAttribute(string title, Shade shade) : base(title.ToStylizedTitleCase(), shade) { }
      }
}