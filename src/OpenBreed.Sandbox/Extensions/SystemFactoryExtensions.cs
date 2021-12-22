using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Worlds.Wecs.Systems;
using OpenBreed.Wecs.Systems;
using System;

namespace OpenBreed.Sandbox.Extensions
{
    public static class SystemFactoryExtensions
    {
        #region Public Methods

        public static void SetupUnknownMapCellDisplaySystem(this ISystemFactory systemFactory, IServiceProvider serviceProvider)
        {
            systemFactory.Register(() => new UnknownMapCellDisplaySystem(serviceProvider.GetService<IPrimitiveRenderer>(),
                                                                         serviceProvider.GetService<IFontMan>()));
        }

        public static void SetupGroupMapCellDisplaySystem(this ISystemFactory systemFactory, IServiceProvider serviceProvider)
        {
            systemFactory.Register(() => new GroupMapCellDisplaySystem(serviceProvider.GetService<IPrimitiveRenderer>(),
                                                                       serviceProvider.GetService<IFontMan>()));
        }

        #endregion Public Methods
    }
}