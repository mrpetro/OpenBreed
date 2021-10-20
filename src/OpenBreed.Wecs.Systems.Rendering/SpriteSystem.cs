using OpenBreed.Core.Commands;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Managers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class SpriteSystem : SystemBase, IRenderableSystem
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

        public void Render(Box2 clipBox, int depth, float dt)
        {
            ExecuteCommands();

            //GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < entities.Count; i++)
                RenderSprite(entities[i], clipBox);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        #endregion Public Methods

        #region Protected Methods

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