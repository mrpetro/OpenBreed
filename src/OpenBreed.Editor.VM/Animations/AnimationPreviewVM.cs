using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc;
using OpenBreed.Editor.UI.Mvc.Controllers;
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
    public class AnimationPreviewVM : BaseViewModel
    {
        #region Private Fields

        private readonly IServiceProvider serviceProvider;
        private readonly EditorView editorView;
        private readonly IAnimationSandbox animationSandbox;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly AnimationPreviewController controller;

        #endregion Private Fields

        #region Public Constructors

        public AnimationPreviewVM(
            IServiceProvider serviceProvider,
            EditorView editorView,
            IAnimationSandbox animationSandbox,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceProvider = serviceProvider;
            this.editorView = editorView;
            this.animationSandbox = animationSandbox;
            this.serviceScopeFactory = serviceScopeFactory;
            this.controller = ActivatorUtilities.CreateInstance<AnimationPreviewController>(serviceProvider, editorView, animationSandbox);
        }

        #endregion Public Constructors

        #region Public Properties

        public Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> InitFunc => OnInitialize;

        public IDbAnimation Animation { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void View(IDbAnimation dbAnimation)
        {
            Animation = dbAnimation;

            controller?.Reset();
        }

        #endregion Public Methods

        #region Private Methods

        private IRenderContext OnInitialize(IGraphicsContext graphicsContext, HostCoordinateSystemConverter hostCoordinateSystemConverter)
        {
            var serviceScope = serviceScopeFactory.CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<IRenderContextProvider>().SetupScope(hostCoordinateSystemConverter, graphicsContext);

            var renderContext = serviceScope.ServiceProvider.GetRequiredService<IRenderContext>();

            var dataLoaderFactory = serviceScope.ServiceProvider.GetService<IDataLoaderFactory>();
            var clipLoader = dataLoaderFactory.GetLoader<IAnimationClipDataLoader<IEntity>>();

            editorView.RenderContext = renderContext;

            var model = clipLoader.Load(Animation);

            animationSandbox.Load(model.Name);

            return renderContext;
        }

        #endregion Private Methods
    }
}