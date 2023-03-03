using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Drawing;

namespace OpenBreed.Sandbox.Helpers
{
    public static class PaletteHelper
    {
        #region Public Methods

        public static Color4 ToColor4(Color color)
        {
            return new Color4(
                color.R / 255.0f,
                color.G / 255.0f,
                color.B / 255.0f,
                color.A / 255.0f);
        }

        #endregion Public Methods
    }
}