using OpenBreed.Common.Game.Wecs.Systems;
using OpenBreed.Common.Game.Wecs.Systems.Actor;
using OpenBreed.Common.Game.Wecs.Systems.Cursor;
using OpenBreed.Common.Game.Wecs.Systems.Turret;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Gui.Systems.Extensions;
using OpenBreed.Wecs.Physics.Systems;
using OpenBreed.Wecs.Physics.Systems.Extensions;



//using OpenBreed.Wecs.Physics.Systems;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenBreed.Wecs.Scripting.Systems;
using OpenBreed.Wecs.Scripting.Systems.Extensions;

namespace OpenBreed.Common.Game.Wecs.Extensions
{
    public static class WorldBuilderExtensions
    {
        #region Public Methods

        public static IWorldBuilder AddGameWorldSystems(this IWorldBuilder builder, bool isEditor)
        {
            //Update Stage
            //builder.AddGameLogicSystems();

            builder.AddPhysicsSystems();
            builder.AddSystem<SolidCollisionHandlerSystem>();
            builder.AddSystem<SlowdownObstacleCollisionSystem>();
            builder.AddSystem<SlopeObstacleCollisionSystem>();
            builder.AddSystem<TriggerCollisionHandlerSystem>();
            builder.AddSystem<Projectile2TriggerCollisionHandlerSystem>();
            builder.AddSystem<ItemPickupSystem>();
            builder.AddSystem<ItemManagingSystem>();
            builder.AddSystem<LivesSystem>();
            builder.AddSystem<ActorSystem>();
            builder.AddSystem<DamageOnHealthDistributionSystem>();
            builder.AddSystem<DestroyOnZeroHealthSystem>();
            builder.AddSystem<TurretTrackingSystem>();
            builder.AddSystem<TurretTrackLockingSystem>();
            builder.AddSystem<TurretTrackUnlockingSystem>();
            builder.AddSystem<RefreshCursorOnWorldUpdateSystem>();
            builder.AddSystem<FollowPositionSystem>();

            builder.AddControlSystems();

            builder.AddCoreSystems();

            //Audio Stage
            builder.AddSoundSystems();

            builder.AddGuiSystems(isEditor);

            //Video Stage
            builder.AddRenderingSystems();

            builder.AddSystem<UnknownMapCellDisplaySystem>();

            builder.AddScriptingSystems();

            return builder;
        }

        #endregion Public Methods
    }
}