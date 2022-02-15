using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class TileSystem : UpdatableSystemBase, IRenderable
    {
        #region Private Fields

        private ITileGrid tileGrid;

        #endregion Private Fields

        #region Public Constructors

        public TileSystem()
        {
            RequireEntityWith<TilePutterComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            tileGrid = world.GetModule<ITileGrid>();

            world.GetModule<IRenderableBatch>().Add(this);
        }

        public void Render(Box2 clipBox, int depth, float dt)
        {
            tileGrid.Render(clipBox);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntity(Entity entity, IWorldContext context)
        {
            var tilePutterCmp = entity.TryGet<TilePutterComponent>();

            if (tilePutterCmp is null)
                return;

            try
            {
                tileGrid.ModifyTile(tilePutterCmp.Position, tilePutterCmp.AtlasId, tilePutterCmp.ImageIndex);
            }
            finally
            {
                entity.Remove<TilePutterComponent>();
            }
        }

        #endregion Protected Methods
    }
}