using Microsoft.Extensions.Logging;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Control.Systems.Events;
using OpenBreed.Wecs.Control.Systems.Helpers;
using OpenBreed.Wecs.Core.Systems.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenTK.Mathematics;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Extensions;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public class OnActorControlActionSystem : IEventSystem<EntityActionEvent<PlayerActions>>
    {
        #region Private Fields

        private const int fireCooldownTime = 1000;
        private static readonly int[] flamethrowerOffsets = [0, 1, 2, 1, 0, -1, -2, -1];
        private readonly IGameServices services;
        private readonly IWeaponMan weaponMan;

        #endregion Private Fields

        #region Public Constructors

        public OnActorControlActionSystem(IGameServices services, IWeaponMan weaponMan)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.weaponMan = weaponMan ?? throw new ArgumentNullException(nameof(weaponMan));
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(EntityActionEvent<PlayerActions> e, IWorld world)
        {
            var entity = services.Entities.GetById(e.EntityId);

            var speedFactor = 30;

            switch (e.ActionCode)
            {
                case PlayerActions.Fire:
                    FireBullet(entity);
                    break;

                case PlayerActions.SwitchWeapon:
                    SwitchToNextWeapon(entity);
                    break;

                default:
                    break;
            }

            var gameWorld = services.Worlds.GetWorld(entity);
            var missionEntity = services.Entities.GetMission(gameWorld.Id);

            void CooldownFinish(IEntity entity, TimerElapsedEventArgs e)
            {
                entity.Get<WeaponsComponent>().FireReady = true;
            }

            void SwitchToNextWeapon(IEntity entity)
            {
                var currentWraponNo = entity.NextWeapon();

                var currentWeapon = weaponMan.GetWeapon(currentWraponNo);

                services.Logger.LogInformation("Switching weapon to: {0}", currentWeapon.Name);
            }

            void FireBullet(IEntity entity)
            {
                var weapons = entity.Get<WeaponsComponent>();

                if (!weapons.FireReady)
                {
                    return;
                }

                var currentWeaponNo = weapons.CurrentWeaponNo;

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
                    var weaponState = weapons.CurrentWeaponState;

                    weaponState++;

                    if (weaponState > 7)
                    {
                        weaponState = 1;
                    }

                    var flamethrowerOffset = flamethrowerOffsets[weaponState];

                    weapons.CurrentWeaponState = weaponState;

                    var perp = new Vector2(thrust.Y, -thrust.X);
                    perp.Normalize();

                    thrust = thrust + perp * flamethrowerOffset * speedFactor;

                    var emitId = emitter.SetOption("thrustX", thrust.X)
                        .SetOption("thrustY", thrust.Y)
                        .Finish();
                }
                else
                {
                    emitter.SetOption("thrustX", thrust.X)
                        .SetOption("thrustY", thrust.Y)
                        .Finish();
                }

                weapons.FireReady = false;

                services.Triggers.AfterDelay(
                    entity,
                    TimeSpan.FromMilliseconds(fireCooldownTime / currentWeapon.FireRate),
                    CooldownFinish,
                    singleTime: true);
            }
        }

        #endregion Public Methods
    }
}