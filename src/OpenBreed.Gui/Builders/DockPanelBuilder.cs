using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Controllers;
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
    internal class DockPanelBuilder : ElementBuilder<IDockPanel>, IDockPanelBuilder
    {
        #region Public Constructors

        public DockPanelBuilder(IElementInputHandler inputHandler) : base(inputHandler)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override IDockPanel Build()
        {
            return new DockPanel(this);
        }

        #endregion Public Methods
    }
}