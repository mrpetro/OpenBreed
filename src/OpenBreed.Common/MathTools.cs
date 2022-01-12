using System;

namespace OpenBreed.Common
{
    public static class MathTools
    {
        #region Public Methods

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