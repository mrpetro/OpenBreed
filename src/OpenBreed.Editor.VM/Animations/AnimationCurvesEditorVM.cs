using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc;
using OpenBreed.Editor.UI.Mvc.Controllers;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Editor.UI.Mvc.Views;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Factories;
using OpenBreed.Wecs.Entities;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;

namespace OpenBreed.Editor.VM.Animations
{
    public class AnimationCurvesEditorVM : BaseViewModel, IRenderViewVM
    {
        #region Private Fields

        private readonly IServiceProvider serviceProvider;
        private readonly AnimationCurvesEditorView view;
        private readonly IAnimationSandbox animationSandbox;
        private readonly IAnimationEditorModel model;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly AnimationCurvesEditorController controller;
        private IRenderView renderView;

        #endregion Private Fields

        #region Public Constructors

        public AnimationCurvesEditorVM(
            IServiceProvider serviceProvider,
            IAnimationSandbox animationSandbox,
            IServiceScopeFactory serviceScopeFactory,
            IAnimationEditorModel model)
        {
            this.serviceProvider = serviceProvider;
            this.animationSandbox = animationSandbox;
            this.serviceScopeFactory = serviceScopeFactory;
            this.model = model;
            this.view = ActivatorUtilities.CreateInstance<AnimationCurvesEditorView>(serviceProvider, model, animationSandbox);
            this.controller = ActivatorUtilities.CreateInstance<AnimationCurvesEditorController>(serviceProvider, view, model);
        }

        #endregion Public Constructors

        #region Public Properties

        public LoadContextHandler InitFunc => OnInitialize;

        public float ClipLength => model.ClipLength;
        public IReadOnlyCollection<IDbAnimationTrack> Tracks => model.Tracks;

        public void Activate()
        {
            renderView?.Activate();
        }

        public void Deactivate()
        {
            renderView?.Deactivate();
        }

        #endregion Public Properties

        #region Public Methods

        public void Edit(IDbAnimationTrack dbTrack)
        {
            controller.Reset();
        }

        #endregion Public Methods

        #region Private Methods

        private void OnInitialize(out IRenderContextProvider renderContextProvider, out Action<IRenderContext> contextInitializer)
        {
            renderContextProvider = serviceProvider.GetRequiredService<IRenderContextProvider>();

            contextInitializer = (context) =>
            {
                if (renderView is null)
                {
                    renderView = context.CreateView(activate: true);

                    view.RenderView = renderView;
                }
            };
        }

        #endregion Private Methods
    }
}