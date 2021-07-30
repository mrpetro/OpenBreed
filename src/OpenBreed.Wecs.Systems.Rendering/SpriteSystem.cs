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

        private readonly List<Entity> inactive = new List<Entity>();
        private readonly List<Entity> active = new List<Entity>();
        private readonly ISpriteMan spriteMan;

        #endregion Private Fields

        #region Internal Constructors

        internal SpriteSystem(ISpriteMan spriteMan)
        {
            this.spriteMan = spriteMan;

            Require<SpriteComponent>();
            Require<PositionComponent>();

            RegisterHandler<SpriteOnCommand>(HandleSpriteOnCommand);
            RegisterHandler<SpriteOffCommand>(HandleSpriteOffCommand);
            RegisterHandler<SpriteSetCommand>(HandleSpriteSetCommand);
            RegisterHandler<SpriteSetAtlasCommand>(HandleSpriteSetAtlasCommand);
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

            for (int i = 0; i < active.Count; i++)
                RenderSprite(active[i], clipBox);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            active.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            active.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleSpriteOnCommand(SpriteOnCommand cmd)
        {
            var toActivate = inactive.FirstOrDefault(item => item.Id == cmd.EntityId);

            if (toActivate != null)
            {
                active.Add(toActivate);
                inactive.Remove(toActivate);
            }

            return true;
        }

        private bool HandleSpriteSetCommand(SpriteSetCommand cmd)
        {
            var toModify = active.FirstOrDefault(item => item.Id == cmd.EntityId);
            if (toModify == null)
                return false;

            var sprite = toModify.Get<SpriteComponent>();
            sprite.ImageId = cmd.ImageId;

            return true;
        }

        private bool HandleSpriteSetAtlasCommand(SpriteSetAtlasCommand cmd)
        {
            var toModify = active.FirstOrDefault(item => item.Id == cmd.EntityId);
            if (toModify == null)
                return false;

            var sprite = toModify.Get<SpriteComponent>();
            sprite.AtlasId = cmd.AtlasId;

            return true;
        }

        private bool HandleSpriteOffCommand(SpriteOffCommand cmd)
        {
            var toDeactivate = active.FirstOrDefault(item => item.Id == cmd.EntityId);

            if (toDeactivate != null)
            {
                inactive.Add(toDeactivate);
                active.Remove(toDeactivate);
            }

            return true;
        }

        /// <summary>
        /// Draw this sprite to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this sprite will be rendered to</param>
        private void RenderSprite(Entity entity, Box2 clipBox)
        {
            //var messaging = entity.TryGet<MessagingComponent>();

            //if (messaging.Messages.Count > 0)
            //{
            //}

            var pos = entity.Get<PositionComponent>();
            var spc = entity.Get<SpriteComponent>();

            spriteMan.Render(spc.AtlasId, spc.ImageId, pos.Value, spc.Order, clipBox);
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