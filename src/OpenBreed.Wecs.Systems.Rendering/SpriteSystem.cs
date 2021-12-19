using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class SpriteSystem : SystemBase, IRenderable
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly ISpriteMan spriteMan;

        #endregion Private Fields

        #region Internal Constructors

        internal SpriteSystem(ISpriteMan spriteMan)
        {
            this.spriteMan = spriteMan;

            RequireEntityWith<SpriteComponent>();
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
            spriteMan.Render(clipBox, dt, RenderSprites);
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
        /// Render all sprites in this system
        /// </summary>
        /// <param name="clipBox">clipBox</param>
        /// <param name="dt">Delta time</param>
        private void RenderSprites(Box2 clipBox, float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                RenderSprite(entities[i], clipBox);
        }

        /// <summary>
        /// Draw this sprite to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this sprite will be rendered to</param>
        private void RenderSprite(Entity entity, Box2 clipBox)
        {
            var spc = entity.Get<SpriteComponent>();

            if (spc.Hidden)
                return;

            var pos = entity.Get<PositionComponent>();

            spriteMan.Render(spc.AtlasId, spc.ImageId, spc.Origin, pos.Value, spc.Order, clipBox);
        }

        #endregion Private Methods

        ///// <summary>
        ///// Draw this sprite to given viewport
        ///// </summary>
        ///// <param name="viewport">Viewport which this sprite will be rendered to</param>
        //private void DrawDebug(SpritePack pack, IViewport viewport)
        //{
        //    var entity = Core.GetManager<IEntityMan>().GetById(pack.EntityId);

        //    var body = entity.GetComponent<Body>().FirstOrDefault();

        //    if (body == null)
        //        return;

        //    if (body.Boxes != null)
        //    {
        //        foreach (var item in body.Boxes)
        //        {
        //            RenderTools.DrawRectangle(item.Item1 * 16.0f,
        //                                      item.Item2 * 16.0f,
        //                                      item.Item1 * 16.0f + 16.0f,
        //                                      item.Item2 * 16.0f + 16.0f);
        //        }
        //    }
        //}
    }
}