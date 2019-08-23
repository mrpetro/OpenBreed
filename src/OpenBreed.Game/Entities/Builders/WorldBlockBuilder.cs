using OpenBreed.Core;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Entities.Builders;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;

namespace OpenBreed.Game.Entities.Builders
{
    public class WorldBlockBuilder : EntityBuilder
    {
        #region Internal Fields

        internal int x;
        internal int y;
        internal int atlasId;
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

        public void SetTileAtlas(string atlasAlias)
        {
            var atlas = Core.Rendering.Tiles.GetByAlias(atlasAlias);
            this.atlasId = atlas.Id;
        }

        public void SetTileId(int tileId)
        {
            this.tileId = tileId;
        }

        public override IEntity Build()
        {
            var entity = Core.Entities.Create();

            entity.Add(GridPosition.Create(x, y));
            entity.Add(Body.Create(1.0f, 1.0f));
            entity.Add(AxisAlignedBoxShape.Create(0, 0, 16, 16));
            entity.Add(Tile.Create(atlasId, tileId));

            return entity;
        }

        #endregion Public Methods
    }
}