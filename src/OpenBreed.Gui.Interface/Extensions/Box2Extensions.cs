using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Core.Interface.Extensions
{
    public static class Box2Extensions
    {
        #region Public Methods

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