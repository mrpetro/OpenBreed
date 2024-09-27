using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Rendering.Interface.Factories;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Factories
{
    internal class RenderViewFactory : IRenderViewFactory
    {
        #region Private Fields

        private readonly IServiceProvider serviceProvider;

        #endregion Private Fields

        #region Public Constructors

        public RenderViewFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public TRenderView CreateView<TRenderView>(IRenderContext renderContext, Box2 viewBox) where TRenderView : IRenderView
        {
            return ActivatorUtilities.CreateInstance<TRenderView>(serviceProvider, renderContext, viewBox);
        }

        #endregion Public Methods
    }
}