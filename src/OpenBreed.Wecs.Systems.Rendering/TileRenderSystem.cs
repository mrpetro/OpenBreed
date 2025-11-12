using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
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
    public class TileRenderSystem : IMatchingSystem, IRenderableSystem
    {
        #region Public Constructors

        public TileRenderSystem()
        {
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

        public void Render(IWorldRenderContext context)
        {
            var entities = context.World.GetMatchingEntities(this);

            foreach (var item in entities)
            {
                RenderEntity(item, context);
            }
        }

        public void RenderEntity(IEntity entity, IWorldRenderContext context)
        {
            var tgc = entity.Get<TileGridComponent>();
            tgc.Grid.Render(context.View, context.ViewBox);
        }

        #endregion Public Methods
    }
}