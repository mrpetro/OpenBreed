using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Helpers;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Wecs.Entities;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Mvc.Views
{
    public class AnimationPreviewView : EditorView
    {
        #region Private Fields

        private readonly IAnimationSandbox model;

        #endregion Private Fields

        #region Public Constructors

        public AnimationPreviewView(
            IEventsMan eventsMan,
            IInteractionFactoryProvider guiFactoryProvider,
            IAnimationSandbox model) : base(eventsMan, guiFactoryProvider)
        {
            this.model = model;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Reset()
        {
            if (RenderView is null)
            {
                return;
            }

            SetScaleLimits(1.0f / (float)Math.Pow(2, 8), (float)Math.Pow(2, 10));
            base.Reset();
        }

        #endregion Public Methods

        #region Internal Methods

        internal void AutoCenter()
        {
            var extent = model.GetGraphicalExtent();

            var scaleX = 1.0f;
            var scaleY = 1.0f;

            if (extent.Size.X > 0)
            {
                scaleX = (float)RenderView.Box.Size.X / (float)extent.Size.X;
            }

            if (extent.Size.Y > 0)
            {
                scaleY = (float)RenderView.Box.Size.Y / (float)extent.Size.Y;
            }

            if (Math.Abs(scaleX) < 0.1)
            {
                scaleX = 0.1f;
            }

            if (Math.Abs(scaleY) < 0.1)
            {
                scaleY = 0.1f;
            }

            var offset = new Vector2(-extent.Center.X, -extent.Center.Y);

            RenderView.SetScale(scaleX, scaleY);
            RenderView.MoveTo(RenderView.Box.HalfSize);

            var sx = scaleX;
            var sy = scaleY;

            RenderView.MoveBy((Vector2i)(offset * new Vector2(sx, sy)));
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void OnRender(IRenderView view, float dt)
        {
            view.PushMatrix();

            view.EnableAlpha();

            RenderAxes(view);

            view.DisableAlpha();

            model.Render(view, dt);

            view.PopMatrix();
        }

        protected override Vector2 GetInteractionSnapCursorPosition(Vector2 worldPosition)
        {
            var stepX = (1, 1);
            var stepY = (1, 1);
            var snappedPosition = MyMathHelper.Snap(new Vector2(worldPosition.X, worldPosition.Y), stepX, stepY);

            return snappedPosition;
        }

        #endregion Protected Methods

        #region Private Methods

        private void RenderAxes(IRenderView view)
        {
            var worldBox = view.ToWorldBox(view.Box);

            view.Context.Primitives.DrawLine(view, new Vector2(worldBox.Min.X, 0), new Vector2(worldBox.Max.X, 0), Color4.Red);
            view.Context.Primitives.DrawLine(view, new Vector2(0, worldBox.Min.Y), new Vector2(0, worldBox.Max.Y), Color4.Green);
        }

        #endregion Private Methods
    }
}