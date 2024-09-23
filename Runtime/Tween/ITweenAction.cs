namespace Emp37.Utility.Tween
{
#pragma warning disable IDE1006 // Naming Styles
      public interface ITweenAction
      {
            public Element executeMove();
            public Element executeRotate();
            public Element executeScale();
            public Element executeAlpha();
      }
#pragma warning restore IDE1006
}