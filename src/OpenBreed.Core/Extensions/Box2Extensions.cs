using OpenTK;

namespace OpenBreed.Core.Extensions
{
    public static class Box2Extensions
    {
        #region Public Methods

        public static Box2 Inflate(this Box2 box, float value)
        {
            return Box2.FromTLRB(box.Top + value, box.Left - value, box.Right + value, box.Bottom - value);
        }

        #endregion Public Methods
    }
}