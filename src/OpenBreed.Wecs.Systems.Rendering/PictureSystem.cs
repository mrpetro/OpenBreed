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
    public class PictureSystem : SystemBase<PictureSystem>, IRenderable
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly IPictureRenderer imageRenderer;

        #endregion Private Fields

        #region Internal Constructors

        internal PictureSystem(
            IWorld world,
            IPictureRenderer imageRenderer) :
            base(world)
        {
            this.imageRenderer = imageRenderer;

            world.GetModule<IRenderableBatch>().Add(this);
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Render(Box2 clipBox, int depth, float dt)
        {
            imageRenderer.RenderBegin();

            try
            {
                for (int i = 0; i < entities.Count; i++)
                    RenderPicture(entities[i], clipBox);
            }
            finally
            {
                imageRenderer.RenderEnd();
            }
        }

        #endregion Public Methods

        #region Protected Methods

        public override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public override void AddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        public override void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Draw picture to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this picture will be rendered to</param>
        private void RenderPicture(IEntity entity, Box2 clipBox)
        {
            var picComponent = entity.Get<PictureComponent>();

            var pos = entity.Get<PositionComponent>().Value;
            pos += picComponent.Origin;

            var size = new Vector2();

            imageRenderer.Render(new Vector3((int)pos.X, (int)pos.Y, picComponent.Order), size, picComponent.Color, picComponent.ImageId);
        }

        #endregion Private Methods
    }
}