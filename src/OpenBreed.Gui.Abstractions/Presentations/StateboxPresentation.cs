using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Presentations
{
    public class StateboxPresentation
    {
        #region Public Properties

        public static Color4<Rgba> BorderColor { get; } = new Color4<Rgba>(0, 0, 0, 255);
        public static Color4<Rgba> BackgroundColor { get; } = new Color4<Rgba>(255, 255, 255, 255);
        public static Color4<Rgba> SymbolColor { get; } = new Color4<Rgba>(0, 0, 0, 255);

        #endregion Public Properties
    }
}