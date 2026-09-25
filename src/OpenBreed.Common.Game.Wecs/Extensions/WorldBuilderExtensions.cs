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

            builder.AddPhysicsSystems()
                .AddSystem<SolidCollisionHandlerSystem>()
                .AddSystem<SlowdownObstacleCollisionSystem>()
                .AddSystem<SlopeObstacleCollisionSystem>()
                .AddSystem<Projectile2TriggerCollisionHandlerSystem>()
                .AddSystem<ItemManagingSystem>()
                .AddSystem<LivesSystem>()
                .AddSystem<ActorSystem>()
                .AddSystem<DamageOnHealthDistributionSystem>()
                .AddSystem<DestroyOnZeroHealthSystem>()
                .AddSystem<TurretTrackingSystem>()
                .AddSystem<TurretTrackLockingSystem>()
                .AddSystem<TurretTrackUnlockingSystem>()
                .AddSystem<RefreshCursorOnWorldUpdateSystem>()
                .AddSystem<FollowPositionSystem>()
                .AddSystem<MovableEnemyControlSystem>();

            builder.AddControlSystems();

            builder.AddCoreSystems();

            //Audio Stage
            builder.AddSoundSystems();

            //Video Stage
            builder.AddRenderingSystems();

            builder.AddSystem<UnknownMapCellDisplaySystem>();
            builder.AddGuiSystems(isEditor);

            builder.AddScriptingSystems();

            return builder;
        }

        #endregion Public Methods
    }
}