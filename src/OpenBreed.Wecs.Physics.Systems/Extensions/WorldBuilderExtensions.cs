using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Physics.Systems.Extensions
{
    public static class WorldBuilderExtensions
    {
        #region Public Methods

        public static IWorldBuilder AddPhysicsSystems(this IWorldBuilder builder)
        {
            builder.AddSystem<VelocityChangedSystem>();
            builder.AddSystem<PositionTrackingSystem>();
            builder.AddSystem<MovementSystemVanilla>();
            builder.AddSystem<DirectionSystemVanilla>();
            builder.AddSystem<AddDynamicBodySystem>();
            builder.AddSystem<RemoveDynamicBodySystem>();
            builder.AddSystem<UpdateDynamicBodySystem>();
            builder.AddSystem<DynamicBodiesCollisionCheckSystem>();
            builder.AddSystem<AddStaticBodySystem>();
            builder.AddSystem<RemoveStaticBodySystem>();

            return builder;
        }

        #endregion Public Methods
    }
}