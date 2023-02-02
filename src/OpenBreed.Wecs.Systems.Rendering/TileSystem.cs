using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Wecs.Systems.Rendering
{
    [RequireEntityWith(typeof(TilePutterComponent))]
    public class TileSystem : UpdatableSystemBase<TileSystem>, IRenderable
    {
        #region Private Fields

        private ITileGrid tileGrid;

        #endregion Private Fields

        #region Public Constructors

        public TileSystem(IWorld world)
        {
            tileGrid = world.GetModule<ITileGrid>();
            world.GetModule<IRenderableBatch>().Add(this);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(Box2 clipBox, int depth, float dt)
        {
            tileGrid.Render(clipBox);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var tp = entity.Get<TilePutterComponent>();

            //Update all tiles
            for (int i = 0; i < tp.Items.Count; i++)
                ModifyTile(tp.Items[i]);

            tp.Items.Clear();
        }

        private void ModifyTile(TileData tileData)
        {
            tileGrid.ModifyTile(tileData.Position, tileData.AtlasId, tileData.ImageIndex);
        }

        #endregion Protected Methods
    }
}