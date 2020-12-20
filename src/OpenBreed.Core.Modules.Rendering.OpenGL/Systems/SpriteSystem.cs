using OpenBreed.Core.Commands;
using OpenBreed.Core;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Managers;
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
    public class SpriteSystem : WorldSystem, IRenderableSystem
    {
        #region Private Fields

        private readonly List<Entity> inactive = new List<Entity>();
        private readonly List<Entity> active = new List<Entity>();

        #endregion Private Fields

        #region Internal Constructors

        internal SpriteSystem(SpriteSystemBuilder builder) : base(builder.core)
        {
            Require<SpriteComponent>();
            Require<PositionComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public static void RegisterHandlers(ICommandsMan commands)
        {
            commands.Register<SpriteOnCommand>(HandleSpriteOnCommand);
            commands.Register<SpriteOffCommand>(HandleSpriteOffCommand);
            commands.Register<SpriteSetCommand>(HandleSpriteSetCommand);
        }

        public void Render(Box2 clipBox, int depth, float dt)
        {
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

        private static bool HandleSpriteOnCommand(ICore core, SpriteOnCommand cmd)
        {
            var system = core.GetSystemByEntityId<SpriteSystem>(cmd.EntityId);

            var toActivate = system.inactive.FirstOrDefault(item => item.Id == cmd.EntityId);

            if (toActivate != null)
            {
                system.active.Add(toActivate);
                system.inactive.Remove(toActivate);
            }

            return true;
        }

        private static bool HandleSpriteSetCommand(ICore core, SpriteSetCommand cmd)
        {
            var system = core.GetSystemByEntityId<SpriteSystem>(cmd.EntityId);

            var toModify = system.active.FirstOrDefault(item => item.Id == cmd.EntityId);
            if (toModify == null)
                return false;

            var sprite = toModify.Get<SpriteComponent>();
            sprite.ImageId = cmd.ImageId;

            return true;
        }

        private static bool HandleSpriteOffCommand(ICore core, SpriteOffCommand cmd)
        {
            var system = core.GetSystemByEntityId<SpriteSystem>(cmd.EntityId);

            var toDeactivate = system.active.FirstOrDefault(item => item.Id == cmd.EntityId);

            if (toDeactivate != null)
            {
                system.inactive.Add(toDeactivate);
                system.active.Remove(toDeactivate);
            }

            return true;
        }

        /// <summary>
        /// Draw this sprite to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this sprite will be rendered to</param>
        private void RenderSprite(Entity entity, Box2 clipBox)
        {
            var pos = entity.Get<PositionComponent>();
            var spc = entity.Get<SpriteComponent>();
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

            atlas.Draw(spc.ImageId);

            GL.PopMatrix();
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