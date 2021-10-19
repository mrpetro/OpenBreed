using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Tools;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems.Animation.Commands;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Teleport
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

        private readonly ICommandsMan commandsMan;

        private readonly IEventsMan eventsMan;

        private readonly ICollisionMan collisionMan;
        private readonly IBuilderFactory builderFactory;
        private readonly IJobsMan jobMan;
        private readonly IShapeMan shapeMan;

        #endregion Private Fields

        #region Public Constructors

        public TeleportHelper(IClipMan clipMan, IWorldMan worldMan, IEntityMan entityMan, IEntityFactory entityFactory, ICommandsMan commandsMan, IEventsMan eventsMan, ICollisionMan collisionMan, IBuilderFactory builderFactory, IJobsMan jobMan, IShapeMan shapeMan)
        {
            this.clipMan = clipMan;
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.entityFactory = entityFactory;
            this.commandsMan = commandsMan;
            this.eventsMan = eventsMan;
            this.collisionMan = collisionMan;
            this.builderFactory = builderFactory;
            this.jobMan = jobMan;
            this.shapeMan = shapeMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void CreateAnimations()
        {
            var animationTeleportEntry = clipMan.CreateClip(ANIMATION_TELEPORT_ENTRY, 4.0f);
            var te = animationTeleportEntry.AddTrack<int>(FrameInterpolation.None, (e, nv) => OnFrameUpdate(e, nv), 0);
            te.AddFrame(0, 1.0f);
            te.AddFrame(1, 2.0f);
            te.AddFrame(2, 3.0f);
            te.AddFrame(3, 4.0f);
        }

        public Entity AddTeleportEntry(World world, int x, int y, int pairId, int atlasId, int gfxValue)
        {
            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Teleport\TeleportEntry.xml");
            var teleportEntry = entityFactory.Create(entityTemplate);

            teleportEntry.Tag = new TeleportPair { Id = pairId, Type = TeleportType.In };

            teleportEntry.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);

            var tileComponentBuilder = builderFactory.GetBuilder<TileComponentBuilder>();
            tileComponentBuilder.SetAtlasById(atlasId);
            tileComponentBuilder.SetImageIndex(gfxValue);
            teleportEntry.Add(tileComponentBuilder.Build());

            commandsMan.Post(new AddEntityCommand(world.Id, teleportEntry.Id));
            return teleportEntry;
        }

        public void RegisterCollisionPairs()
        {
            //collisionMan.RegisterCollisionPair(ColliderTypes.ActorBody, ColliderTypes.TeleportEntryTrigger, Actor2TriggerCallback);
            collisionMan.RegisterFixturePair(ColliderTypes.ActorTrigger, ColliderTypes.TeleportEntryTrigger, Actor2TriggerCallbackEx);
        }

        public Entity AddTeleportExit(World world, int x, int y, int pairId, int atlasId, int gfxValue)
        {
            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Teleport\TeleportExit.xml");
            var teleportExit = entityFactory.Create(entityTemplate);
            //var teleportExit = core.GetManager<IEntityMan>().CreateFromTemplate("TeleportExit");

            teleportExit.Tag = new TeleportPair { Id = pairId, Type = TeleportType.Out };

            teleportExit.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);

            var tileComponentBuilder = builderFactory.GetBuilder<TileComponentBuilder>();
            tileComponentBuilder.SetAtlasById(atlasId);
            tileComponentBuilder.SetImageIndex(gfxValue);
            teleportExit.Add(tileComponentBuilder.Build());

            commandsMan.Post(new AddEntityCommand(world.Id, teleportExit.Id));
            //world.AddEntity(teleportExit);

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
            commandsMan.Post(new SpriteSetCommand(entity.Id, nextValue));
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

            var pair = (TeleportPair)teleportEntity.Tag;

            var jobChain = new JobChain();

            var worldIdToRemoveFrom = actorEntity.WorldId;

            //Pause this world
            jobChain.Equeue(new WorldJob<WorldPausedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => commandsMan.Post(new PauseWorldCommand(worldIdToRemoveFrom, true))));
            //Fade out camera
            jobChain.Equeue(new EntityJob<AnimStoppedEventArgs>(cameraEntity, () => commandsMan.Post(new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_OUT, 0))));
            //Remove entity from this world
            //jobChain.Equeue(new WorldJob<EntityRemovedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => commandsMan.Post(new RemoveEntityCommand(actorEntity.WorldId, actorEntity.Id))));
            //Set position of entity to entry position in next world
            jobChain.Equeue(new EntityJob(() => SetPosition(actorEntity, teleportEntity, true)));
            //Unpause this world
            jobChain.Equeue(new WorldJob<WorldUnpausedEventArgs>(worldMan, eventsMan, (s, a) => { return a.WorldId == worldIdToRemoveFrom; }, () => commandsMan.Post(new PauseWorldCommand(worldIdToRemoveFrom, false))));
            //Fade in camera
            jobChain.Equeue(new EntityJob<AnimStoppedEventArgs>(cameraEntity, () => commandsMan.Post(new PlayAnimCommand(cameraEntity.Id, CameraHelper.CAMERA_FADE_IN, 0))));

            jobMan.Execute(jobChain);
        }

        #endregion Private Methods
    }
}