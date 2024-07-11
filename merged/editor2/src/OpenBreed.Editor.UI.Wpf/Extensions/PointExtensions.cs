using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OpenBreed.Editor.UI.Wpf.Extensions
{
    public static class PointExtensions
    {
        #region Public Methods

        public static MyPoint ToMyPoint(this Point point)
        {
            return new MyPoint((int)point.X, (int)point.Y);
        }

        #endregion Public Methods
    }
}