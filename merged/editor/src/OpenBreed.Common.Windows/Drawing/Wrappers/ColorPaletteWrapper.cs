using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Windows.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Drawing.Wrappers
{
    public class ColorPaletteWrapper : IColorPalette
    {
        #region Public Constructors

        public ColorPaletteWrapper(ColorPalette wrapped)
        {
            Wrapped = wrapped;
        }

        #endregion Public Constructors

        #region Public Properties

        public MyColor[] Entries => Wrapped.Entries.Select(c => c.ToMyColor()).ToArray();

        public ColorPalette Wrapped { get; }

        #endregion Public Properties
    }
}
