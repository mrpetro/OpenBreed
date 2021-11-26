using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities
{
    public enum TeleportType
    {
        In,
        Out
    }

    public struct TeleportPair : IEquatable<TeleportPair>
    {
        #region Public Fields

        public int Id;
        public TeleportType Type;

        #endregion Public Fields

        #region Public Methods

        public bool Equals(TeleportPair other)
        {
            return Type == other.Type && Id == other.Id;
        }

        #endregion Public Methods
    }

    public class TeleportHelper
    {
        #region Public Fields

        public const string SPRITE_TELEPORT_ENTRY = "Atlases/Sprites/Teleport/Entry";

        public const string SPRITE_TELEPORT_EXIT = "Atlases/Sprites/Teleport/Exit";

        #endregion Public Fields

        #region Private Fields

        private const string ANIMATION_TELEPORT_ENTRY = "Animations/Teleport/Entry";

        private const string ANIMATION_TELEPORT_EXIT = "Animations/Teleport/Exit";
        private readonly IClipMan clipMan;
        private readonly IWorldMan worldMan;

        private readonly IEntityMan entityMan;

        private readonly IEntityFactory entityFactory;

        private readonly IEventsMan eventsMan;

        private readonly ICollisionMan collisionMan;
        private readonly IBuilderFactory builderFactory;
        private readonly IJobsMan jobMan;
        private readonly IShapeMan shapeMan;

        #endregion Private Fields

        #region Public Constructors

        public TeleportHelper(IClipMan clipMan, IWorldMan worldMan, IEntityMan entityMan, IEntityFactory entityFactory, IEventsMan eventsMan, ICollisionMan collisionMan, IBuilderFactory builderFactory, IJobsMan jobMan, IShapeMan shapeMan)
        {
            this.clipMan = clipMan;
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.entityFactory = entityFactory;
            this.eventsMan = eventsMan;
            this.collisionMan = collisionMan;
            this.builderFactory = builderFactory;
            this.jobMan = jobMan;
            this.shapeMan = shapeMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public Entity AddTeleportEntry(World world, int x, int y, int pairId, string tileAtlasName, int gfxValue)
        {
            var teleportEntry = entityFactory.Create(@"Entities\Common\TeleportEntry.xml")
                .SetParameter("tileSet", tileAtlasName)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            teleportEntry.Tag = new TeleportPair { Id = pairId, Type = TeleportType.In };

            teleportEntry.EnterWorld(world.Id);
            return teleportEntry;
        }

        public void RegisterCollisionPairs()
        {
            //collisionMan.RegisterCollisionPair(ColliderTypes.ActorBody, ColliderTypes.TeleportEntryTrigger, Actor2TriggerCallback);
            collisionMan.RegisterFixturePair(ColliderTypes.ActorTrigger, ColliderTypes.TeleportEntryTrigger, Actor2TriggerCallbackEx);
        }

        public Entity AddTeleportExit(World world, int x, int y, int pairId, string tileAtlasName, int gfxValue)
        {
            var teleportExit = entityFactory.Create(@"Entities\Common\TeleportExit.xml")
                .SetParameter("tileSet", tileAtlasName)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            teleportExit.Tag = new TeleportPair { Id = pairId, Type = TeleportType.Out };

            //teleportExit.PutTile(atlasId, gfxValue, 0, new Vector2(16 * x, 16 * y));

            teleportExit.EnterWorld(world.Id);

            return teleportExit;
        }

        public void SetPosition(Entity target, Entity entryEntity, bool cancelMovement)
        {
            var pair = (TeleportPair)entryEntity.Tag;
            pair.Type = TeleportType.Out;
            var exitEntity = entityMan.GetByTag(pair).FirstOrDefault(item => item != entryEntity);

            if (exitEntity == null)
                throw new Exception("No exit entity found");

            var exitPos = exitEntity.Get<PositionComponent>();
            var targetPos = target.Get<PositionComponent>();

            var bodyCmp = target.Get<BodyComponent>();
            var shape = shapeMan.GetById(bodyCmp.Fixtures.First().ShapeId);
            var targetAabb = shape.GetAabb().Translated(targetPos.Value);

            var offset = new Vector2((32 - targetAabb.Width) / 2.0f, (32 - targetAabb.Height) / 2.0f);

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
            if (Equals(actorEntity.State, "Teleporting"))
                return;

            actorEntity.State = "Teleporting";

            var cameraEntity = actorEntity.TryGet<FollowerComponent>()?.FollowerIds.
                                                                              Select(item => entityMan.GetById(item)).
                                                                              FirstOrDefault(item => item.Tag is "PlayerCamera");

            if (cameraEntity == null)
                return;

            var cameraFadeOutClipId = clipMan.GetByName(CameraHelper.CAMERA_FADE_OUT).Id;
            var cameraFadeInClipId = clipMan.GetByName(CameraHelper.CAMERA_FADE_IN).Id;


            var pair = (TeleportPair)teleportEntity.Tag;

            var jobChain = new JobChain();

            var worldIdToRemoveFrom = actorEntity.WorldId;

            //Pause this world
            jobChain.Equeue(new WorldJob<WorldPausedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => worldMan.GetById(worldIdToRemoveFrom).Pause()));
            //Fade out camera
            jobChain.Equeue(new EntityJob<AnimFinishedEventArgs>(cameraEntity, () => cameraEntity.PlayAnimation(0, cameraFadeOutClipId)));
            //Remove entity from this world
            //jobChain.Equeue(new WorldJob<EntityRemovedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => commandsMan.Post(new RemoveEntityCommand(actorEntity.WorldId, actorEntity.Id))));
            //Set position of entity to entry position in next world
            jobChain.Equeue(new EntityJob(() => SetPosition(actorEntity, teleportEntity, true)));
            //Unpause this world
            jobChain.Equeue(new WorldJob<WorldUnpausedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => worldMan.GetById(worldIdToRemoveFrom).Unpause()));
            //Fade in camera
            jobChain.Equeue(new EntityJob<AnimFinishedEventArgs>(cameraEntity, () => cameraEntity.PlayAnimation(0, cameraFadeInClipId)));

            jobMan.Execute(jobChain);
        }

        #endregion Private Methods
    }
}