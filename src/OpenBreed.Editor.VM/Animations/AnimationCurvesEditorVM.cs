using Microsoft.Extensions.DependencyInjection;
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
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;

namespace OpenBreed.Editor.VM.Animations
{
    public class AnimationCurvesEditorVM : BaseViewModel, IClipEditorModel
    {
        #region Private Fields

        private readonly IServiceProvider serviceProvider;
        private readonly EditorView editorView;
        private readonly IAnimationSandbox animationSandbox;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly AnimationCurvesEditorController controller;

        #endregion Private Fields

        #region Public Constructors

        public AnimationCurvesEditorVM(
            IServiceProvider serviceProvider,
            EditorView editorView,
            IAnimationSandbox animationSandbox,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceProvider = serviceProvider;
            this.editorView = editorView;
            this.animationSandbox = animationSandbox;
            this.serviceScopeFactory = serviceScopeFactory;
            this.controller = ActivatorUtilities.CreateInstance<AnimationCurvesEditorController>(serviceProvider, editorView, this);

            //eventsMan.Subscribe<RenderContextInitializedEvent>((rc) =>
            //{
            //    controller.Reset();
            //});
        }

        #endregion Public Constructors

        #region Public Properties

        public Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> InitFunc => OnInitialize;

        public IDbAnimationTrack Track { get; private set; }
        public float ClipLength { get; private set; }

        public IReadOnlyCollection<IDbAnimationTrack> Tracks => throw new NotImplementedException();

        #endregion Public Properties

        #region Public Methods

        public void Edit(IDbAnimationTrack dbTrack)
        {
            Track = dbTrack;

            controller?.Reset();
        }

        #endregion Public Methods

        #region Private Methods

        private IRenderContext OnInitialize(IGraphicsContext graphicsContext, HostCoordinateSystemConverter hostCoordinateSystemConverter)
        {
            var serviceScope = serviceScopeFactory.CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<IRenderContextProvider>().SetupScope(hostCoordinateSystemConverter, graphicsContext);
            var renderContext = serviceScope.ServiceProvider.GetRequiredService<IRenderContext>();

            editorView.RenderContext = renderContext;

            return renderContext;
        }

        #endregion Private Methods
    }
}