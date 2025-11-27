using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc;
using OpenBreed.Editor.UI.Mvc.Controllers;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Editor.UI.Mvc.Views;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Messages;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Factories;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Wecs.Entities;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Animations
{
    public class AnimationPreviewVM : BaseViewModel, IRenderViewVM
    {
        #region Private Fields

        private readonly IServiceProvider serviceProvider;
        private readonly AnimationPreviewView view;
        private readonly IAnimationSandbox animationSandbox;
        private readonly IAnimationEditorModel model;
        private readonly AnimationPreviewController controller;
        private IRenderView renderView;

        #endregion Private Fields

        #region Public Constructors

        public AnimationPreviewVM(
            IServiceProvider serviceProvider,
            IAnimationSandbox animationSandbox,
            IAnimationEditorModel model)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.animationSandbox = animationSandbox ?? throw new ArgumentNullException(nameof(animationSandbox));
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.view = ActivatorUtilities.CreateInstance<AnimationPreviewView>(serviceProvider, animationSandbox);
            this.controller = ActivatorUtilities.CreateInstance<AnimationPreviewController>(serviceProvider, view, animationSandbox);

            controller?.Reset();
        }

        #endregion Public Constructors

        #region Public Properties

        public LoadContextHandler InitFunc => OnControlInitialize;

        #endregion Public Properties

        #region Public Methods

        public void Activate()
        {
            renderView?.Activate();
        }

        public void Deactivate()
        {
            renderView?.Deactivate();
        }

        #endregion Public Methods

        #region Private Methods

        private void OnControlInitialize(out IRenderContextProvider renderContextProvider, out Action<IRenderContext> contextInitializer)
        {
            renderContextProvider = serviceProvider.GetRequiredService<IRenderContextProvider>();

            contextInitializer = (context) =>
            {
                if (renderView is null)
                {
                    renderView = context.CreateView(activate: true);

                    view.RenderView = renderView;

                    //var clip = model.Load();

                    animationSandbox.Load(model.Name, context);
                }
            };
        }

        #endregion Private Methods
    }
}