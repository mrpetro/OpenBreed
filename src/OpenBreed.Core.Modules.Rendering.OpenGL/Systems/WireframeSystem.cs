using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Systems;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class WireframeSystem : WorldSystem, IWireframeSystem
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly List<IWireframe> wireframeComps = new List<IWireframe>();
        private readonly List<Position> positionComps = new List<Position>();
        private readonly List<IShapeComponent> shapeComps = new List<IShapeComponent>();

        #endregion Private Fields

        #region Public Constructors

        public WireframeSystem(WireframeSystemBuilder builder) : base(builder.core)
        {
            Require<IWireframe>();
            Require<Position>();
            Require<IShapeComponent>();
        }

        public WireframeSystem(ICore core) : base(core)
        {
            Require<IWireframe>();
            Require<Position>();
            Require<IShapeComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// This will draw all tiles to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which tiles will be drawn to</param>
        public void Render(IViewport viewport, float dt)
        {
            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            //GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < entities.Count; i++)
                DrawEntityWireframe(viewport, i);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        /// <summary>
        /// Draw this wireframe to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which entity wireframe will be rendered to</param>
        public void DrawEntityWireframe(IViewport viewport, int index)
        {
            var entity = entities[index];
            var wireframe = wireframeComps[index];
            var position = positionComps[index];
            var shape = shapeComps[index];

            GL.PushMatrix();

            GL.Translate((int)position.Value.X, (int)position.Value.Y, 0.0f);

            if (shape is AxisAlignedBoxShape)
                DrawBox((AxisAlignedBoxShape)shape, wireframe.Color);

            GL.PopMatrix();

            if (entity.DebugData != null)
            {
                var data = entity.DebugData as object[];
                if (data != null)
                {
                    if ((string)data[0] == "COLLISION_PAIR")
                    {
                        RenderTools.DrawLine((Vector2)data[1], (Vector2)data[2], Color4.Yellow);
                    }
                }
            }
        }

        ///// <summary>
        ///// Draw this sprite to given viewport
        ///// </summary>
        ///// <param name="viewport">Viewport which this sprite will be rendered to</param>
        //public void Draw(Body body, IViewport viewport)
        //{
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

        //    //if (body.Collides)
        //    //{
        //    //    RenderTools.DrawBox(body.Aabb, Color4.Red);
        //    //    RenderTools.DrawLine(body.Aabb.GetCenter(), Vector2.Add(body.Aabb.GetCenter(), body.Projection * 10), Color4.Purple);
        //    //}
        //    //else
        //    //    RenderTools.DrawBox(body.Aabb, Color4.Green);
        //}

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            entities.Add(entity);
            wireframeComps.Add(entity.Components.OfType<IWireframe>().First());
            positionComps.Add(entity.Components.OfType<Position>().First());
            shapeComps.Add(entity.Components.OfType<IShapeComponent>().First());
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            var index = entities.IndexOf(entity);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
            wireframeComps.RemoveAt(index);
            positionComps.RemoveAt(index);
            shapeComps.RemoveAt(index);
        }

        #endregion Protected Methods

        #region Private Methods

        private void DrawBox(AxisAlignedBoxShape box, Color4 color)
        {
            RenderTools.DrawBox(box.Aabb, color);
        }

        #endregion Private Methods
    }
}