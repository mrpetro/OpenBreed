using OpenBreed.Gui.Interface.Elements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Builders
{
    internal class ButtonBuilder : ElementBuilder, IButtonBuilder
    {
        #region Internal Fields

        internal Action<IButton> ClickAction;

        #endregion Internal Fields

        #region Public Constructors

        public ButtonBuilder(ElementBuilder parentBuilder)
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