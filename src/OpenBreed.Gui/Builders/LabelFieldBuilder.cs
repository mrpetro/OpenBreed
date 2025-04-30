using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Controllers;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Elements;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Gui.Builders
{
    internal class LabelFieldBuilder : ElementBuilder<ILabelField>, ILabelFieldBuilder
    {
        #region Internal Fields

        internal string Text = string.Empty;
        internal string FontName = "Arial";
        internal int FontSize = 12;
        internal HorizontalAlignment HorizontalAlignment;
        internal VerticalAlignment VerticalAlignment;

        #endregion Internal Fields

        #region Private Fields

        private readonly IFontMan fontMan;

        #endregion Private Fields

        #region Public Constructors

        public LabelFieldBuilder(IElementInputController inputHandler, IFontMan fontMan) : base(inputHandler)
        {
            this.fontMan = fontMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void SetText(string? text)
        {
            Text = text ?? string.Empty;
        }

        public void SetFontSize(int size)
        {
            FontSize = size;
        }

        public void SetFontName(string name)
        {
            FontName = name;
        }

        public void SetHorizontalAlignment(HorizontalAlignment horizontalAlignment)
        {
            HorizontalAlignment = horizontalAlignment;
        }

        public void SetVerticalAlignment(VerticalAlignment verticalAlignment)
        {
            VerticalAlignment = verticalAlignment;
        }

        #endregion Public Methods

        #region Internal Methods

        internal IFont GetFont()
        {
            return fontMan.GetOSFont(FontName, FontSize);
        }

        public override ILabelField Build()
        {
            return new LabelField(this);
        }

        #endregion Internal Methods
    }
}