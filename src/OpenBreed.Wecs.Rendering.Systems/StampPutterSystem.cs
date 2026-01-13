using OpenBreed.Rendering.Abstractions;
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Wecs.Core.Systems.Categories;

namespace OpenBreed.Wecs.Rendering.Systems
{
    [RequireEntityWith(
        typeof(StampPutterComponent),
        typeof(TileGridComponent))]
    [SystemCategory(CommonCategories.Rendering)]
    public class StampPutterSystem : IUpdatableSystem, IMatchingSystem
    {
        #region Private Fields

        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public StampPutterSystem(IWorldMan worldMan)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
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

        #endregion Private Methods
    }
}