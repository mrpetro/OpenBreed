using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Controllers;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Elements;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;

namespace OpenBreed.Gui.Builders
{
    internal class TextFieldBuilder : ElementBuilder<ITextField>, ITextFieldBuilder
    {
        #region Internal Fields

        internal string Text = string.Empty;
        internal string FontName = "Arial";
        internal int FontSize = 12;
        internal PropertyBinding<string>? TextBinding { get; private set; }

        #endregion Internal Fields

        #region Private Fields

        private readonly IFontMan fontMan;


        #endregion Private Fields

        #region Public Constructors

        public TextFieldBuilder(IElementInputController<ITextField> inputController, IFontMan fontMan) : base(inputController)
        {
            this.fontMan = fontMan;

            SetMinimumSize(100, 25);
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

        public void BindProperty(PropertyBinding<string>? binding)
        {
            TextBinding = binding;
        }

        #endregion Public Methods

        #region Internal Methods

        public override ITextField Build()
        {
            return new TextField(this);
        }

        internal IFontAtlas GetFont()
        {
            return fontMan.GetOSFont(FontName, FontSize);
        }

        #endregion Internal Methods
    }
}