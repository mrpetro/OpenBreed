using OpenBreed.Common;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Systems.Hud;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Sandbox.Systems;
using OpenBreed.Sandbox.Systems.Actor;
using OpenBreed.Sandbox.Systems.Camera;
using OpenBreed.Sandbox.Systems.Game;
using OpenBreed.Sandbox.Systems.Mission;
using OpenBreed.Sandbox.Systems.MissionScreen;
using OpenBreed.Sandbox.Systems.SmartCard;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Control.Systems;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Rendering.Systems;
using OpenBreed.Wecs.Scripting.Systems;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Extensions
{
    public static class WorldManExtensions
    {
        #region Public Methods

        public static IWorld CreateDebugHud(this IWorldMan worldMan)
        {
            return worldMan.Create()
                .SetName(WorldNames.DebugHud)
                .AddSystem<AnimatorSystem>()
                .AddSystem<TextSystem>()
                .AddSystem<DebugHudSystem>()
                .AddSystem<DebugHudInitSystem>()
                .AddSystem<ScriptRunningSystem>()
                .AddSystem<CameraSettingPaletteSystem>()
                .Build();
        }

        public static IWorld CreateGameHud(this IWorldMan worldMan)
        {
            return worldMan.Create().SetName(WorldNames.GameHud)
            .AddSystem<AnimatorSystem>()
            .AddSystem<SpriteSystem>()
            .AddSystem<TextSystem>()
            .AddSystem<ScriptRunningSystem>()
            .AddSystem<GameHudUpdatingSystem>()
            .AddSystem<GameHudInitSystem>()
            .AddSystem<CameraSettingPaletteSystem>()
            .Build();
        }

        public static IWorld CreateSmartCardReader(this IWorldMan worldMan)
        {
            return worldMan.Create()
                .SetName(WorldNames.SmartCardReader)
                .AddSystem<AnimatorSystem>()
                .AddSystem<SpriteSystem>()
                .AddSystem<PictureSystem>()
                .AddSystem<TextSystem>()
                .AddSystem<SmartCardInitSystem>()
                .AddSystem<CameraSettingPaletteSystem>()
                .Build();
        }

        public static IWorld CreateGameWorld(this IWorldMan worldMan, string name)
        {
            return worldMan.Create()
                .SetName(name)
                .AddSystem<CameraSettingPaletteSystem>()
                .AddSystem<OnActorTouchDoorTriggerService>()
                .AddSystem<OnActorTouchExitTriggerSystem>()
                .AddSystem<OnActorTouchTeleportTriggerSystem>()
                .AddSystem<OnActorTouchItemTriggerSystem>()
                .AddSystem<OnActorTouchSmartCardTriggerSystem>()
                .AddSystem<OnActorTouchLandMineTriggerSystem>()
                .AddSystem<ExplosionOnEnterWorldSystem>()
                .AddSystem<TurretSystem>()
                .AddSystem<ActorAnimateSystem>()
                .AddSystem<ActorResurectSystem>()
                .AddSystem<OnRefractionLazerProjectileHitSystem>()
                .AddSystem<OnDefaultProjectileHitSystem>()
                .AddSystem<OnInitFirewallProjectileSystem>()
                .AddSystem<OnInitMissileProjectileSystem>()
                .AddSystem<OnInitTrilazerGunProjectileSystem>()
                .AddSystem<OnInitTurretLazerProjectileSystem>()
                .AddSystem<OnInitRefractionLazerProjectileSystem>()
                .AddSystem<OnActorInitShowMissionSystem>()
                .AddSystem<OnActorControlActionSystem>()
                .AddSystem<LynetteVoiceSystem>()
                .AddSystem<GameInitSystem>()
                .AddGameWorldSystems(isEditor: false)
                .Build();
        }

        public static IWorld CreateMissionScreen(this IWorldMan worldMan)
        {
            return worldMan.Create()
                .SetName("MissionScreen")
                .AddSystem<AnimatorSystem>()
                .AddSystem<SpriteSystem>()
                .AddSystem<PictureSystem>()
                .AddSystem<TextSystem>()
                .AddSystem<MissionScreenInitSystem>()
                .AddSystem<CameraSettingPaletteSystem>()
                .Build();
        }

        public static void SetEntityPosition(this IWorldMan worldMan, IEntity target, int entryId)
        {
            var world = worldMan.GetById(target.WorldId);

            var entryEntity = WorldExtensions.GetTopLeftMostEntity(world.FindEntryEntities(entryId));

            if (entryEntity is null)
            {
                entryEntity = WorldExtensions.GetTopLeftMostEntity(world.FindEntryEntities(2));
            }

            if (entryEntity is null)
            {
                throw new Exception($"No entry with ID '{entryId}' found.");
            }

            var entryPos = entryEntity.Get<PositionComponent>();
            var targetPos = target.Get<PositionComponent>();

            var newPosition = entryPos.Value;

            targetPos.Value = newPosition;

            var velocityCmp = target.Get<VelocityComponent>();
            velocityCmp.Value = Vector2.Zero;

            var thrustCmp = target.Get<ThrustComponent>();
            thrustCmp.Value = Vector2.Zero;

            target.State = null;
        }

        #endregion Public Methods
    }
}