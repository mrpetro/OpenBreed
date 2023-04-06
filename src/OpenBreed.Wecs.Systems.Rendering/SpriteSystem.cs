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
        typeof(SpriteComponent),
        typeof(PositionComponent))]
    public class SpriteSystem : SystemBase<SpriteSystem>, IRenderable
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly ISpriteMan spriteMan;
        private readonly ISpriteRenderer spriteRenderer;

        #endregion Private Fields

        #region Internal Constructors

        internal SpriteSystem(
            IWorld world, 
            ISpriteMan spriteMan,
            ISpriteRenderer spriteRenderer)
        {
            this.spriteMan = spriteMan;
            this.spriteRenderer = spriteRenderer;

            world.GetModule<IRenderableBatch>().Add(this);
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Render(Box2 clipBox, int depth, float dt)
        {
            spriteRenderer.RenderBegin();

            try
            {
                for (int i = 0; i < entities.Count; i++)
                    RenderSprite(entities[i], clipBox);
            }
            finally
            {
                spriteRenderer.RenderEnd();
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
        /// Draw this sprite to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this sprite will be rendered to</param>
        private void RenderSprite(IEntity entity, Box2 clipBox)
        {
            var spc = entity.Get<SpriteComponent>();

            if (spc.Hidden)
                return;

            if(spc.AtlasId == -1)
                return;

            var atlas = spriteMan.GetById(spc.AtlasId);

            if (!atlas.IsValid(spc.ImageId))
                return;

            var pos = entity.Get<PositionComponent>().Value;
            pos -= spc.Origin;
            var size = atlas.GetSpriteSize(spc.ImageId);
            size *= spc.Scale;

            //Test viewport for clippling here
            if (pos.X + size.X < clipBox.Min.X)
                return;

            if (pos.X > clipBox.Max.X)
                return;

            if (pos.Y + size.Y < clipBox.Min.Y)
                return;

            if (pos.Y > clipBox.Max.Y)
                return;

            spriteRenderer.Render(new Vector3((int)pos.X, (int)pos.Y, spc.Order), spc.Scale, Color4.White, spc.AtlasId, spc.ImageId);
        }

        #endregion Private Methods
    }
}