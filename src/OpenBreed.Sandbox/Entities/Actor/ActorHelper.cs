using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Input.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public class ActorHelper
    {
        #region Private Fields

        private readonly IClipMan<IEntity> clipMan;

        private readonly ICollisionMan<IEntity> collisionMan;
        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;

        private readonly DynamicResolver dynamicResolver;
        private readonly FixtureTypes fixtureTypes;
        private readonly IScriptMan scriptMan;
        private readonly IFsmMan fsmMan;
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public ActorHelper(
            IClipMan<IEntity> clipMan,           
            ICollisionMan<IEntity> collisionMan,
            IEntityMan entityMan,
            IWorldMan worldMan,
            IDataLoaderFactory dataLoaderFactory,
            IEntityFactory entityFactory,
            DynamicResolver dynamicResolver,
            FixtureTypes fixtureTypes,
            IScriptMan scriptMan,
            IFsmMan fsmMan,
            ITriggerMan triggerMan)
        {
            this.clipMan = clipMan;
            this.collisionMan = collisionMan;
            this.entityMan = entityMan;
            this.worldMan = worldMan;
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
            this.dynamicResolver = dynamicResolver;
            this.fixtureTypes = fixtureTypes;
            this.scriptMan = scriptMan;
            this.fsmMan = fsmMan;
            this.triggerMan = triggerMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void RegisterCollisionPairs()
        {
            collisionMan.RegisterFixturePair(ColliderTypes.ActorBody, ColliderTypes.FullObstacle, FullObstableCallback);
            collisionMan.RegisterFixturePair(ColliderTypes.ActorBody, ColliderTypes.ActorOnlyObstacle, FullObstableCallback);

            collisionMan.RegisterFixturePair(ColliderTypes.ActorBody, ColliderTypes.SlopeObstacle, SlopeObstacleCallback);
            collisionMan.RegisterFixturePair(ColliderTypes.ActorBody, ColliderTypes.SlowdownObstacle, SlowdownObstacleCallback);
            
            collisionMan.RegisterFixturePair(ColliderTypes.Projectile, ColliderTypes.FullObstacle, ProjectileTriggerCallback);

            collisionMan.RegisterFixturePair(ColliderTypes.ActorBody, ColliderTypes.Trigger, TriggerCallback);
        }

        public IEntity CreateMission(string name)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\Mission")
                .SetTag(name)
                .Build();

            return entity;
        }

        public IEntity CreateDummyActor(string name, Vector2 pos)
        {
            var actor = CreateDummy(name, pos);
            actor.CreateTimer("CooldownDelay");

            //actor.Add(new InventoryComponent(new Bag[] { new Bag("Backpack") }));
            //actor.Add(new EquipmentComponent(new Slot[] { new Slot("Torso"), new Slot("Hands") }));
            actor.Add(new InventoryComponent(16));

            var p1Controller = entityMan.GetByTag("Controllers.P1").First();

            p1Controller.Get<ControllerComponent>().ControlledEntityId = actor.Id;

            return actor;
        }

        public IEntity CreatePlayerActor(string name, Vector2 pos)
        {
            var actor = CreateActor(name, pos);

            var p1Controller = entityMan.GetByTag("Controllers.P1").First();
            p1Controller.Get<ControllerComponent>().ControlledEntityId = actor.Id;


            //actor.Add(new InventoryComponent(new Bag[] { new Bag("Backpack") }));
            //actor.Add(new EquipmentComponent(new Slot[] { new Slot("Torso"), new Slot("Hands") }));
            actor.Add(new InventoryComponent(16));

            //var p1 = playersMan.GetByName("P1");

            //actor.Add(new WalkingInputComponent(p1.Id, 0));
            //actor.Add(new AttackInputComponent(p1.Id, 0));
            //actor.Add(new WalkingControlComponent());
            //actor.Add(new AttackControlComponent());

            return actor;
        }

        public IEntity CreateActor(string name, Vector2 pos)
        {
            var actor = entityFactory.Create(@"ABTA\Templates\Common\Actors\John")
                .SetParameter("startX", pos.X)
                .SetParameter("startY", pos.Y)
                .SetTag(name)
                .Build();

            return actor;
        }

        public IEntity CreateDummy(string name, Vector2 pos)
        {
            var actor = entityFactory.Create(@"ABTA\Templates\Common\Actors\Dummy")
                .SetParameter("startX", pos.X)
                .SetParameter("startY", pos.Y)
                .SetTag(name)
                .Build();

            return actor;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void AddHero(IWorld world, int ix, int iy)
        {
            var playerActor = CreatePlayerActor("John", new Vector2(16 * ix, 16 * iy));

            worldMan.RequestAddEntity(playerActor, world.Id);
        }

        #endregion Internal Methods

        #region Private Methods

        private void Dynamic2StaticCallback(int colliderTypeA, IEntity entityA, int colliderTypeB, IEntity entityB, float dt, Vector2 projection)
        {
            dynamicResolver.ResolveVsStatic(entityA, entityB, dt, projection);
        }

        private void FullObstableCallback(BodyFixture fixtureA, IEntity entityA, BodyFixture fixtureB, IEntity entityB, float dt, Vector2 projection)
        {
            dynamicResolver.ResolveVsStatic(entityA, entityB, dt, projection);
        }

        private void TryOnCollision(IEntity entityA, IEntity entityB, Vector2 projection)
        {
            var functionId = entityA.GetFunctionId("OnCollision");

            if (functionId is null)
                return;

            var scriptFunction = scriptMan.GetFunction(functionId);

            if (scriptFunction is null)
                return;

            scriptFunction.Invoke(entityA, entityB, projection);
        }

        private void ProjectileTriggerCallback(BodyFixture fixtureA, IEntity entityA, BodyFixture fixtureB, IEntity entityB, float dt, Vector2 projection)
        {
            TryOnCollision(entityA, entityB, projection);
        }

        private void TriggerCallback(BodyFixture fixtureA, IEntity entityA, BodyFixture fixtureB, IEntity entityB, float dt, Vector2 projection)
        {
            TryOnCollision(entityA, entityB, projection);
            TryOnCollision(entityB, entityA, projection);
        }

        private void SlopeObstacleCallback(BodyFixture fixtureA, IEntity entityA, BodyFixture fixtureB, IEntity entityB, float dt, Vector2 projection)
        {
            var metadata = entityB.Get<MetadataComponent>();

            Vector2 slopeDirection;

            switch (metadata.Flavor)
            {
                case "DownLeft":
                    slopeDirection = new Vector2(0, 1);
                    break;
                case "UpLeft":
                    slopeDirection = new Vector2(0, -1);
                    break;
                case "UpRight":
                    slopeDirection = new Vector2(0, -1);
                    break;
                case "DownRight":
                    slopeDirection = new Vector2(0, 1);
                    break;
                default:
                    slopeDirection = new Vector2(0, 0);
                    break;
            }

            dynamicResolver.ResolveVsSlope(entityA, entityB, projection, slopeDirection);
        }

        private void SlowdownObstacleCallback(BodyFixture fixtureA, IEntity entityA, BodyFixture fixtureB, IEntity entityB, float dt, Vector2 projection)
        {
            //if (entityA.State is "Slowdown")
            //    return;

            var velCmp = entityA.Get<VelocityComponent>();

            velCmp.Value = Vector2.Multiply(velCmp.Value, 0.5f);

            //entityA.State = "Slowdown";

            Console.WriteLine("Slowdown");
        }

        #endregion Private Methods
    }
}