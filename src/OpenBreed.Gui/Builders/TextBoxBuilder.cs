using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Elements;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Gui.Builders
{
    internal class TextBoxBuilder : ElementBuilder<ITextBox>, ITextBoxBuilder
    {
        #region Internal Fields

        internal string Text = string.Empty;
        internal string FontName = "Arial";
        internal int FontSize = 12;

        #endregion Internal Fields

        #region Private Fields

        private readonly IFontMan fontMan;

        #endregion Private Fields

        #region Public Constructors

        public TextBoxBuilder(IFontMan fontMan)
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

        #endregion Public Methods

        #region Internal Methods

        public override ITextBox Build()
        {
            return new TextBox(this);
        }

        internal IFont GetFont()
        {
            return fontMan.GetOSFont(FontName, FontSize);
        }

        #endregion Internal Methods
    }
}