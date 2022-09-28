using OpenBreed.Sandbox.Worlds.Wecs.Systems;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Audio;
using OpenBreed.Wecs.Systems.Control;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Gui;
using OpenBreed.Wecs.Systems.Physics;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Extensions
{
    public static class WorldBuilderExtensions
    {
        #region Public Methods

        public static void SetupGameWorldSystems(this WorldBuilder builder, ISystemFactory systemFactory)
        {
            //Input Stage
            builder.AddSystem(systemFactory.Create<AiControlSystem>());
            builder.AddSystem(systemFactory.Create<WalkingControllerSystem>());
            builder.AddSystem(systemFactory.Create<AttackControllerSystem>());

            //Update Stage
            builder.AddSystem(systemFactory.Create<MovementSystemVanilla>());
            builder.AddSystem(systemFactory.Create<DirectionSystemVanilla>());

            //builder.AddSystem(new FollowerSystem(core));
            builder.AddSystem(systemFactory.Create<DynamicBodiesAabbUpdaterSystem>());
            builder.AddSystem(systemFactory.Create<DynamicBodiesCollisionCheckSystem>());
            builder.AddSystem(systemFactory.Create<StaticBodiesSystem>());
            //builder.AddSystem(systemFactory.Create<CollisionResponseSystem>());

            builder.AddSystem(systemFactory.Create<FollowerSystem>());

            builder.AddSystem(systemFactory.Create<AnimatorSystem>());
            builder.AddSystem(systemFactory.Create<TimerSystem>());
            builder.AddSystem(systemFactory.Create<PausingSystem>());
            builder.AddSystem(systemFactory.Create<FsmSystem>());

            //Audio Stage
            builder.AddSystem(systemFactory.Create<SoundSystem>());

            //Video Stage
            builder.AddSystem(systemFactory.Create<StampSystem>());
            builder.AddSystem(systemFactory.Create<TileSystem>());
            builder.AddSystem(systemFactory.Create<SpriteSystem>());
            builder.AddSystem(systemFactory.Create<PictureSystem>());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            builder.AddSystem(systemFactory.Create<TextSystem>());
            //builder.AddSystem(systemFactory.Create<PhysicsDebugDisplaySystem>());
            builder.AddSystem(systemFactory.Create<UnknownMapCellDisplaySystem>());
            //builder.AddSystem(systemFactory.Create<GroupMapCellDisplaySystem>());
            builder.AddSystem(systemFactory.Create<ViewportSystem>());
        }

        #endregion Public Methods
    }
}