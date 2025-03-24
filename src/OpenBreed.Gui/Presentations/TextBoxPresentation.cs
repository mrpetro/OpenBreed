using OpenBreed.Gui.Abstractions.Presentations;

namespace OpenBreed.Gui.Presentations
{
    public class TextBoxPresentation : ElementPresentation, ITextBoxPresentation
    {
        #region Public Constructors

        public TextBoxPresentation(string fontName, int fontSize)
        {
            FontName = fontName;
            FontSize = fontSize;
        }

        #endregion Public Constructors

        #region Public Properties

        public string FontName { get; }
        public int FontSize { get; }

        #endregion Public Properties
    }
}