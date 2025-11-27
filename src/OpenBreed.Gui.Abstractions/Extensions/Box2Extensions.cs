using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Core.Abstractions.Extensions
{
    public static class Box2Extensions
    {
        #region Public Methods

        public static (Box2 Bottom, Box2 Top) SplitHorizontally(this Box2 box, float splitPosition)
        {
            return (new Box2(
                        box.Min.X,
                        box.Min.Y,
                        box.Max.X,
                        box.Min.Y + splitPosition),
                    new Box2(
                        box.Min.X,
                        box.Min.Y + splitPosition,
                        box.Max.X,
                        box.Max.Y));
        }

        public static (Box2 Left, Box2 Right) SplitVertically(this Box2 box, float splitPosition)
        {
            return (new Box2(
                        box.Min.X,
                        box.Min.Y,
                        box.Min.X + splitPosition,
                        box.Max.Y),
                    new Box2(
                        box.Min.X + splitPosition,
                        box.Min.Y,
                        box.Max.X,
                        box.Max.Y));
        }

        public static Box2 Deflate(this Box2 box, float value)
        {
            return new Box2(
                box.Min.X + value,
                box.Min.Y + value,
                box.Max.X - value,
                box.Max.Y - value);
        }

        public static Box2 Deflate(this Box2 box, Box2 value)
        {
            return new Box2(
                box.Min.X + value.Min.X,
                box.Min.Y + value.Min.Y,
                box.Max.X - value.Max.X,
                box.Max.Y - value.Max.Y);
        }

        #endregion Public Methods
    }
}