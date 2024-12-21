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

        public static Color4 BorderColor { get; } = new Color4(0, 0, 0, 255);
        public static Color4 BackgroundColor { get; } = new Color4(255, 255, 255, 255);
        public static Color4 SymbolColor { get; } = new Color4(0, 0, 0, 255);

        #endregion Public Properties
    }
}