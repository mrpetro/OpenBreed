using OpenBreed.Gui.Interface.Elements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Builders
{
    internal class InteractivePanelBuilder : InteractiveElementBuilder, IInteractivePanelBuilder
    {
        #region Internal Fields

        internal Color4 BorderColor;
        internal Color4 FillColor;

        #endregion Internal Fields

        #region Public Constructors

        public InteractivePanelBuilder(InteractionCore interactionCore, InteractiveElementBuilder parentBuilder)
                    : base(interactionCore, parentBuilder)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void SetBorderColor(Color4 color)
        {
            BorderColor = color;
        }

        public void SetFillColor(Color4 color)
        {
            FillColor = color;
        }

        #endregion Public Methods

        #region Internal Methods

        internal override InteractiveElement InternalBuild()
        {
            return new InteractivePanel(this);
        }

        #endregion Internal Methods
    }
}