using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Extensions
{
    public static class WorldExtensions
    {
        #region Public Methods

        public static void CreateView(this IWorld world, IRenderContext renderContext)
        {
            var view = renderContext.CreateView();
            world.AddToView(view);
        }

        public static void AddToView(this IWorld world, IRenderView renderView)
        {
            renderView.Rendering += (s, dt) =>
            {
                renderView.RenderWorld(world, 0, new Box2(renderView.Box.Min, renderView.Box.Max), dt);
            };
        }

        #endregion Public Methods
    }
}