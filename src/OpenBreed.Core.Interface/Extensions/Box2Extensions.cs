using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Core.Interface.Extensions
{
    public static class Box2Extensions
    {
        #region Public Methods

        public static Box2 Inflate(this Box2 box, float value)
        {
            return new Box2(box.Min.X - value, box.Min.Y - value, box.Max.Y + value, box.Max.X + value);
        }

        #endregion Public Methods
    }
}