using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Abstractions.Extensions
{
    public static class Color4Extensions
    {
        public static Color4 Multiply(this Color4 color, float value)
        {
            return new Color4(
                color.R * value,
                color.G * value,
                color.B * value,
                color.A);
        }
    }
}
