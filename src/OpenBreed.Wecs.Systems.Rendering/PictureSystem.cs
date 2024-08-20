using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
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
    public class PictureSystem : MatchingSystemBase<PictureSystem>, IRenderableSystem
    {
        #region Private Fields

        private readonly IPictureRenderer imageRenderer;

        #endregion Private Fields

        #region Internal Constructors

        internal PictureSystem(
            IPictureRenderer imageRenderer)
        {
            this.imageRenderer = imageRenderer;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Render(IWorldRenderContext context)
        {
            imageRenderer.RenderBegin();

            try
            {
                for (int i = 0; i < entities.Count; i++)
                    RenderPicture(entities[i], context);
            }
            finally
            {
                imageRenderer.RenderEnd();
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
            var picComponent = entity.Get<PictureComponent>();

            var pos = entity.Get<PositionComponent>().Value;
            pos += picComponent.Origin;

            imageRenderer.Render(context.View, new Vector3((int)pos.X, (int)pos.Y, picComponent.Order), Vector2.One, picComponent.Color, picComponent.ImageId);
        }

        #endregion Private Methods
    }
}