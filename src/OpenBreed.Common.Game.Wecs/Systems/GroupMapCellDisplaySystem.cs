using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    [RequireEntityWith(
        typeof(PositionComponent),
        typeof(GroupComponent))]
    public class GroupMapCellDisplaySystem : MatchingSystemBase<GroupMapCellDisplaySystem>, IRenderableSystem
    {
        #region Private Fields

        private readonly IPrimitiveRenderer primitiveRenderer;

        private readonly IFontMan fontMan;

        private readonly IFont font;

        #endregion Private Fields

        #region Public Constructors

        public GroupMapCellDisplaySystem(
            IPrimitiveRenderer primitiveRenderer,
            IFontMan fontMan)
        {
            this.primitiveRenderer = primitiveRenderer;
            this.fontMan = fontMan;

            font = fontMan.GetOSFont("ARIAL", 8);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(OpenBreed.Wecs.Worlds.IWorldRenderContext context)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);

            for (int i = 0; i < entities.Count; i++)
                DrawEntityAabb(context.View, entities[i], context.ViewBox);

            GL.Disable(EnableCap.Blend);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Draw this wireframe to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which entity wireframe will be rendered to</param>
        private void DrawEntityAabb(Rendering.Interface.Managers.IRenderView view, IEntity entity, Box2 clipBox)
        {
            var posCmp = entity.Get<PositionComponent>();
            var groupCmp = entity.Get<GroupComponent>();

            if (groupCmp.Id == -1)
                return;

            if (posCmp.Value.X + 16 < clipBox.Min.X)
                return;

            if (posCmp.Value.X > clipBox.Max.X)
                return;

            if (posCmp.Value.Y + 16 < clipBox.Min.Y)
                return;

            if (posCmp.Value.Y > clipBox.Max.Y)
                return;

            view.PushMatrix();

            view.Translate((int)posCmp.Value.X, (int)posCmp.Value.Y, 0.0f);

            var aabb = new Box2(0, 0, 16, 16);

            primitiveRenderer.DrawRectangle(view, aabb, Color4.Yellow);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black);

            font.Draw(view, groupCmp.Id.ToString(), Color4.White, clipBox);

            GL.Disable(EnableCap.Blend);

            view.PopMatrix();
        }

        #endregion Private Methods
    }
}