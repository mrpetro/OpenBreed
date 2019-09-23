using OpenBreed.Core;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Entities.Builders;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenTK;
using System.Linq;

namespace OpenBreed.Game.Entities.Builders
{
    public class WorldBlockBuilder : EntityBuilder
    {
        #region Internal Fields

        internal Vector2 pos;
        internal int atlasId;
        internal int tileId;

        #endregion Internal Fields

        #region Public Constructors

        public WorldBlockBuilder(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void SetPosition(Vector2 pos )
        {
            this.pos = pos;
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

        private static void OnCollision(IEntity thisEntity, IEntity otherEntity, Vector2 projection)
        {
            thisEntity.RaiseEvent(new CollisionEvent(otherEntity));
        }


        public override IEntity Build()
        {
            var entity = Core.Entities.Create();

            entity.Add(Position.Create(pos));
            entity.Add(Body.Create(1.0f, 1.0f, "Static", (e, c) => OnCollision(entity, e, c)));
            entity.Add(AxisAlignedBoxShape.Create(0, 0, 16, 16));
            entity.Add(Tile.Create(atlasId, tileId));

            return entity;
        }

        #endregion Public Methods
    }
}