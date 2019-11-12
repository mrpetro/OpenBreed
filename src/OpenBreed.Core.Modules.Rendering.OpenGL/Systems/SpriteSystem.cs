using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class SpriteSystem : WorldSystem, ISpriteSystem, IMsgListener
    {
        #region Private Fields

        private MsgHandler msgHandler;
        private readonly List<SpritePack> inactive = new List<SpritePack>();
        private readonly List<SpritePack> active = new List<SpritePack>();

        #endregion Private Fields

        #region Public Constructors

        public SpriteSystem(ICore core) : base(core)
        {
            msgHandler = new MsgHandler(this);

            Require<ISpriteComponent>();
            Require<IPosition>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.MessageBus.RegisterHandler(SpriteOnMsg.TYPE, msgHandler);
            World.MessageBus.RegisterHandler(SpriteOffMsg.TYPE, msgHandler);
            World.MessageBus.RegisterHandler(SpriteSetMsg.TYPE, msgHandler);
        }

        public override bool RecieveMsg(object sender, IMsg msg)
        {
            switch (msg.Type)
            {
                case SpriteOnMsg.TYPE:
                    return HandleSpriteOnMsg(sender, (SpriteOnMsg)msg);
                case SpriteOffMsg.TYPE:
                    return HandleSpriteOffMsg(sender, (SpriteOffMsg)msg);
                case SpriteSetMsg.TYPE:
                    return HandleSpriteSetMsg(sender, (SpriteSetMsg)msg);
                default:
                    return false;
            }
        }

        private bool HandleSpriteOnMsg(object sender, SpriteOnMsg msg)
        {
            var toActivate = inactive.FirstOrDefault(item => item.Entity == msg.Entity);

            if (toActivate != null)
            {
                active.Add(toActivate);
                inactive.Remove(toActivate);
            }

            return true;
        }

        private bool HandleSpriteSetMsg(object sender, SpriteSetMsg msg)
        {
            var toModify = active.FirstOrDefault(item => item.Entity == msg.Entity);
            if (toModify == null)
                return false;

            toModify.Sprite.ImageId = msg.ImageId;

            return true;
        }

        private bool HandleSpriteOffMsg(object sender, SpriteOffMsg msg)
        {
            var toDeactivate = active.FirstOrDefault(item => item.Entity == msg.Entity);

            if (toDeactivate != null)
            {
                inactive.Add(toDeactivate);
                active.Remove(toDeactivate);
            }

            return true;
        }


        /// <summary>
        /// This will draw all tiles to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which tiles will be drawn to</param>
        public void Render(IViewport viewport, float dt)
        {
            msgHandler.PostEnqueued();

            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            //GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < active.Count; i++)
                DrawEntitySprite(viewport, i);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        /// <summary>
        /// Draw this sprite to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this sprite will be rendered to</param>
        public void DrawEntitySprite(IViewport viewport, int index)
        {
            var pack = active[index];

            DrawDebug(pack, viewport);

            GL.PushMatrix();

            GL.Translate((int)pack.Position.Value.X, (int)pack.Position.Value.Y, pack.Sprite.Order);

            var spriteAtlas = Core.Rendering.Sprites.GetById(pack.Sprite.AtlasId);
            //GL.Translate(-spriteAtlas.SpriteWidth / 2, -spriteAtlas.SpriteHeight / 2, 0.0f);
            spriteAtlas.Draw(pack.Sprite.ImageId);

            GL.PopMatrix();
        }

        /// <summary>
        /// Draw this sprite to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this sprite will be rendered to</param>
        private void DrawDebug(SpritePack pack, IViewport viewport)
        {
            var body = pack.Entity.Components.OfType<IBody>().FirstOrDefault();

            if (body == null)
                return;

            if (body.Boxes != null)
            {
                foreach (var item in body.Boxes)
                {
                    RenderTools.DrawRectangle(item.Item1 * 16.0f,
                                              item.Item2 * 16.0f,
                                              item.Item1 * 16.0f + 16.0f,
                                              item.Item2 * 16.0f + 16.0f);
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            var pack = new SpritePack(entity,
                                      entity.Components.OfType<ISpriteComponent>().First(),
                                      entity.Components.OfType<IPosition>().First());

            active.Add(pack);
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            var pack = active.FirstOrDefault(item => item.Entity == entity);

            if (pack == null)
                throw new InvalidOperationException("Entity not found in this system.");

            active.Remove(pack);
        }

        public bool EnqueueMsg(object sender, IEntityMsg msg)
        {
            return false;
        }

        #endregion Protected Methods
    }
}