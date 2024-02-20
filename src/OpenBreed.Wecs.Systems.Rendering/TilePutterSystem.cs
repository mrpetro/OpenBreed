using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Wecs.Systems.Rendering
{
    [RequireEntityWith(
        typeof(TilePutterComponent),
        typeof(TileGridComponent))]

    public class TilePutterSystem : UpdatableMatchingSystemBase<TilePutterSystem>
    {

        #region Public Constructors

        public TilePutterSystem()
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var tp = entity.Get<TilePutterComponent>();

            var grid = entity.Get<TileGridComponent>().Grid;

            //Update all tiles
            for (int i = 0; i < tp.Items.Count; i++)
                ModifyTile(grid, tp.Items[i]);

            tp.Items.Clear();
        }

        private void ModifyTile(ITileGrid tileGrid, TileData tileData)
        {
            tileGrid.ModifyTile(tileData.Position, tileData.AtlasId, tileData.ImageIndex);
        }

        #endregion Protected Methods
    }
}