using OpenBreed.Gui.Abstractions.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Builders
{
    public interface ITextFieldBuilder : IElementBuilder
    {
        #region Public Methods

        void SetText(string? text);

        void SetFontSize(int size);

        void SetFontName(string name);

        #endregion Public Methods
    }
}