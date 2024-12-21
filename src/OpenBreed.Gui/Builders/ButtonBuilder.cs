using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Elements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Builders
{
    internal class ButtonBuilder : ElementBuilder, IButtonBuilder
    {
        #region Internal Fields

        internal Action<IButton> ClickAction;

        #endregion Internal Fields

        #region Public Constructors

        public ButtonBuilder(IElementBuilder parentBuilder)
                    : base(parentBuilder)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        #endregion Public Methods

        #region Internal Methods

        internal override Element InternalBuild()
        {
            return new Button(this);
        }

        #endregion Internal Methods
    }
}