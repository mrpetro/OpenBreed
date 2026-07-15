using OpenBreed.Common.Interface.Drawing;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Extensions
{
    public static class ColorExtensions
    {
        #region Public Methods

        public static Color4<Rgba> ToColor4(this MyColor color)
        {
            return new Color4<Rgba>(
                color.R / 255.0f,
                color.G / 255.0f,
                color.B / 255.0f,
                color.A / 255.0f);
        }

        #endregion Public Methods
    }
}