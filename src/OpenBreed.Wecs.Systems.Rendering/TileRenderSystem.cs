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
    public class TileRenderSystem : MatchingSystemBase, IRenderableSystem
    {
        #region Public Constructors

        public TileRenderSystem()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IWorldRenderContext context)
        {
            foreach (var item in entities)
            {
                RenderEntity(item, context);
            }

            //tileGrid.Render(clipBox);
        }

        public void RenderEntity(IEntity entity, IWorldRenderContext context)
        {
            var tgc = entity.Get<TileGridComponent>();
            tgc.Grid.Render(context.View, context.ViewBox);
        }

        #endregion Public Methods
    }
}