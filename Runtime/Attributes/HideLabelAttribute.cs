namespace Emp37.Utility
{
      /// <summary>
      /// Attribute used to hide the label of a serialized field in the inspector.
      /// </summary>
      public class HideLabelAttribute : LabelAttribute
      {
            public HideLabelAttribute() : base(string.Empty)
            {
            }
      }
}