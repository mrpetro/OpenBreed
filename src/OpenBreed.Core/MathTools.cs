using OpenTK;
using System;

namespace OpenBreed.Core
{
    public static class MathTools
    {
        #region Public Methods

        public static float AngleBetween(Vector2 a, Vector2 b)
        {
            return (float)Math.Atan2(b.Y - a.Y, b.X - a.X);
        }

        public static object Lerp(object start, object end, float by)
        {
            if (start is float && end is float)
                return Lerp((float)start, (float)end, by);
            else
                throw new NotImplementedException();
        }

        public static float Lerp(float start, float end, float by)
        {
            return start + (end - start) * by;
        }

        #endregion Public Methods
    }
}