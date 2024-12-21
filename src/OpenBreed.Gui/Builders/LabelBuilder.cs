using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Elements;

namespace OpenBreed.Gui.Builders
{
    internal class LabelBuilder : ElementBuilder, ILabelBuilder
    {
        #region Internal Fields

        internal string Text = string.Empty;
        internal HorizontalAlignment HorizontalAlignment;
        internal VerticalAlignment VerticalAlignment;

        #endregion Internal Fields

        #region Public Constructors

        public LabelBuilder(IElementBuilder parentBuilder)
                    : base(parentBuilder)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void SetText(string? text)
        {
            Text = text ?? string.Empty;
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

        internal override Element InternalBuild()
        {
            return new Label(this);
        }

        #endregion Internal Methods
    }
}