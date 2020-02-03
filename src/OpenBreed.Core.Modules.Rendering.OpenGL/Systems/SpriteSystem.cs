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
    public class SpriteSystem : WorldSystem, ISpriteSystem, ICommandExecutor
    {
        #region Private Fields

        private readonly List<SpritePack> inactive = new List<SpritePack>();
        private readonly List<SpritePack> active = new List<SpritePack>();
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
        /// This will draw all tiles to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which tiles will be drawn to</param>
        public void Render(IViewport viewport, float dt)
        {
            cmdHandler.ExecuteEnqueued();

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

            //DrawDebug(pack, viewport);

            GL.PushMatrix();

            GL.Translate((int)pack.Position.Value.X, (int)pack.Position.Value.Y, pack.Sprite.Order);

            var spriteAtlas = Core.Rendering.Sprites.GetById(pack.Sprite.AtlasId);
            //GL.Translate(-spriteAtlas.SpriteWidth / 2, -spriteAtlas.SpriteHeight / 2, 0.0f);
            spriteAtlas.Draw(pack.Sprite.ImageId);

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
            var pack = new SpritePack(entity.Id,
                                      entity.Components.OfType<SpriteComponent>().First(),
                                      entity.Components.OfType<Position>().First());

            active.Add(pack);
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            var pack = active.FirstOrDefault(item => item.EntityId == entity.Id);

            if (pack == null)
                throw new InvalidOperationException("Entity not found in this system.");

            active.Remove(pack);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleSpriteOnCommand(object sender, SpriteOnCommand cmd)
        {
            var toActivate = inactive.FirstOrDefault(item => item.EntityId == cmd.EntityId);

            if (toActivate != null)
            {
                active.Add(toActivate);
                inactive.Remove(toActivate);
            }

            return true;
        }

        private bool HandleSpriteSetCommand(object sender, SpriteSetCommand cmd)
        {
            var toModify = active.FirstOrDefault(item => item.EntityId == cmd.EntityId);
            if (toModify == null)
                return false;

            toModify.Sprite.ImageId = cmd.ImageId;

            return true;
        }

        private bool HandleSpriteOffCommand(object sender, SpriteOffCommand cmd)
        {
            var toDeactivate = active.FirstOrDefault(item => item.EntityId == cmd.EntityId);

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

        //    var body = entity.Components.OfType<Body>().FirstOrDefault();

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