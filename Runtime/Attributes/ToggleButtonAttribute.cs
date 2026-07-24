using System;

namespace Emp37.Utility
{
        /// <summary>
        /// Attribute used to display a serialized boolean field as a toggle button.    
        /// </summary>
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
        public class ToggleButtonAttribute : UnityEngine.PropertyAttribute
        {
                public readonly float Height = 18F;

                public ToggleButtonAttribute(Size size = Size.Default) => Height = size switch { Size.Large => 24F, Size.ExtraLarge => 32F, _ => Height };
        }
}