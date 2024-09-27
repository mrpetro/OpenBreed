using Microsoft.Extensions.Logging;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Rendering.Interface.Factories;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Factories
{
    public class OpenTKRenderContextFactory : IRenderContextFactory
    {
        #region Private Fields

        private readonly ILogger logger;
        private readonly IEventsMan eventsMan;
        private HostCoordinateSystemConverter hostCoordinateSystemConverter;
        private IGraphicsContext graphicsContext;

        #endregion Private Fields

        #region Public Constructors

        public OpenTKRenderContextFactory(ILogger logger, IEventsMan eventsMan)
        {
            this.logger = logger;
            this.eventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public IRenderContext CreateContext()
        {
            return new OpenTKRenderContext(logger, eventsMan, graphicsContext, hostCoordinateSystemConverter);
        }

        public void SetupScope(HostCoordinateSystemConverter hostCoordinateSystemConverter, IGraphicsContext graphicsContext)
        {
            this.hostCoordinateSystemConverter = hostCoordinateSystemConverter;
            this.graphicsContext = graphicsContext;
        }

        #endregion Public Methods
    }
}