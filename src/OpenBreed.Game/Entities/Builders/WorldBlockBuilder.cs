using OpenBreed.Core;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;

namespace OpenBreed.Game.Entities.Builders
{
    public class WorldBlockBuilder : WorldEntityBuilder
    {
        #region Internal Fields

        internal int x;
        internal int y;
        internal TileAtlas tileAtlas;
        internal int tileId;

        #endregion Internal Fields

        #region Public Constructors

        public WorldBlockBuilder(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void SetIndices(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void SetTileAtlas(TileAtlas tileAtlas)
        {
            this.tileAtlas = tileAtlas;
        }

        public void SetTileId(int tileId)
        {
            this.tileId = tileId;
        }

        public override IEntity Build()
        {
            return new WorldBlock(this);
        }

        #endregion Public Methods
    }
}