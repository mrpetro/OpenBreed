using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc.Controllers;
using OpenBreed.Editor.UI.Mvc.Views;
using OpenBreed.Editor.VM.Base;
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
        private readonly IServiceScopeFactory serviceScopeFactory;
        private AnimationPreviewController controller;
        private IClip<IEntity> model;

        #endregion Private Fields

        #region Public Constructors

        public AnimationPreviewVM(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceProvider = serviceProvider;
            this.serviceScopeFactory = serviceScopeFactory;
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
            serviceScope.ServiceProvider.GetRequiredService<IRenderContextFactory>().SetupScope(hostCoordinateSystemConverter, graphicsContext);

            var renderContext = serviceScope.ServiceProvider.GetRequiredService<IRenderContext>();
            var eventsMan = serviceScope.ServiceProvider.GetRequiredService<IEventsMan>();

            var dataLoaderFactory = serviceScope.ServiceProvider.GetService<IDataLoaderFactory>();
            var clipLoader = dataLoaderFactory.GetLoader<IAnimationClipDataLoader<IEntity>>();


            var model = clipLoader.Load(Animation);

            controller = ActivatorUtilities.CreateInstance<AnimationPreviewController>(serviceScope.ServiceProvider, model);

            return renderContext;
        }

        #endregion Private Methods
    }
}