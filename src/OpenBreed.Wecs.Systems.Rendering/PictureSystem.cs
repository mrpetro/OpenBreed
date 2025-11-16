using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions.Renderers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Rendering
{
    [RequireEntityWith(
        typeof(PictureComponent),
        typeof(PositionComponent))]
    public class PictureSystem : IMatchingSystem, IRenderableSystem
    {
        #region Public Constructors

        public PictureSystem()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IWorldRenderContext context)
        {
            var entities = context.World.GetMatchingEntities(this);

            var pictureRenderer = context.View.Context.PictureRenderer;

            pictureRenderer.RenderBegin();

            try
            {
                foreach (var entity in entities)
                {
                    RenderPicture(entity, context);
                }
            }
            finally
            {
                pictureRenderer.RenderEnd();
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Draw picture to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this picture will be rendered to</param>
        private void RenderPicture(IEntity entity, IWorldRenderContext context)
        {
            var pictureRenderer = context.View.Context.PictureRenderer;

            var picComponent = entity.Get<PictureComponent>();

            var pos = entity.Get<PositionComponent>().Value;
            pos += picComponent.Origin;

            pictureRenderer.Render(context.View, new Vector3((int)pos.X, (int)pos.Y, picComponent.Order), Vector2.One, picComponent.Color, picComponent.ImageId);
        }

        #endregion Private Methods
    }
}