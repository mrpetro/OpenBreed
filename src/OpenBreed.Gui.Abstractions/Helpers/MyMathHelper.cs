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
                Math.Clamp(v.X, min.X, max.X),
                Math.Clamp(v.Y, min.Y, max.Y));
        }

        public static float Snap(float original, int numerator, int denominator)
        {
            return (float)Math.Round(original * denominator / numerator) * numerator / denominator;
        }

        public static Vector2 Snap(Vector2 original, (int Numerator, int Denominator) snapX, (int Numerator, int Denominator) snapY)
        {
            var x = (float)Math.Round(original.X * snapX.Denominator / snapX.Numerator) * snapX.Numerator / snapX.Denominator;
            var y = (float)Math.Round(original.Y * snapY.Denominator / snapY.Numerator) * snapY.Numerator / snapY.Denominator;
            //var x = (int)(original.X / step.X + 0.5f) * step.X;
            //var y = (int)(original.Y / step.Y + 0.5f) * step.Y;

            return new Vector2(x, y);
        }

        #endregion Public Methods
    }
}