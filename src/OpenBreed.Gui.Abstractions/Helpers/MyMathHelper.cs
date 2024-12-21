using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Helpers
{
    public static class MyMathHelper
    {
        #region Public Methods

        public static Vector2 Clamp(Vector2 v, Vector2 min, Vector2 max)
        {
            return new Vector2(
                MathHelper.Clamp(v.X, min.X, max.X),
                MathHelper.Clamp(v.Y, min.Y, max.Y));
        }

        #endregion Public Methods
    }
}