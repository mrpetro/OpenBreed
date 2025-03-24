using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Presentations
{
    public class DesktopPresentation
    {
        #region Public Properties

        public static Color4 LightSideColor { get; } = new Color4(130, 130, 130, 255);
        public static Color4 FlatSideColor { get; } = new Color4(100, 100, 100, 255);
        public static Color4 DarkSideColor { get; } = new Color4(70, 70, 70, 255);

        #endregion Public Properties
    }
}