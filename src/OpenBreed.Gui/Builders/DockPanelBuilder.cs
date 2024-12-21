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
    internal class DockPanelBuilder : ContainerBuilder, IDockPanelBuilder
    {
        #region Public Constructors

        public DockPanelBuilder(IElementBuilder parentBuilder)
                    : base(parentBuilder)
        {
        }

        #endregion Public Constructors

        #region Internal Methods

        internal override Element InternalBuild()
        {
            return new DockPanel(this);
        }

        #endregion Internal Methods
    }
}