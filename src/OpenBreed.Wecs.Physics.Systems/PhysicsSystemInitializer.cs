using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Physics.Systems.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Physics.Systems
{
    internal class PhysicsSystemInitializer : ISystemInitializer
    {
        #region Public Methods

        public void Initialize(IServiceProvider serviceProvider, ISystem system)
        {
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