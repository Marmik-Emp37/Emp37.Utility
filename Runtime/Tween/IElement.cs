namespace Emp37.Utility.Tweening
{
      public interface IElement
      {
            public bool IsEmpty { get; }
            public bool IsComplete { get; }

            internal void Update();
      }
}