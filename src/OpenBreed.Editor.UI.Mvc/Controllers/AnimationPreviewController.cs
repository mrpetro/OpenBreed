using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using OpenBreed.Editor.UI.Mvc.Views;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Rendering.Abstractions.Extensions;
using System.Drawing;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc.Extensions;
using OpenBreed.Common;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Common.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems;
using OpenBreed.Rendering.OpenGL.Managers;

namespace OpenBreed.Editor.UI.Mvc.Controllers
{
    public class AnimationPreviewController : IController
    {
        #region Private Fields

        private readonly EditorView view;
        private readonly IAnimationSandbox animationSandbox;
        private bool pendingReset;

        #endregion Private Fields

        #region Public Constructors

        public AnimationPreviewController(
            IEventsMan eventsMan,
            EditorView view,
            IAnimationSandbox animationSandbox)
        {
            this.view = view;
            this.animationSandbox = animationSandbox;

            view.Rendering += OnRender;
            view.CursorDown += OnCursorDown;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Reset()
        {
            view.SetScaleLimits(1.0f / (float)Math.Pow(2, 8), (float)Math.Pow(2, 8));
            view.Reset();
        }

        #endregion Public Methods

        #region Private Methods

        private void OnReset(IRenderView view)
        {
            view.SetScale(2.0f);
            view.MoveTo(view.Box.HalfSize);
        }

        private void OnRender(IRenderView view, float dt)
        {
            view.PushMatrix();

            if (pendingReset)
            {
                OnReset(view);
                pendingReset = false;
            }

            view.EnableAlpha();

            RenderAxes(view);

            view.DisableAlpha();

            animationSandbox.Render(view, dt);

            view.PopMatrix();
        }

        private void OnCursorDown(ViewCursorDownEvent e)
        {
            if (e.Key == CursorKey.Left)
            {
                //var cursorPos = GetCellIndexCoords(e.View, e.Position) + new Vector4i(model.CenterX, model.CenterY, 0, 1);

                //model.PutTiles(cursorPos, CurrentTileAtlasId, CurrentTileSelection);
            }
            else if (e.Key == CursorKey.Right)
            {
                //var cursorPos = GetCellIndexCoords(e.View, e.Position) + new Vector4i(model.CenterX, model.CenterY, 0, 1);

                //model.EraseTile(cursorPos);
            }
        }

        private void RenderAxes(IRenderView view)
        {
            var worldBox = view.ToWorldBox(view.Box);

            view.Context.Primitives.DrawLine(view, new Vector2(worldBox.Min.X, 0), new Vector2(worldBox.Max.X, 0), Color4.Red);
            view.Context.Primitives.DrawLine(view, new Vector2(0, worldBox.Min.Y), new Vector2(0, worldBox.Max.Y), Color4.Green);
        }

        #endregion Private Methods
    }
}