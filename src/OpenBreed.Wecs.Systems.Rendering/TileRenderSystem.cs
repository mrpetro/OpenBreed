using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Rendering
{
    [RequireEntityWith(
        typeof(TileGridComponent))]
    public class TileRenderSystem : SystemBase<TileRenderSystem>, IRenderable
    {
        #region Private Fields

        private readonly HashSet<IEntity> entities = new HashSet<IEntity>();

        #endregion Private Fields

        #region Public Constructors

        public TileRenderSystem(IWorld world)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void AddEntity(IEntity entity) => entities.Add(entity);

        public override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public override void RemoveEntity(IEntity entity) => entities.Remove(entity);

        public void Render(Box2 clipBox, int depth, float dt)
        {
            foreach (var item in entities)
            {
                RenderEntity(item, clipBox, depth, dt);
            }

            //tileGrid.Render(clipBox);
        }

        public void RenderEntity(IEntity entity, Box2 clipBox, int depth, float dt)
        {
            var tgc = entity.Get<TileGridComponent>();
            tgc.Grid.Render(clipBox);
        }

        #endregion Public Methods
    }
}