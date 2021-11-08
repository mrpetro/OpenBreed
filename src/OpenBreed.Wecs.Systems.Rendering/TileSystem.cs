using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System;
using System.Collections;
using System.Diagnostics;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class TileSystem : SystemBase, IRenderableSystem
    {
        #region Public Fields

        public const int TILE_SIZE = 16;
        public int MAX_TILES_COUNT = 1024 * 1024;

        #endregion Public Fields

        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly ITileMan tileMan;
        private readonly ITileGridFactory tileGridMan;
        private readonly IStampMan stampMan;
        private ITileGrid tileGrid;

        private Hashtable entities = new Hashtable();

        #endregion Private Fields

        #region Internal Constructors

        internal TileSystem(IEntityMan entityMan, ITileMan tileMan, ITileGridFactory tileGridMan, IStampMan stampMan)
        {
            this.entityMan = entityMan;
            this.tileMan = tileMan;
            this.tileGridMan = tileGridMan;
            this.stampMan = stampMan;
            RequireEntityWith<TileComponent>();
            RequireEntityWith<PositionComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            tileGrid = world.GetModule<ITileGrid>();
        }

        public void Render(Box2 clipBox, int depth, float dt)
        {
            tileGrid.Render(clipBox);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        protected override void OnAddEntity(Entity entity)
        {
            Debug.Assert(!entities.Contains(entity), "Entity already added!");

            var pos = entity.Get<PositionComponent>();

            var tile = entity.Get<TileComponent>();

            tileGrid.ModifyTile(pos.Value, tile.AtlasId, tile.ImageId);

            entities[entity] = tile;
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods
    }
}