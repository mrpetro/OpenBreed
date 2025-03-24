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
    internal class ButtonBuilder : ElementBuilder<IButton>, IButtonBuilder
    {
        #region Internal Fields

        internal Action<IButton> ClickAction;

        #endregion Internal Fields

        #region Public Constructors

        public ButtonBuilder()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override IButton Build()
        {
            return new Button(this);
        }

        #endregion Public Methods
    }
}