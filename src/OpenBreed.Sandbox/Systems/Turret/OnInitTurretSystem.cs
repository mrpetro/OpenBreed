using Microsoft.Extensions.Logging;
using OpenBreed.Audio.Abstractions;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Wecs.Control.Systems.Events;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Control.Systems.Helpers;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Core.Systems.Events;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Physics.Systems.Events;
using OpenBreed.Wecs.Physics.Systems.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public class OnInitTurretSystem : IEventSystem<EntityEnteredEvent>
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public OnInitTurretSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(
            [TargetWorldAsSourceFilter]
            [SourceWorldWithNameFilter(WorldNames.Game)]
            [EntityTriggerActionFilter("EnterWorld", "PrepareTurret")]
            EntityEnteredEvent e, IWorld world)
        {
            var entity = services.Entities.GetById(e.EntityId);

            var previousDegree = 0.0f;
            var speedFactor = 150.0f;
            var fireRate = 0.3f;
            var fireReady = true;

            services.Triggers.OnEntityDirectionChanged(
                entity,
                OnDirectionChanged,
                false);

            services.Triggers.OnTrackingTarget(
                entity,
                OnTrackingTarget,
                false);

            services.Triggers.OnDestroyed(
                entity,
                Explode,
                false);

            void Explode(IEntity entity, DestroyedEvent e)
            {
                var pos = entity.GetPosition();
                entity.StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
                    .SetOption("flavor", "Big")
                    .SetOption("startX", pos.X)
                    .SetOption("startY", pos.Y)
                    .Finish();

                services.Worlds.RequestRemoveEntity(entity);
                services.Entities.RequestErase(entity);
            }

            void OnDirectionChanged(IEntity entity, DirectionChangedEvent e)
            {
                var pos = entity.GetPosition();
                var targetDirection = entity.GetTargetDirection();
                var direction = entity.GetDirection();

                var degree = MovementTools.SnapToCompass16Degree(direction.X, direction.Y);

                if (degree != previousDegree)
                {
                    var soundId = services.Sounds.GetByName("Vanilla/Common/Turret/Turn");

                    entity.EmitSound(soundId);

                    var clipId = services.Clips.GetId($"Vanilla/L1/Turret/Tracking/{degree.ToString("0.0", CultureInfo.InvariantCulture)}");

                    entity.PlayAnimation(0, clipId);
                    previousDegree = degree;
                }
            }

            void OnTrackingTarget(IEntity entity, TrackingTargetEvent e)
            {
                var trackedEntity = services.Entities.GetById(e.TrackedEntityId);
                var trackedPosition = trackedEntity.GetPosition();
                entity.SetTargetDirectionToCoordinates(trackedPosition);

                TryFireBullet(entity);
            }

            void CooldownFinish(IEntity entity, TimerElapsedEventArgs e)
            {
                fireReady = true;
            }

            void TryFireBullet(IEntity entity)
            {
                if (!fireReady)
                {
                    return;
                }

                var pos = entity.GetPosition();
                var dir = MovementTools.SnapToCompass16Way(entity.GetDirection());

                pos = pos + dir * 16;
                var thrust = dir * speedFactor;

                entity.StartEmit("ABTA\\Templates\\L1\\TurretLazer")
                    .SetOption("startX", pos.X)
                    .SetOption("startY", pos.Y)
                    .SetOption("thrustX", thrust.X)
                    .SetOption("thrustY", thrust.Y)
                    .SetTag("TurretLazer")
                    .Finish();

                services.Logger.LogInformation("TURRET.FIRE");

                fireReady = false;

                services.Triggers.AfterDelay(entity, TimeSpan.FromMilliseconds(1000 / fireRate), CooldownFinish);
            }
        }

        #endregion Public Methods
    }
}