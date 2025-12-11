using Microsoft.Extensions.Logging;
using OpenBreed.Audio.Interface;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Audio.Extensions;
using OpenBreed.Wecs.Systems.Control;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public class ActorPrepareOnEnterSystem : IEntityOnTriggerActionSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public ActorPrepareOnEnterSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "EnterWorld";

        public string ActionName => "Prepare";

        #endregion Public Properties

        #region Public Methods

        public void OnTrigger(IEntity triggeringEntity, IEntity triggerEntity)
        {
            var cooldownTimerId = triggerEntity.GetTimerId("CooldownDelay");
            var delayTimerId = triggerEntity.GetTimerId("ActionDeley");
            var currentWeaponNo = 0;
            var flamethrowerOffsetIndex = 0;
            var fireReady = true;
            var fireCooldownTime = 1000;
            var speedFactor = 30;

            int[] flamethrowerOffsets = [0, 1, 2, 1, 0, -1, -2, -1];

            (string Name, string Projectile, int FireRate, string MuzzleFlash, int Speed)[] weapons =
                [
                ("AssaultGun",      "AssaultGun",      25, "AssaultGun", 12 * speedFactor),
                ("MissileLauncher", "Missile",         1,  "",           10 * speedFactor),
                ("TrilazerGun",     "TrilazerGun",     7,  "",           12 * speedFactor),
                ("Flamethrower",    "Firewall",        25, "",           10 * speedFactor),
                ("RefractionGun",   "RefractionLazer", 10, "",           8 * speedFactor)];

            var actions = new Dictionary<PlayerActions, Action<IEntity>> {
                { PlayerActions.Fire, FireBullet },
                { PlayerActions.SwitchWeapon, SwitchToNextWeapon }
            };

            var gameWorld = services.Worlds.GetWorld(triggeringEntity);
            var missionEntity = services.Entities.GetMission(gameWorld.Id);

            services.EntityTriggers.TryOnTrigger("HeroEnter", triggerEntity, missionEntity);

            services.Triggers.OnEntityAction(
                    triggerEntity,
                    CheckAction,
                    false);

            services.Triggers.OnEntityDirectionChanged(
                triggerEntity,
                OnDirectionChanged,
                false);

            services.Triggers.OnEntityVelocityChanged(
                triggerEntity,
                OnVelocityChanged,
                false);

            services.Triggers.OnDestroyed(
                triggerEntity,
                SendToLimbo,
                false);


            void CooldownFinish(IEntity entity, TimerElapsedEventArgs e)
            {
                fireReady = true;
            }

            void FireBullet(IEntity entity)
            {
                if (!fireReady)
                {
                    return;
                }

                var currentWeapon = weapons[currentWeaponNo];

                var pos = entity.GetPosition();
                var dir = MovementTools.SnapToCompass8Way(entity.GetDirection());

                pos = pos + dir * 16;
                var thrust = dir * currentWeapon.Speed;

                var emitter = entity.StartEmit($"ABTA\\Templates\\Common\\Projectiles\\{currentWeapon.Projectile}")
                    .SetOption("startX", pos.X)
                    .SetOption("startY", pos.Y);

                if (currentWeapon.Name == "TrilazerGun")
                {
                    emitter.SetOption("thrustX", thrust.X)
                            .SetOption("thrustY", thrust.Y)
                            .Finish();

                    var perp = new Vector2(dir.Y, -dir.X);

                    var p1Thrust = thrust + perp * 2 * speedFactor;

                    emitter.SetOption("thrustX", p1Thrust.X)
                           .SetOption("thrustY", p1Thrust.Y)
                           .Finish();

                    var p2Thrust = thrust - perp * 2 * speedFactor;

                    emitter.SetOption("thrustX", p2Thrust.X)
                           .SetOption("thrustY", p2Thrust.Y)
                           .Finish();

                }
                else if(currentWeapon.Name == "Flamethrower")
                {
                    flamethrowerOffsetIndex++;

                    if (flamethrowerOffsetIndex > 7)
                    {
                        flamethrowerOffsetIndex = 1;
                    }

                    var flamethrowerOffset = flamethrowerOffsets[flamethrowerOffsetIndex];

                    var perp = new Vector2(thrust.Y, -thrust.X);
                    perp.Normalize();

                    thrust = thrust + perp * flamethrowerOffset * speedFactor;

                    emitter.SetOption("thrustX", thrust.X)
                        .SetOption("thrustY", thrust.Y)
                        .Finish();
                }
                else
                {
                    emitter.SetOption("thrustX", thrust.X)
                        .SetOption("thrustY", thrust.Y)
                        .Finish();
                }

                fireReady = false;
                services.Triggers.AfterDelay(
                    entity,
                    cooldownTimerId,
                    TimeSpan.FromMilliseconds(1000 / currentWeapon.FireRate),
                    CooldownFinish, singleTime: true);
            }

            void OnDirectionChanged(IEntity entity, DirectionChangedEvent e)
            {
                var targetDirection = entity.GetTargetDirection();
                var direction = entity.GetDirection();
                var animDirName = AnimHelper.ToDirectionName(direction);

                var isMoving = entity.IsMoving();
                var movementStateName = default(string);

                if (isMoving)
                {
                    movementStateName = "Walking";
                }
                else
                {
                    movementStateName = "Standing";
                }

                var clipId = services.Clips.GetId($"Vanilla/Common/Actor/{movementStateName}/{animDirName}");
                entity.PlayAnimation(0, clipId);
            }

            void OnVelocityChanged(IEntity entity, VelocityChangedEvent e)
            {
                var targetDirection = entity.GetTargetDirection();
                var direction = entity.GetDirection();
                var animDirName = AnimHelper.ToDirectionName(direction);

                var isMoving = entity.IsMoving();
                var movementStateName = default(string);

                if (isMoving)
                {
                    movementStateName = "Walking";
                    var clipId = services.Clips.GetId($"Vanilla/Common/Actor/{movementStateName}/{animDirName}");
                    entity.PlayAnimation(0, clipId);
                }
                else
                {
                    movementStateName = "Standing";
                    var clipId = services.Clips.GetId($"Vanilla/Common/Actor/{movementStateName}/{animDirName}");
                    entity.StopAnimation(0);
                }
            }

            void SwitchToNextWeapon(IEntity entity)
            {
                currentWeaponNo++;

                if (currentWeaponNo > 4)
                {
                    currentWeaponNo = 1;
                }

                var currentWeapon = weapons[currentWeaponNo];

                services.Logger.LogInformation("Switching weapon to: {0}", currentWeapon.Name);
            }

            void CheckAction(IEntity entity, EntityActionEvent<PlayerActions> e)
            {
                if (actions.TryGetValue(e.ActionCode, out var func))
                {
                    func.Invoke(entity);
                }
                else
                {
                    services.Logger.LogError("Missing implementation for action: {0}", e.ActionCode);
                }
 
            }

            void SendToLimbo(IEntity entity, DestroyedEvent e)
            {
                var pos = entity.GetPosition();
                entity.StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
                    .SetOption("flavor", "Small")
                    .SetOption("startX", pos.X)
                    .SetOption("startY", pos.Y)
                    .Finish();


                var soundId = services.Sounds.GetByName("Vanilla/Common/Hero/Dying");
                entity.EmitSound(soundId);

                entity.SetResurrectable(entity.WorldId);

                //-- Wait 3 seconds

                //-- Appear
                //-- Enable grace period for 3 seconds

                services.Logger.LogInformation("Player Died!");

                var limboWorld = services.Worlds.GetByName("Limbo");

                services.Worlds.RequestAddEntity(entity, limboWorld.Id);

                services.Triggers.OnEntityEnteredWorld(
                    entity,
                    PostEnter,
                    true);
            }


            void PostEnter(IEntity entity, EntityEnteredEvent e)
            {
                var w = services.Worlds.GetById(e.WorldId);

                services.Logger.LogInformation($"Entering {0}", w.Name);

                if (w.Name == "Limbo")
                {
                    Wait3Seconds(entity);
                }
            }

            void Wait3Seconds(IEntity entity)
            {
                services.Logger.LogInformation("Wait 3 seconds...");

                services.Triggers.AfterDelay(
                    entity,
                    delayTimerId,
                    TimeSpan.FromMilliseconds(3000),
                    Resurrect,
                    true);
            }

            void Resurrect(IEntity entity, TimerElapsedEventArgs e)
            {
                services.Logger.LogInformation("Resurrect...");
                entity.RestoreFullHealth();
                entity.Resurrect();
            }
        }


        #endregion Public Methods
    }
}