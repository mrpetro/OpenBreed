using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Sandbox.Wecs.Systems
{
    [RequireEntityWith(
        typeof(PositionComponent),
        typeof(UnknownCodeComponent))]
    public class UnknownMapCellDisplaySystem : MatchingSystemBase<UnknownMapCellDisplaySystem>, IRenderable
    {
        #region Private Fields

        private readonly IPrimitiveRenderer primitiveRenderer;

        private readonly IFontMan fontMan;

        private readonly IFont font;

        private List<IEntity> entities = new List<IEntity>();

        #endregion Private Fields

        #region Public Constructors

        public UnknownMapCellDisplaySystem(
            IWorld world,
            IPrimitiveRenderer primitiveRenderer,
            IFontMan fontMan)
        {
            this.primitiveRenderer = primitiveRenderer;
            this.fontMan = fontMan;

            font = fontMan.GetOSFont("ARIAL", 8);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(Box2 clipBox, int depth, float dt)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < entities.Count; i++)
                DrawEntityAabb(entities[i], clipBox);

            GL.Disable(EnableCap.Blend);
        }

        #endregion Public Methods

        #region Protected Methods

        public override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public override void AddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        public override void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Draw this wireframe to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which entity wireframe will be rendered to</param>
        private void DrawEntityAabb(IEntity entity, Box2 clipBox)
        {
            var posCmp = entity.Get<PositionComponent>();

            if (posCmp.Value.X + 16 < clipBox.Min.X)
                return;

            if (posCmp.Value.X > clipBox.Max.X)
                return;

            if (posCmp.Value.Y + 16 < clipBox.Min.Y)
                return;

            if (posCmp.Value.Y > clipBox.Max.Y)
                return;

            var unknownCodeCmp = entity.Get<UnknownCodeComponent>();

            primitiveRenderer.PushMatrix();

            primitiveRenderer.Translate((int)posCmp.Value.X, (int)posCmp.Value.Y, 0.0f);

            var aabb = new Box2(0, 0, 16, 16);

            primitiveRenderer.DrawRectangle(aabb, Color4.Yellow);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black);

            font.Draw(unknownCodeCmp.Code.ToString(), Color4.White, clipBox);
            GL.Disable(EnableCap.Blend);

            primitiveRenderer.PopMatrix();
        }

        #endregion Private Methods
    }
}