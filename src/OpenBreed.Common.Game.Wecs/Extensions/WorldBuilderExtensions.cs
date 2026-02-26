using OpenBreed.Common.Game.Wecs.Systems;
using OpenBreed.Common.Game.Wecs.Systems.Actor;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Animation.Systems;
using OpenBreed.Wecs.Audio.Systems;
using OpenBreed.Wecs.Control.Systems;
using OpenBreed.Wecs.Core.Systems;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Gui.Systems;
using OpenBreed.Wecs.Physics.Systems;
using OpenBreed.Wecs.Rendering.Systems;
using OpenBreed.Wecs.Scripting.Systems;
using OpenBreed.Common.Game.Wecs.Systems.Cursor;

namespace OpenBreed.Common.Game.Wecs.Extensions
{
    public static class WorldBuilderExtensions
    {
        #region Public Methods

        public static void SetupScreenWorldSystems(this IWorldBuilder builder)
        {
            //Input Stage
            builder.AddSystem<ActorMovementByPlayerInputsSystem>();

            //Video

            builder.AddSystem<ViewportSystem>();
            builder.AddSystem<SoundSystem>();
            builder.AddSystem<TimerSystem>();
            builder.AddSystem<FrameSystem>();
        }

        public static void SetupLimboWorldSystems(this IWorldBuilder builder)
        {
            builder.AddSystem<ResurrectionSystem>();
            builder.AddSystem<TimerSystem>();
        }

        public static void SetupGameWorldSystems(this IWorldBuilder builder, bool isEditor)
        {
            //Update Stage
            //builder.AddGameLogicSystems();

            builder.AddSystem<MovementSystemVanilla>();
            builder.AddSystem<DirectionSystemVanilla>();

            builder.AddSystem<AddDynamicBodySystem>();
            builder.AddSystem<RemoveDynamicBodySystem>();
            builder.AddSystem<UpdateDynamicBodySystem>();
            builder.AddSystem<DynamicBodiesCollisionCheckSystem>();
            builder.AddSystem<AddStaticBodySystem>();
            builder.AddSystem<RemoveStaticBodySystem>();
            builder.AddSystem<OnAddEntityTriggerSystem>();
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
            builder.AddSystem<LifetimeSystem>();
            builder.AddSystem<EntityEmitterSystem>();
            builder.AddSystem<TurretTrackingSystem>();
            builder.AddSystem<TurretTrackLockingSystem>();
            builder.AddSystem<TurretTrackUnlockingSystem>();
            builder.AddSystem<RefreshCursorOnWorldUpdateSystem>();

            builder.AddSystem<FollowerSystem>();
            builder.AddSystem<AnimatorSystem>();
            builder.AddSystem<TimerSystem>();
            builder.AddSystem<FrameSystem>();
            builder.AddSystem<PausingSystem>();
            builder.AddSystem<FsmSystem>();
            builder.AddSystem<VelocityChangedSystem>();

            builder.AddSystem<StampPutterSystem>();
            builder.AddSystem<TilePutterSystem>();

            //Audio Stage
            builder.AddSystem<SoundSystem>();

            //Video Stage
            builder.AddSystem<TileRenderSystem>();
            builder.AddSystem<SpriteSystem>();
            builder.AddSystem<PictureSystem>();
            builder.AddSystem<TextSystem>();
            builder.AddSystem<CollisionVisualizingSystem>();
            builder.AddSystem<UnknownMapCellDisplaySystem>();
            builder.AddSystem<ViewportSystem>();

            builder.AddSystem<ScriptRunningSystem>();

            if (!isEditor)
            {
                //GUI Stage
                builder.AddSystem<CursorSystem>();
            }

            //Reset Stage
        }

        #endregion Public Methods
    }
}