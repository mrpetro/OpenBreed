using Microsoft.Extensions.Logging;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Rendering;
using OpenBreed.Gui.Elements;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Builders
{
    internal class DesktopBuilder : DockPanelBuilder, IDesktopBuilder
    {
        #region Public Constructors

        public DesktopBuilder(IInteractionRenderer interactionRenderer, ILogger logger, IRenderView renderView)
        {
            InteractionRenderer = interactionRenderer;
            Logger = logger;
            RenderView = renderView;
        }

        #endregion Public Constructors

        #region Internal Properties

        internal IInteractionRenderer InteractionRenderer { get; }
        internal ILogger Logger { get; }
        internal IRenderView RenderView { get; }

        #endregion Internal Properties

        #region Internal Methods

        public new IDesktop Build()
        {
            return new Desktop(this);
        }

        #endregion Internal Methods
    }
}