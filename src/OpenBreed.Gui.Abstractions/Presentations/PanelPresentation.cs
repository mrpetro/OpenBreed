using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Presentations
{
    public class PanelPresentation
    {
        #region Public Properties

        public static Color4<Rgba> FillColor { get; } = new Color4<Rgba>(100, 100, 100, 255);
        public static Color4<Rgba> BorderColor { get; } = new Color4<Rgba>(70, 70, 70, 255);

        #endregion Public Properties
    }
}