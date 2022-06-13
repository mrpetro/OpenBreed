﻿using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities
{
    public class TeleportHelper
    {
        #region Public Fields

        public const string SPRITE_TELEPORT_ENTRY = "Atlases/Sprites/Teleport/Entry";

        public const string SPRITE_TELEPORT_EXIT = "Atlases/Sprites/Teleport/Exit";

        #endregion Public Fields

        #region Private Fields

        private const string ANIMATION_TELEPORT_ENTRY = "Animations/Teleport/Entry";

        private const string ANIMATION_TELEPORT_EXIT = "Animations/Teleport/Exit";
        private readonly IClipMan<Entity> clipMan;
        private readonly IWorldMan worldMan;

        private readonly IEntityMan entityMan;

        private readonly IEntityFactory entityFactory;

        private readonly IEventsMan eventsMan;
        private readonly ITriggerMan triggerMan;
        private readonly ICollisionMan<Entity> collisionMan;
        private readonly IBuilderFactory builderFactory;
        private readonly IJobsMan jobMan;
        private readonly IShapeMan shapeMan;

        #endregion Private Fields

        #region Public Constructors

        public TeleportHelper(IClipMan<Entity> clipMan, IWorldMan worldMan, IEntityMan entityMan, IEntityFactory entityFactory, IEventsMan eventsMan, ITriggerMan triggerMan, ICollisionMan<Entity> collisionMan, IBuilderFactory builderFactory, IJobsMan jobMan, IShapeMan shapeMan)
        {
            this.clipMan = clipMan;
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.entityFactory = entityFactory;
            this.eventsMan = eventsMan;
            this.triggerMan = triggerMan;
            this.collisionMan = collisionMan;
            this.builderFactory = builderFactory;
            this.jobMan = jobMan;
            this.shapeMan = shapeMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public Entity AddTeleportEntry(World world, int x, int y, int pairId, string level, int gfxValue)
        {
            var teleportEntry = entityFactory.Create(@"Vanilla\Templates\ABTA\Common\TeleportEntry.xml")
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            teleportEntry.Tag = $"TeleportEntry/{pairId}";

            teleportEntry.EnterWorld(world.Id);
            return teleportEntry;
        }

        public void RegisterCollisionPairs()
        {
            //collisionMan.RegisterCollisionPair(ColliderTypes.ActorBody, ColliderTypes.TeleportEntryTrigger, Actor2TriggerCallback);
            collisionMan.RegisterFixturePair(ColliderTypes.ActorTrigger, ColliderTypes.TeleportEntryTrigger, Actor2TriggerCallbackEx);
        }

        public Entity AddTeleportExit(World world, int x, int y, int pairId, string level, int gfxValue)
        {
            var teleportExit = entityFactory.Create(@"Vanilla\Templates\ABTA\Common\TeleportExit.xml")
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            teleportExit.Tag = $"TeleportExit/{pairId}";

            //teleportExit.PutTile(atlasId, gfxValue, 0, new Vector2(16 * x, 16 * y));

            teleportExit.EnterWorld(world.Id);

            return teleportExit;
        }

        public void SetPosition(Entity target, Entity entryEntity, bool cancelMovement)
        {
            var pairId = entryEntity.Tag.Split('/')[1];
            // Search for all exits from same world as entry with same pair ID 
            var exitEntity = entityMan.GetByTag($"TeleportExit/{pairId}").FirstOrDefault(item => item.WorldId == entryEntity.WorldId);

            if (exitEntity is null)
                throw new Exception("No exit entity found");

            var exitPos = exitEntity.Get<PositionComponent>();
            var targetPos = target.Get<PositionComponent>();

            var bodyCmp = target.Get<BodyComponent>();
            var shape = shapeMan.GetById(bodyCmp.Fixtures.First().ShapeId);
            var targetAabb = shape.GetAabb().Translated(targetPos.Value);

            var offset = new Vector2((32 - targetAabb.Size.X) / 2.0f, (32 - targetAabb.Size.Y) / 2.0f);

            var newPosition = exitPos.Value + offset;

            targetPos.Value = newPosition;

            if (cancelMovement)
            {
                var velocityCmp = target.Get<VelocityComponent>();
                velocityCmp.Value = Vector2.Zero;

                var thrustCmp = target.Get<ThrustComponent>();
                thrustCmp.Value = Vector2.Zero;
            }

            target.State = null;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnFrameUpdate(Entity entity, int nextValue)
        {
            entity.SetSpriteImageId(nextValue);
        }

        private void Actor2TriggerCallbackEx(BodyFixture colliderTypeA, Entity entityA, BodyFixture colliderTypeB, Entity entityB, Vector2 projection)
        {
                PerformEntityExit(entityA, entityB);
        }

        private void PerformEntityExit(Entity actorEntity, Entity teleportEntity)
        {
            // For preventing running rest of the code when actor will hit couple of teleporter blocks at same time
            if (Equals(actorEntity.State, "Teleporting"))
                return;

            actorEntity.State = "Teleporting";

            var cameraEntity = actorEntity.GetFollowers().FollowerIds.
                                                                              Select(item => entityMan.GetById(item)).
                                                                              FirstOrDefault(item => item.Tag is "PlayerCamera");

            if (cameraEntity == null)
                return;

            var cameraFadeOutClipId = clipMan.GetByName(CameraHelper.CAMERA_FADE_OUT).Id;
            var cameraFadeInClipId = clipMan.GetByName(CameraHelper.CAMERA_FADE_IN).Id;

            var jobChain = new JobChain();

            var targetWorldId = actorEntity.WorldId;


            //Pause this world
            jobChain.Equeue(new EntityJob<WorldPausedEventArgs>(triggerMan, cameraEntity, () => cameraEntity.PauseWorld()));
            //Fade out camera
            jobChain.Equeue(new EntityJob<AnimFinishedEventArgs>(triggerMan, cameraEntity, () => cameraEntity.PlayAnimation(0, cameraFadeOutClipId)));
            //Set position of entity to teleport exit
            jobChain.Equeue(new InstantJob(() => SetPosition(actorEntity, teleportEntity, true)));
            //Unpause this world
            jobChain.Equeue(new EntityJob<WorldUnpausedEventArgs>(triggerMan, cameraEntity, () => cameraEntity.UnpauseWorld()));
            //Fade in camera
            jobChain.Equeue(new EntityJob<AnimFinishedEventArgs>(triggerMan, cameraEntity, () => cameraEntity.PlayAnimation(0, cameraFadeInClipId)));

            jobMan.Execute(jobChain);
        }

        #endregion Private Methods
    }
}