using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;

namespace OpenBreed.Wecs.Systems.Rendering
{
    [RequireEntityWith(
        typeof(StampPutterComponent),
        typeof(TileGridComponent))]
    public class StampPutterSystem : UpdatableMatchingSystemBase<StampPutterSystem>
    {
        #region Public Constructors

        public StampPutterSystem()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var items = entity.Get<StampPutterComponent>().Items;
            var grid = entity.Get<TileGridComponent>().Grid;

            //Update all tiles
            for (int i = 0; i < items.Count; i++)
                ModifyTiles(grid, items[i]);

            items.Clear();
        }

        private void ModifyTiles(ITileGrid tileGrid, StampData data)
        {
            tileGrid.ModifyTiles(data.Position, data.StampId);
        }

        #endregion Protected Methods
    }
}