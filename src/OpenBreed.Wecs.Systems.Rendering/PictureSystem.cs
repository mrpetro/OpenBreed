using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class PictureSystem : SystemBase, IRenderable
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IPictureRenderer imageRenderer;

        #endregion Private Fields

        #region Internal Constructors

        internal PictureSystem(IPictureRenderer imageRenderer)
        {
            this.imageRenderer = imageRenderer;
            RequireEntityWith<PictureComponent>();
            RequireEntityWith<PositionComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            world.GetModule<IRenderableBatch>().Add(this);
        }

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

        #region Private Methods

        /// <summary>
        /// Draw picture to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this picture will be rendered to</param>
        private void RenderPicture(Entity entity, Box2 clipBox)
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