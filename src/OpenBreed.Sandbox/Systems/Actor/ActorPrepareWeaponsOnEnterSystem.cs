using Microsoft.Extensions.Logging;
using OpenBreed.Audio.Interface;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Abstractions.Extensions;
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
using OpenBreed.Wecs.Abstractions.Systems;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public class ActorPrepareWeaponsOnEnterSystem : IEntityOnTriggerActionSystem
    {
        #region Private Fields

        private readonly IGameServices services;
        private readonly IWeaponMan weaponMan;

        #endregion Private Fields

        #region Public Constructors

        public ActorPrepareWeaponsOnEnterSystem(IGameServices services, IWeaponMan weaponMan)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.weaponMan = weaponMan ?? throw new ArgumentNullException(nameof(weaponMan));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "EnterWorld";

        public string ActionName => "PrepareWeapons";

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

                var currentWeapon = weaponMan.GetWeapon(currentWeaponNo);

                var pos = entity.GetPosition();
                var dir = MovementTools.SnapToCompass8Way(entity.GetDirection());

                pos = pos + dir * 16;
                var thrust = dir * currentWeapon.Speed * speedFactor;

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
                else if (currentWeapon.Name == "Flamethrower")
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

            void SwitchToNextWeapon(IEntity entity)
            {
                currentWeaponNo++;

                if (currentWeaponNo > 4)
                {
                    currentWeaponNo = 1;
                }

                var currentWeapon = weaponMan.GetWeapon(currentWeaponNo);

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
        }

        #endregion Public Methods
    }
}