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
    public class ActorPrepareMovementOnEnterSystem : IEntityOnTriggerActionSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public ActorPrepareMovementOnEnterSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "EnterWorld";

        public string ActionName => "PrepareMovement";

        #endregion Public Properties

        #region Public Methods

        public void OnTrigger(IEntity triggeringEntity, IEntity triggerEntity)
        {
            var cooldownTimerId = triggerEntity.GetTimerId("CooldownDelay");
            var delayTimerId = triggerEntity.GetTimerId("ActionDeley");

            var gameWorld = services.Worlds.GetWorld(triggeringEntity);
            var missionEntity = services.Entities.GetMission(gameWorld.Id);

            services.EntityTriggers.TryOnTrigger("HeroEnter", triggerEntity, missionEntity);

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

                entity.State = "Dead";

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