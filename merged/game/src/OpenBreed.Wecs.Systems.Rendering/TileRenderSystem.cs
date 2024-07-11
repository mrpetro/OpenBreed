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
    public class TileRenderSystem : MatchingSystemBase<TileRenderSystem>, IRenderableSystem
    {
        #region Public Constructors

        public TileRenderSystem()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IRenderContext context)
        {
            foreach (var item in entities)
            {
                RenderEntity(item, context);
            }

            //tileGrid.Render(clipBox);
        }

        public void RenderEntity(IEntity entity, IRenderContext context)
        {
            var tgc = entity.Get<TileGridComponent>();
            tgc.Grid.Render(context.ViewBox);
        }

        #endregion Public Methods
    }
}