using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions.Renderers;
using OpenBreed.Rendering.OpenGL.Helpers;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class TileGridFactory : ITileGridFactory
    {
        #region Private Fields

        private readonly ITileMan tileMan;
        private readonly IStampMan stampMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        public TileGridFactory(ITileMan tileMan, IStampMan stampMan, ILogger logger)
        {
            this.tileMan = tileMan;
            this.stampMan = stampMan;
            this.logger = logger;
        }

        #endregion Internal Constructors

        #region Public Methods

        public ITileGrid CreateGrid(int width, int height, int layersNo, int cellSize)
        {
            return new TileGrid(stampMan, logger, width, height, layersNo, cellSize);
        }

        #endregion Public Methods
    }
}