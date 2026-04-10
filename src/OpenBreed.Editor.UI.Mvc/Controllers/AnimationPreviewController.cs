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
using OpenBreed.Core.Abstractions.Managers;
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
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Systems;
using OpenBreed.Rendering.OpenGL.Managers;

namespace OpenBreed.Editor.UI.Mvc.Controllers
{
    public class AnimationPreviewController : IController
    {
        #region Private Fields

        private readonly AnimationPreviewView view;
        private readonly IAnimationSandbox animationSandbox;
        private bool pendingReset;

        #endregion Private Fields

        #region Public Constructors

        public AnimationPreviewController(
            IEventsMan eventsMan,
            AnimationPreviewView view,
            IAnimationSandbox animationSandbox)
        {
            this.view = view;
            this.animationSandbox = animationSandbox;

            view.CursorDown += OnCursorDown;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Reset()
        {
            view.Reset();
        }

        #endregion Public Methods

        #region Private Methods

        private void OnCursorDown(ViewCursorDownEvent e)
        {
            if (e.Key == CursorKey.Right)
            {
                view.AutoCenter();
            }
        }

        #endregion Private Methods
    }
}