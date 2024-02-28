using OpenBreed.Sandbox.Wecs.Systems;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Audio;
using OpenBreed.Wecs.Systems.Control;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Gui;
using OpenBreed.Wecs.Systems.Physics;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Scripting;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Extensions
{
    public static class WorldBuilderExtensions
    {
        #region Public Methods

        public static void SetupLimboWorldSystems(this IWorldBuilder builder)
        {
            builder.AddSystem<ResurrectionSystem>();
            builder.AddSystem<TimerSystem>();
        }

        public static void SetupGameWorldSystems(this IWorldBuilder builder)
        {
            //Update Stage
            builder.AddSystem<MovementSystemVanilla>();
            builder.AddSystem<DirectionSystemVanilla>();

            //builder.AddSystem(new FollowerSystem(core));
            builder.AddSystem<AddDynamicBodySystem>();
            builder.AddSystem<RemoveDynamicBodySystem>();
            builder.AddSystem<UpdateDynamicBodySystem>();
            builder.AddSystem<DynamicBodiesCollisionCheckSystem>();
            builder.AddSystem<AddStaticBodySystem>();
            builder.AddSystem<RemoveStaticBodySystem>();
            //builder.AddSystem<StaticBodiesSystem>();
            //builder.AddSystem(systemFactory.Create<CollisionResponseSystem>());

            builder.AddSystem<ItemPickupSystem>();
            builder.AddSystem<ItemManagingSystem>();
            builder.AddSystem<LivesSystem>();
            builder.AddSystem<DamageOnHealthDistributionSystem>();
            builder.AddSystem<DestroyOnZeroHealthSystem>();
            builder.AddSystem<LifetimeSystem>();
            builder.AddSystem<EntityEmitterSystem>();
            builder.AddSystem<TurretTrackingSystem>();
            builder.AddSystem<TurretTrackLockingSystem>();
            builder.AddSystem<TurretTrackUnlockingSystem>();

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
            //builder.AddSystem<SoundSystem>();

            //Video Stage
            builder.AddSystem<TileRenderSystem>();
            builder.AddSystem<SpriteSystem>();
            builder.AddSystem<PictureSystem>();
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            builder.AddSystem<TextSystem>();
            builder.AddSystem<CollisionVisualizingSystem>();
            builder.AddSystem<UnknownMapCellDisplaySystem>();
            //builder.AddSystem(systemFactory.Create<GroupMapCellDisplaySystem>());
            builder.AddSystem<ViewportSystem>();

            builder.AddSystem<ScriptRunningSystem>();


            //GUI Stage
            builder.AddSystem<CursorSystem>();

            //Reset Stage
        }

        #endregion Public Methods
    }
}