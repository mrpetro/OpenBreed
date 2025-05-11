using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Rendering;
using OpenBreed.Gui.Builders;
using OpenBreed.Gui.Rendering;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui
{
    internal class InteractionFactoryProvider : IInteractionFactoryProvider
    {
        #region Private Fields

        private readonly IServiceProvider serviceProvider;

        #endregion Private Fields

        #region Public Constructors

        public InteractionFactoryProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public IInteractionFactory GetFactory(IRenderView view)
        {
            return new InteractionFactory(serviceProvider, view);
        }

        #endregion Public Methods
    }

    internal class InteractionFactory : IInteractionFactory
    {
        #region Public Constructors

        public InteractionFactory(IServiceProvider serviceProvider, IRenderView view)
        {
            ServiceProvider = serviceProvider;
            View = view;
        }

        #endregion Public Constructors

        #region Public Properties

        public IServiceProvider ServiceProvider { get; }
        public IRenderView View { get; }

        #endregion Public Properties

        #region Public Methods

        public IDesktop CreateDesktop(Action<IDesktopBuilder> setter)
        {
            var builder = ActivatorUtilities.CreateInstance<DesktopBuilder>(ServiceProvider, View);

            setter.Invoke(builder);

            return builder.Build();
        }

        #endregion Public Methods
    }
}