using OpenBreed.Core;
using System;

namespace OpenBreed.Game
{
    public static class Tools
    {
        #region Public Methods

        public static float GetZoom(ICore core, float zoom)
        {
            float scaleFactor = 1.0f;

            if (Math.Sign(core.Inputs.WheelDelta) > 0)
                scaleFactor = 1.25f;
            else if (Math.Sign(core.Inputs.WheelDelta) < 0)
                scaleFactor = 0.75f;

            zoom *= scaleFactor;

            if (zoom < 0.125f)
                zoom = 0.125f;
            else if (zoom > 8.0f)
                zoom = 8.0f;

            return zoom;
        }

        #endregion Public Methods
    }
}