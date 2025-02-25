namespace Emp37.Utility
{
      /// <summary>
      /// Attribute for displaying titles above fields in the inspector.
      /// </summary>
      public class StylizedTitleAttribute : TitleAttribute
      {
            public StylizedTitleAttribute(string title) : base(Utility.ToStylizedTitleCase(title)) { }
            public StylizedTitleAttribute(string title, Shade shade) : base(Utility.ToStylizedTitleCase(title), shade) { }
            public StylizedTitleAttribute(string title, Shade text, Shade underline) : base(Utility.ToStylizedTitleCase(title), text, underline) { }
      }
}