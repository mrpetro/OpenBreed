using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenBreed.Core.Abstractions.Managers;
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
        private readonly IServiceScopeFactory serviceScopeFactory;

        #endregion Private Fields

        #region Public Constructors

        public OpenTKRenderContextProvider(ILogger logger, IEventsMan eventsMan, IPaletteMan paletteMan, IStampMan stampMan, IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.eventsMan = eventsMan;
            this.paletteMan = paletteMan;
            this.stampMan = stampMan;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public IRenderContext GetContext(IGraphicsContext graphicsContext, HostCoordinateSystemConverter hostCoordinateSystemConverter)
        {
            var renderContext = new OpenTKRenderContext(
                logger,
                eventsMan,
                paletteMan,
                stampMan,
                serviceScopeFactory,
                graphicsContext,
                (c) => { },
                hostCoordinateSystemConverter);

            return renderContext;
        }

        #endregion Public Methods
    }
}