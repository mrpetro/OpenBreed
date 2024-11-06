using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Interface.Builders;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Interface
{
    internal class InteractivePanel : InteractiveElement, IInteractivePanel
    {
        #region Internal Constructors

        internal InteractivePanel(InteractivePanelBuilder builder) : base(builder)
        {
            FillColor = builder.FillColor;
            BorderColor = builder.BorderColor;
        }

        #endregion Internal Constructors

        #region Public Properties

        public Color4 FillColor { get; }

        public Color4 BorderColor { get; }

        #endregion Public Properties
    }
}