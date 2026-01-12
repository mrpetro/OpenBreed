using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Systems.Physics.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics
{
    internal class PhysicsSystemInitializer : ISystemInitializer
    {
        #region Public Methods

        public void Initialize(IServiceProvider serviceProvider, ISystem system)
        {
            if (system is IMatchingSystem)
            {
                var systemRequirementsProvider = serviceProvider.GetRequiredService<ISystemRequirementsProvider>();
                systemRequirementsProvider.RegisterRequirements(system.GetType());
            }

            if (system is IOnEntityCollisionSystem onEntityCollisionSystem)
            {
                var collisionMan = serviceProvider.GetRequiredService<ICollisionMan<IEntity>>();

                foreach (var colliderType in onEntityCollisionSystem.ColliderTypesB)
                {
                    collisionMan.RegisterFixturePair(
                        onEntityCollisionSystem.ColliderTypeA,
                        colliderType,
                        onEntityCollisionSystem.OnCollision);
                }
            }
        }

        #endregion Public Methods
    }
}