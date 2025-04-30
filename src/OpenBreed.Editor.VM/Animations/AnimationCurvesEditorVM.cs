using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc.Controllers;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Editor.UI.Mvc.Views;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Factories;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Animations
{
    public class AnimationCurvesEditorVM : BaseViewModel, IClipEditorModel
    {
        #region Private Fields

        private readonly IServiceProvider serviceProvider;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private AnimationCurvesEditorController controller;

        #endregion Private Fields

        #region Public Constructors

        public AnimationCurvesEditorVM(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceProvider = serviceProvider;
            this.serviceScopeFactory = serviceScopeFactory;
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
            serviceScope.ServiceProvider.GetRequiredService<IRenderContextFactory>().SetupScope(hostCoordinateSystemConverter, graphicsContext);

            var renderContext = serviceScope.ServiceProvider.GetRequiredService<IRenderContext>();
            var eventsMan = serviceScope.ServiceProvider.GetRequiredService<IEventsMan>();

            controller = ActivatorUtilities.CreateInstance<AnimationCurvesEditorController>(serviceScope.ServiceProvider, this);

            eventsMan.Subscribe<RenderContextInitializedEvent>((rc) =>
            {
                controller.Reset();
            });

            return renderContext;
        }

        #endregion Private Methods
    }
}