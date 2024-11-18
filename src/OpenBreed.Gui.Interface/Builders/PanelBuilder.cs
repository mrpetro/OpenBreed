using OpenBreed.Gui.Interface.Elements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Builders
{
    internal class PanelBuilder : ElementBuilder, IPanelBuilder
    {
        #region Public Constructors

        public PanelBuilder(ElementBuilder parentBuilder)
                    : base(parentBuilder)
        {
        }

        #endregion Public Constructors

        #region Internal Methods

        internal override Element InternalBuild()
        {
            return new Panel(this);
        }

        #endregion Internal Methods
    }
}