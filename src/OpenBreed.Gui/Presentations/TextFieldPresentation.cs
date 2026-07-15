using OpenBreed.Gui.Abstractions.Presentations;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Presentations
{
    public class TextFieldPresentation : ElementPresentation, ITextFieldPresentation
    {
        #region Public Constructors

        public TextFieldPresentation(string fontName, int fontSize)
        {
            FontName = fontName;
            FontSize = fontSize;
        }

        #endregion Public Constructors

        #region Public Properties

        public static Color4<Rgba> BackgroundColor { get; } = new Color4<Rgba>(255, 255, 255, 20);
        public static Color4<Rgba> BorderColor { get; } = new Color4<Rgba>(127, 127, 127, 20);
        public static Color4<Rgba> PointerColor { get; } = new Color4<Rgba>(0, 0, 0, 20);

        public string FontName { get; }
        public int FontSize { get; }

        #endregion Public Properties
    }
}