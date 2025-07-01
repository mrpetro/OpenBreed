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

        public LoadContextHandler InitFunc => OnInitialize;

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

        private void OnInitialize(out IRenderContextProvider renderContextProvider, out Action<IRenderContext> contextInitializer)
        {
            renderContextProvider = serviceProvider.GetRequiredService<IRenderContextProvider>();

            contextInitializer = (context) =>
            {
                var dataLoaderFactory = context.ServiceProvider.GetService<IDataLoaderFactory>();
                var clipLoader = dataLoaderFactory.GetLoader<IAnimationClipDataLoader<IEntity>>();

                editorView.RenderContext = context;

                var model = clipLoader.Load(Animation);

                animationSandbox.Load(model.Name, context);
            };
        }

        #endregion Private Methods
    }
}