using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Components.Gui;
using OpenBreed.Wecs.Components.Physics;

using OpenBreed.Rendering.Interface;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace OpenBreed.Wecs.Systems.Gui
{
    public class PhysicsDebugDisplaySystem : SystemBase, IRenderableSystem
    {
        #region Private Fields

        private List<Entity> entities = new List<Entity>();
        private readonly IPrimitiveRenderer primitiveRenderer;

        #endregion Private Fields

        #region Public Constructors

        public PhysicsDebugDisplaySystem(IPrimitiveRenderer primitiveRenderer)
        {
            this.primitiveRenderer = primitiveRenderer;

            Require<BodyComponent>();
            Require<PositionComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            //inputsMan.MouseMove += Inputs_MouseMove;
        }

        //private void Inputs_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
        //{
        //    for (int i = 0; i < entities.Count; i++)
        //    {
        //        var icc = entities[i].Get<CursorInputComponent>();

        //        if (icc.CursorId != 0)
        //            return;

        //        var viewportSystem = renderingModule.ScreenWorld.Systems.OfType<ViewportSystem>().FirstOrDefault();

        //        var gameViewport = renderingModule.ScreenWorld.Entities.FirstOrDefault( item => object.Equals(item.Tag, "GameViewport"));

        //        if (gameViewport == null)
        //            return;

        //        var pos = entities[i].Get<PositionComponent>();


        //        var coord = viewportSystem.ClientToWorld(new OpenTK.Vector4(e.X, e.Y, 0.0f, 1.0f), gameViewport);
        //        pos.Value = new OpenTK.Vector2(coord.X, coord.Y);
        //    }
        //}

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

        public void Render(Box2 clipBox, int depth, float dt)
        {
            //GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < entities.Count; i++)
                DrawEntityAabb(entities[i], clipBox);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        /// <summary>
        /// Draw this wireframe to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which entity wireframe will be rendered to</param>
        private void DrawEntityAabb(Entity entity, Box2 clipBox)
        {
            var posCmp = entity.Get<PositionComponent>();
            var bodyCmp = entity.Get<BodyComponent>();

            if (bodyCmp.Tag != "Dynamic")
                return;

            var aabb = bodyCmp.Aabb;

            //Test viewport for clippling here
            if (aabb.Right < clipBox.Left)
                return;

            if (aabb.Left > clipBox.Right)
                return;

            if (aabb.Top < clipBox.Bottom)
                return;

            if (aabb.Bottom > clipBox.Top)
                return;


            // Draw black box
            GL.Color4(Color4.Green);
            primitiveRenderer.DrawRectangle(bodyCmp.Aabb);
        }

        #endregion Protected Methods
    }
}