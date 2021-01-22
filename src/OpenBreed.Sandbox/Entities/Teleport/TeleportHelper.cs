using OpenBreed.Core;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Jobs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Systems.Rendering.Commands;
using OpenBreed.Core.Events;
using OpenBreed.Core.Commands;
using OpenBreed.Components.Common;
using OpenBreed.Sandbox.Entities.WorldGate;
using OpenBreed.Common.Tools;
using OpenBreed.Physics.Generic;
using OpenBreed.Components.Physics;
using OpenBreed.Animation.Interface;
using OpenBreed.Ecsw;
using OpenBreed.Ecsw.Entities.Xml;
using OpenBreed.Ecsw.Entities;

namespace OpenBreed.Sandbox.Entities.Teleport
{
    public struct TeleportPair : IEquatable<TeleportPair>
    {
        public int Id;

        public bool Equals(TeleportPair other)
        {
            return Id == other.Id;
        }
    }

    public static class TeleportHelper
    {
        #region Public Fields

        public const string SPRITE_TELEPORT_ENTRY = "Atlases/Sprites/Teleport/Entry";
        public const string SPRITE_TELEPORT_EXIT = "Atlases/Sprites/Teleport/Exit";

        #endregion Public Fields

        #region Private Fields

        private const string ANIMATION_TELEPORT_ENTRY = "Animations/Teleport/Entry";
        private const string ANIMATION_TELEPORT_EXIT = "Animations/Teleport/Exit";

        #endregion Private Fields

        #region Public Methods

        public static void CreateAnimations(ICore core)
        {
            var animations = core.GetManager<IAnimMan>();

            var animationTeleportEntry = animations.Create(ANIMATION_TELEPORT_ENTRY, 4.0f);
            var te = animationTeleportEntry.AddPart<int>(OnFrameUpdate, 0);
            te.AddFrame(0, 1.0f);
            te.AddFrame(1, 2.0f);
            te.AddFrame(2, 3.0f);
            te.AddFrame(3, 4.0f);

        }

        private static void OnFrameUpdate(Entity entity, int nextValue)
        {
            entity.Core.Commands.Post(new SpriteSetCommand(entity.Id, nextValue));
        }

        public static Entity AddTeleportEntry(World world, int x, int y, int pairId)
        {
            var core = world.Core;

            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Teleport\TeleportEntry.xml");
            var teleportEntry = world.Core.GetManager<IEntityFactory>().Create(entityTemplate);

            teleportEntry.Tag = new TeleportPair { Id = pairId };

            teleportEntry.Get<PositionComponent>().Value = new Vector2( 16 * x, 16 * y);
            teleportEntry.Add(new CollisionComponent(ColliderTypes.TeleportEntryTrigger));
            world.Core.Commands.Post(new AddEntityCommand(world.Id, teleportEntry.Id));
            return teleportEntry;
        }

        //public static void RegisterCollisionPairs(ICore core)
        //{
        //    var collisionMan = core.GetModule<PhysicsModule>().Collisions;

        //    collisionMan.RegisterCollisionPair(ColliderTypes.ActorBody, ColliderTypes.TeleportEntryTrigger, Actor2TriggerCallback);
        //    //collisionMan.RegisterCollisionPair(ColliderTypes.WorldExitTrigger, ColliderTypes.ActorBody, Actor2TriggerCallback);
        //}

        //private static void Actor2TriggerCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection)
        //{
        //    if (colliderTypeA == ColliderTypes.WorldExitTrigger && colliderTypeB == ColliderTypes.ActorBody)
        //        PerformEntityExit(entityB, entityA);
        //    else if (colliderTypeA == ColliderTypes.ActorBody && colliderTypeB == ColliderTypes.WorldExitTrigger)
        //        PerformEntityExit(entityA, entityB);
        //}


        public static Entity AddTeleportExit(World world, int x, int y, int pairId)
        {
            var core = world.Core;

            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Teleport\TeleportExit.xml");
            var teleportExit = world.Core.GetManager<IEntityFactory>().Create(entityTemplate);
            //var teleportExit = core.GetManager<IEntityMan>().CreateFromTemplate("TeleportExit");

            teleportExit.Tag = new TeleportPair { Id = pairId };

            teleportExit.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);

            //teleportExit.Subscribe<AnimChangedEventArgs>(OnFrameChanged);

            world.Core.Commands.Post(new AddEntityCommand(world.Id, teleportExit.Id));
            //world.AddEntity(teleportExit);

            return teleportExit;
        }

        #endregion Public Methods

        #region Private Methods

        public static void SetPosition(Entity target, Entity entryEntity, bool cancelMovement)
        {
            var pair = (TeleportPair)entryEntity.Tag;
            var exitEntity = target.Core.GetManager<IEntityMan>().GetByTag(pair).FirstOrDefault(item => item != entryEntity);

            if (exitEntity == null)
                throw new Exception("No exit entity found");

            var exitPos = exitEntity.Get<PositionComponent>();
            var targetPos = target.Get<PositionComponent>();
            var targetAabb = target.Get<BodyComponent>().Aabb;
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

        }

        #endregion Private Methods
    }
}