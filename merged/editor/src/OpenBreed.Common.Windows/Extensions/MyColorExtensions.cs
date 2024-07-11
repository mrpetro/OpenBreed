using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Extensions
{
    public static class MyColorExtensions
    {
        #region Public Methods

        public static Color ToColor(this MyColor color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static MyColor ToMyColor(this Color color)
        {
            return MyColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        #endregion Public Methods
    }
}