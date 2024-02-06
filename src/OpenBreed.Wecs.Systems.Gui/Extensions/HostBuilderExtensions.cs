﻿using OpenBreed.Common;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Entities;
using System;
using OpenBreed.Core;
using OpenBreed.Input.Interface;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface.Managers;

namespace OpenBreed.Wecs.Systems.Gui.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupGuiSystems(this ISystemFactory systemFactory, IServiceProvider sp)
        {
            systemFactory.RegisterSystem<CollisionVisualizingSystem>(
                () => new CollisionVisualizingSystem(
                    sp.GetService<IEntityMan>(),
                    sp.GetService<IPrimitiveRenderer>(),
                    sp.GetService<ICollisionMan<IEntity>>()));

            systemFactory.RegisterSystem<CursorSystem>(
                () => new CursorSystem(
                    sp.GetRequiredService<IViewClient>(),
                    sp.GetRequiredService<IInputsMan>(),
                    sp.GetRequiredService<IPrimitiveRenderer>(),
                    sp.GetRequiredService<IEventsMan>()));
        }

        #endregion Public Methods
    }
}