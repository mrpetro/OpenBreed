using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Systems;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class SpriteSystem : WorldSystem, ICommandExecutor, ICameraSystem
    {
        #region Private Fields

        private readonly List<IEntity> inactive = new List<IEntity>();
        private readonly List<IEntity> active = new List<IEntity>();
        private CommandHandler cmdHandler;

        #endregion Private Fields

        #region Public Constructors

        internal SpriteSystem(SpriteSystemBuilder builder) : base(builder.core)
        {
            cmdHandler = new CommandHandler(this);

            Require<SpriteComponent>();
            Require<Position>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.RegisterHandler(SpriteOnCommand.TYPE, cmdHandler);
            World.RegisterHandler(SpriteOffCommand.TYPE, cmdHandler);
            World.RegisterHandler(SpriteSetCommand.TYPE, cmdHandler);
        }

        public override bool ExecuteCommand(object sender, ICommand cmd)
        {
            switch (cmd.Type)
            {
                case SpriteOnCommand.TYPE:
                    return HandleSpriteOnCommand(sender, (SpriteOnCommand)cmd);

                case SpriteOffCommand.TYPE:
                    return HandleSpriteOffCommand(sender, (SpriteOffCommand)cmd);

                case SpriteSetCommand.TYPE:
                    return HandleSpriteSetCommand(sender, (SpriteSetCommand)cmd);

                default:
                    return false;
            }
        }


        /// <summary>
        /// Render this system using given viewport component and time step
        /// </summary>
        /// <param name="viewport">Rendered viewport</param>
        /// <param name="dt">Time step</param>
        public void Render(IEntity viewport, float dt)
        {

        }

        public void Render(float left, float bottom, float right, float top, float dt)
        {
            cmdHandler.ExecuteEnqueued();

            //GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < active.Count; i++)
                DrawSprite(active[i]);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        /// <summary>
        /// This will draw all tiles to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which tiles will be drawn to</param>
        public void Render(IViewport viewport, float dt)
        {
            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            Render(left, bottom, right, top, dt);
        }

        /// <summary>
        /// Draw this sprite to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this sprite will be rendered to</param>
        public void DrawSprite(IEntity entity)
        {
            var pos = entity.GetComponent<Position>();
            var sprite = entity.GetComponent<SpriteComponent>();

            GL.PushMatrix();

            GL.Translate((int)pos.Value.X, (int)pos.Value.Y, sprite.Order);

            var spriteAtlas = Core.Rendering.Sprites.GetById(sprite.AtlasId);
            //GL.Translate(-spriteAtlas.SpriteWidth / 2, -spriteAtlas.SpriteHeight / 2, 0.0f);
            spriteAtlas.Draw(sprite.ImageId);

            GL.PopMatrix();
        }

        public bool EnqueueMsg(object sender, IEntityCommand msg)
        {
            return false;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            active.Add(entity);
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            active.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleSpriteOnCommand(object sender, SpriteOnCommand cmd)
        {
            var toActivate = inactive.FirstOrDefault(item => item.Id == cmd.EntityId);

            if (toActivate != null)
            {
                active.Add(toActivate);
                inactive.Remove(toActivate);
            }

            return true;
        }

        private bool HandleSpriteSetCommand(object sender, SpriteSetCommand cmd)
        {
            var toModify = active.FirstOrDefault(item => item.Id == cmd.EntityId);
            if (toModify == null)
                return false;

            var sprite = toModify.GetComponent<SpriteComponent>();
            sprite.ImageId = cmd.ImageId;

            return true;
        }

        private bool HandleSpriteOffCommand(object sender, SpriteOffCommand cmd)
        {
            var toDeactivate = active.FirstOrDefault(item => item.Id == cmd.EntityId);

            if (toDeactivate != null)
            {
                inactive.Add(toDeactivate);
                active.Remove(toDeactivate);
            }

            return true;
        }

        ///// <summary>
        ///// Draw this sprite to given viewport
        ///// </summary>
        ///// <param name="viewport">Viewport which this sprite will be rendered to</param>
        //private void DrawDebug(SpritePack pack, IViewport viewport)
        //{
        //    var entity = Core.Entities.GetById(pack.EntityId);

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

        #endregion Private Methods
    }
}