using OpenBreed.Gui.Abstractions.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Builders
{
    public interface ILabelFieldBuilder : IElementBuilder
    {
        #region Public Methods

        void SetHorizontalAlignment(HorizontalAlignment horizontalAlignment);

        void SetVerticalAlignment(VerticalAlignment verticalAlignment);

        void SetText(string? text);

        void SetFontSize(int size);

        void SetFontName(string name);

        #endregion Public Methods
    }
}