using Microsoft.Extensions.Logging;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Factories;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Factories
{
    public class OpenTKRenderContextProvider : IRenderContextProvider
    {
        #region Private Fields

        private readonly ILogger logger;
        private readonly IEventsMan eventsMan;
        private readonly IPaletteMan paletteMan;
        private readonly IStampMan stampMan;
        private readonly Dictionary<IGraphicsContext, IRenderContext> contextLookup = new Dictionary<IGraphicsContext, IRenderContext>();
        private HostCoordinateSystemConverter hostCoordinateSystemConverter;
        private IGraphicsContext graphicsContext;

        #endregion Private Fields

        #region Public Constructors

        public OpenTKRenderContextProvider(ILogger logger, IEventsMan eventsMan, IPaletteMan paletteMan, IStampMan stampMan)
        {
            this.logger = logger;
            this.eventsMan = eventsMan;
            this.paletteMan = paletteMan;
            this.stampMan = stampMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public IRenderContext GetContext()
        {
            if (!contextLookup.TryGetValue(graphicsContext, out IRenderContext renderContext))
            {
                renderContext = new OpenTKRenderContext(
                    logger,
                    eventsMan,
                    paletteMan,
                    stampMan,
                    graphicsContext,
                    DeinitializeContext,
                    hostCoordinateSystemConverter);

                contextLookup.Add(graphicsContext, renderContext);
            }

            return renderContext;
        }

        private void DeinitializeContext(IGraphicsContext context)
        {
            contextLookup.Remove(context);
        }

        public void SetupScope(HostCoordinateSystemConverter hostCoordinateSystemConverter, IGraphicsContext graphicsContext)
        {
            this.hostCoordinateSystemConverter = hostCoordinateSystemConverter;
            this.graphicsContext = graphicsContext;
        }

        #endregion Public Methods
    }
}