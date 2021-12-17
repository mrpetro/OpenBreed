using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace OpenBreed.Sandbox.Worlds.Wecs.Systems
{
    public class GroupMapCellDisplaySystem : SystemBase, IRenderableSystem
    {
        #region Private Fields

        private readonly IPrimitiveRenderer primitiveRenderer;

        private readonly IFontMan fontMan;

        private readonly IFont font;

        private List<Entity> entities = new List<Entity>();

        #endregion Private Fields

        #region Public Constructors

        public GroupMapCellDisplaySystem(IPrimitiveRenderer primitiveRenderer, IFontMan fontMan)
        {
            this.primitiveRenderer = primitiveRenderer;
            this.fontMan = fontMan;

            RequireEntityWith<PositionComponent>();
            RequireEntityWith<GroupComponentEx>();

            font = fontMan.GetOSFont("ARIAL", 8);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);
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
        /// Draw this wireframe to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which entity wireframe will be rendered to</param>
        private void DrawEntityAabb(Entity entity, Box2 clipBox)
        {
            var posCmp = entity.Get<PositionComponent>();
            var groupCmp = entity.Get<GroupComponentEx>();

            if (groupCmp.Id == -1)
                return;

            if (posCmp.Value.X + 16 < clipBox.Left)
                return;

            if (posCmp.Value.X > clipBox.Right)
                return;

            if (posCmp.Value.Y + 16 < clipBox.Bottom)
                return;

            if (posCmp.Value.Y > clipBox.Top)
                return;

            GL.PushMatrix();

            GL.Translate((int)posCmp.Value.X, (int)posCmp.Value.Y, 0.0f);

            var aabb = Box2.FromTLRB(0, 0, 16, 16);

            // Draw black box
            GL.Color4(Color4.Yellow);
            primitiveRenderer.DrawRectangle(aabb);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black);

            font.Draw(groupCmp.Id.ToString(), clipBox);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);

            GL.PopMatrix();
        }

        #endregion Private Methods
    }
}