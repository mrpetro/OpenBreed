using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Wecs.Systems.Rendering
{
    [RequireEntityWith(
        typeof(TilePutterComponent),
        typeof(TileGridComponent))]
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

        //TODO: Remove me
        public bool ContainsEntity(IEntity entity)
        {
            return false;

            //throw new System.NotImplementedException();
        }

        //TODO: Remove me
        public void OnAddEntity(IWorld world, IEntity entity)
        {
            //throw new System.NotImplementedException();
        }

        //TODO: Remove me
        public void OnRemoveEntity(IWorld world, IEntity entity)
        {
            //throw new System.NotImplementedException();
        }

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