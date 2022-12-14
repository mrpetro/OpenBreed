using OpenBreed.Common;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Entities;
using System;

namespace OpenBreed.Wecs.Systems.Gui.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupPhysicsDebugSystem(this ISystemFactory systemFactory, IServiceProvider serviceProvider)
        {
            systemFactory.RegisterSystem<PhysicsDebugDisplaySystem>(
                (world) => new PhysicsDebugDisplaySystem(world, serviceProvider.GetService<IPrimitiveRenderer>()));
        }

        #endregion Public Methods
    }
}