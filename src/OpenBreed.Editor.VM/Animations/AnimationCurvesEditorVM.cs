using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Core.Abstractions.Managers;
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
using System.Collections.ObjectModel;
using System.ComponentModel;

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

        public AnimationCurvesEditorToolsVM Tools { get; }

        private IRenderView renderView;
        private bool actionsShow;

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
            Tools = ActivatorUtilities.CreateInstance<AnimationCurvesEditorToolsVM>(serviceProvider, controller);
        }

        #endregion Public Constructors

        #region Public Properties

        public bool ActionsShow
        {
            get { return actionsShow; }
            set { SetProperty(ref actionsShow, value); }
        }

        public LoadContextHandler InitFunc => OnInitialize;

        public ObservableCollection<ContextAction> Actions { get; } = new ObservableCollection<ContextAction>();

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

        public void Edit(IDbAnimationTrack dbTrack)
        {
            model.Edit(dbTrack);

            controller.Focus();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(ActionsShow):
                    if (ActionsShow)
                    {
                        RefreshActions();
                    }
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods


        private void RefreshActions()
        {
            Actions.Clear();
            Actions.Add(new ContextAction("Insert mode", () => controller.SetMode(AnimationCurvesEditorMode.InsertKeyFrames)));
            Actions.Add(new ContextAction("Select mode", () => controller.SetMode(AnimationCurvesEditorMode.SelectKeyFrames)));
            SetupStandardActions();
        }

        private void SetupStandardActions()
        {
            Actions.Add(new ContextAction("Auto center", () => controller.Focus()));
        }

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