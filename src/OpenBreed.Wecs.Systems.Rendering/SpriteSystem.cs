using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Systems.Rendering
{
    [RequireEntityWith(
        typeof(SpriteComponent),
        typeof(PositionComponent))]
    public class SpriteSystem : IMatchingSystem, IRenderableSystem
    {
        #region Private Fields

        private readonly ISpriteMan spriteMan;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSystem(ISpriteMan spriteMan)
        {
            this.spriteMan = spriteMan;
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

            var spriteRenderer = context.View.Context.SpriteRenderer;

            spriteRenderer.RenderBegin();

            try
            {
                foreach (var entity in entities)
                {
                    RenderSprite(entity, context);
                }
            }
            finally
            {
                spriteRenderer.RenderEnd();
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Draw this sprite to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this sprite will be rendered to</param>
        private void RenderSprite(IEntity entity, IWorldRenderContext context)
        {
            var clipBox = context.ViewBox;

            var spc = entity.Get<SpriteComponent>();

            if (spc.Hidden)
                return;

            if (spc.AtlasId == -1)
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

            context.View.Context.SpriteRenderer.Render(context.View, new Vector3((int)pos.X, (int)pos.Y, spc.Order), spc.Scale, Color4.White, spc.AtlasId, spc.ImageId);
        }

        #endregion Private Methods
    }
}