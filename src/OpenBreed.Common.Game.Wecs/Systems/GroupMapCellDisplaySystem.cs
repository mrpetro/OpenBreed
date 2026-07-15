using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Wecs.Core.Components;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    [RequireEntityWith(
        typeof(PositionComponent),
        typeof(GroupComponent))]
    public class GroupMapCellDisplaySystem : IRenderableSystem
    {
        #region Private Fields

        private readonly IFontMan fontMan;

        private readonly IFontAtlas font;

        #endregion Private Fields

        #region Public Constructors

        public GroupMapCellDisplaySystem(
            IFontMan fontMan)
        {
            this.fontMan = fontMan;

            font = fontMan.GetOSFont("ARIAL", 8);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IEnumerable<IEntity> entities, IWorldRenderContext context)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);

            foreach (var entity in entities)
            {
                DrawEntityAabb(context.View, entity, context.ViewBox);
            }

            GL.Disable(EnableCap.Blend);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Draw this wireframe to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which entity wireframe will be rendered to</param>
        private void DrawEntityAabb(Rendering.Abstractions.IRenderView view, IEntity entity, Box2 clipBox)
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

            view.Context.Primitives.DrawRectangle(view, aabb, Color4.Yellow);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black[0], Color4.Black[1], Color4.Black[2], Color4.Black[3]);

            font.Draw(view, groupCmp.Id.ToString(), Color4.White, clipBox);

            GL.Disable(EnableCap.Blend);

            view.PopMatrix();
        }

        #endregion Private Methods
    }
}