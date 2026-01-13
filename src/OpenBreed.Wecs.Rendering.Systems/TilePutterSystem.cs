using OpenBreed.Rendering.Abstractions;
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Wecs.Core.Systems.Categories;
using System;

namespace OpenBreed.Wecs.Rendering.Systems
{
    [RequireEntityWith(
        typeof(TilePutterComponent),
        typeof(TileGridComponent))]
    [SystemCategory(CommonCategories.Rendering)]
    public class TilePutterSystem : IMatchingSystem, IUpdatableSystem
    {
        #region Private Fields

        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public TilePutterSystem(IWorldMan worldMan)
        {
            this.worldMan = worldMan ?? throw new ArgumentNullException(nameof(worldMan));
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(IUpdateContext context)
        {
            var world = worldMan.GetById(context.WorldId);

            var entities = world.GetMatchingEntities(this);

            foreach (var entity in entities)
            {
                UpdateEntity(entity, context);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void UpdateEntity(IEntity entity, IUpdateContext context)
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

        #endregion Private Methods
    }
}