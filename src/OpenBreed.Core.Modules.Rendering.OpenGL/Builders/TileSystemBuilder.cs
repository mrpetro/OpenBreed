using OpenBreed.Core.Builders;
using OpenBreed.Core.Modules.Rendering.Systems;

namespace OpenBreed.Core.Modules.Physics.Builders
{
    public class TileSystemBuilder : IWorldSystemBuilder<TileSystem>
    {
        #region Internal Fields

        internal ICore core;
        internal int gridWidth;
        internal int gridHeight;
        internal int layersNo;
        internal float tileSize;
        internal bool gridVisible;

        #endregion Internal Fields

        #region Public Constructors

        public TileSystemBuilder(ICore core)
        {
            this.core = core;
        }

        #endregion Public Constructors

        #region Public Methods

        public TileSystemBuilder SetGridSize(int width, int height)
        {
            gridWidth = width;
            gridHeight = height;
            return this;
        }

        public TileSystemBuilder SetLayersNo(int layersNo)
        {
            this.layersNo = layersNo;
            return this;
        }

        public TileSystemBuilder SetTileSize(float tileSize)
        {
            this.tileSize = tileSize;
            return this;
        }

        public TileSystemBuilder SetGridVisible(bool gridVisible)
        {
            this.gridVisible = gridVisible;
            return this;
        }

        public TileSystem Build()
        {
            return new TileSystem(this);
        }

        #endregion Public Methods
    }
}