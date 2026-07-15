using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Presentations
{
    public class ButtonPresentation
    {
        #region Public Properties

        public static Color4<Rgba> LightSideColor { get; } = new Color4<Rgba>(130, 130, 130, 20);
        public static Color4<Rgba> FlatSideColor { get; } = new Color4<Rgba>(100, 100, 100, 20);
        public static Color4<Rgba> DarkSideColor { get; } = new Color4<Rgba>(70, 70, 70, 20);

        #endregion Public Properties
    }
}