namespace Emp37.Utility
{
      /// <summary>
      /// Attribute for displaying titles above fields in the inspector.
      /// </summary>
      public class StylizedTitleAttribute : TitleAttribute
      {
            public StylizedTitleAttribute(string title) : base(title.ToStylizedTitleCase()) { }
            public StylizedTitleAttribute(string title, Shade shade) : base(title.ToStylizedTitleCase(), shade) { }
            public StylizedTitleAttribute(string title, Shade text, Shade underline) : base(title.ToStylizedTitleCase(), text, underline) { }
      }
}