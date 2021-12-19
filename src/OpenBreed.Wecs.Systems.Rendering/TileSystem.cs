﻿using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class TileSystem : SystemBase, IUpdatableSystem, IRenderable
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
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

        public void Update(float dt)
        {
            foreach (var entity in entities)
            {
                var tilePutterCmp = entity.TryGet<TilePutterComponent>();

                if (tilePutterCmp is null)
                    continue;

                try
                {
                    tileGrid.ModifyTile(tilePutterCmp.Position, tilePutterCmp.AtlasId, tilePutterCmp.ImageIndex);
                }
                finally
                {
                    entity.Remove<TilePutterComponent>();
                }
            }
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            //foreach (var item in Entities)
            //{
            //}
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods
    }
}