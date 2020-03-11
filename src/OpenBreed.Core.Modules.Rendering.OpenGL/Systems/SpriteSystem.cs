using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Systems;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class SpriteSystem : WorldSystem, ICommandExecutor, IRenderableSystem
    {
        #region Private Fields

        private readonly List<IEntity> inactive = new List<IEntity>();
        private readonly List<IEntity> active = new List<IEntity>();
        private CommandHandler cmdHandler;

        #endregion Private Fields

        #region Internal Constructors

        internal SpriteSystem(SpriteSystemBuilder builder) : base(builder.core)
        {
            cmdHandler = new CommandHandler(this);

            Require<SpriteComponent>();
            Require<PositionComponent>();
        }

        #endregion Internal Constructors

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

        public void Render(Box2 clipBox, int depth, float dt)
        {
            cmdHandler.ExecuteEnqueued();

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

        /// <summary>
        /// Draw this sprite to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this sprite will be rendered to</param>
        private void RenderSprite(IEntity entity, Box2 clipBox)
        {
            var pos = entity.GetComponent<PositionComponent>();
            var spc = entity.GetComponent<SpriteComponent>();
            var atlas = Core.Rendering.Sprites.GetById(spc.AtlasId);

            //Test viewport for clippling here
            if (pos.Value.X + atlas.SpriteWidth < clipBox.Left)
                return;

            if (pos.Value.X > clipBox.Right)
                return;

            if (pos.Value.Y + atlas.SpriteHeight < clipBox.Bottom)
                return;

            if (pos.Value.Y > clipBox.Top)
                return;


            GL.PushMatrix();

            GL.Translate((int)pos.Value.X, (int)pos.Value.Y, spc.Order);

            //GL.Translate(-spriteAtlas.SpriteWidth / 2, -spriteAtlas.SpriteHeight / 2, 0.0f);
            atlas.Draw(spc.ImageId);

            GL.PopMatrix();
        }

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

        #endregion Private Methods

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
    }
}