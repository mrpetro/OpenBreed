﻿using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Wecs.Worlds;
using OpenTK;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public class ActorHelper
    {
        #region Private Fields

        private readonly IClipMan clipMan;

        private readonly ICollisionMan collisionMan;

        private readonly IPlayersMan playersMan;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;

        private readonly DynamicResolver dynamicResolver;
        private readonly FixtureTypes fixtureTypes;

        #endregion Private Fields

        #region Public Constructors

        public ActorHelper(IClipMan clipMan, ICollisionMan collisionMan, IPlayersMan playersMan, IDataLoaderFactory dataLoaderFactory, IEntityFactory entityFactory, DynamicResolver dynamicResolver, FixtureTypes fixtureTypes)
        {
            this.clipMan = clipMan;
            this.collisionMan = collisionMan;
            this.playersMan = playersMan;
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
            this.dynamicResolver = dynamicResolver;
            this.fixtureTypes = fixtureTypes;
        }

        #endregion Public Constructors

        #region Public Methods

        public void RegisterCollisionPairs()
        {
            //collisionMan.RegisterCollisionPair(ColliderTypes.ActorBody, ColliderTypes.StaticObstacle, Dynamic2StaticCallback);

            collisionMan.RegisterFixturePair(ColliderTypes.ActorBody, ColliderTypes.StaticObstacle, Dynamic2StaticCallbackEx);
        }

        public void CreateAnimations()
        {
            var animationLoader = dataLoaderFactory.GetLoader<IClip>();

            animationLoader.Load("Animations/Actor/Standing/Up");
            animationLoader.Load("Animations/Actor/Standing/UpRight");
            animationLoader.Load("Animations/Actor/Standing/Right");
            animationLoader.Load("Animations/Actor/Standing/RightDown");
            animationLoader.Load("Animations/Actor/Standing/Down");
            animationLoader.Load("Animations/Actor/Standing/DownLeft");
            animationLoader.Load("Animations/Actor/Standing/Left");
            animationLoader.Load("Animations/Actor/Standing/LeftUp");

            animationLoader.Load("Animations/Actor/Walking/Up");
            animationLoader.Load("Animations/Actor/Walking/UpRight");
            animationLoader.Load("Animations/Actor/Walking/Right");
            animationLoader.Load("Animations/Actor/Walking/RightDown");
            animationLoader.Load("Animations/Actor/Walking/Down");
            animationLoader.Load("Animations/Actor/Walking/DownLeft");
            animationLoader.Load("Animations/Actor/Walking/Left");
            animationLoader.Load("Animations/Actor/Walking/LeftUp");
        }

        public Entity CreatePlayerActor(Vector2 pos)
        {
            var actor = CreateActor(pos);

            var p1 = playersMan.GetByName("P1");

            actor.Add(new WalkingInputComponent(p1.Id, 0));
            actor.Add(new AttackInputComponent(p1.Id, 0));
            actor.Add(new WalkingControlComponent());
            actor.Add(new AttackControlComponent());

            return actor;
        }

        public Entity CreateActor(Vector2 pos)
        {
            var actor = entityFactory.Create(@"Entities\Actor\John.xml")
                .SetParameter("startX", pos.X)
                .SetParameter("startY", pos.Y)
                .Build();

            actor.Add(new AngularVelocityComponent(0));
            actor.Add(new AngularThrustComponent(0));
            //actor.Add(new InventoryComponent(new Bag[] { new Bag("Backpack") }));
            //actor.Add(new EquipmentComponent(new Slot[] { new Slot("Torso"), new Slot("Hands") }));
            //actor.Add(AxisAlignedBoxShape.Create(0, 0, 32, 32));
            actor.Add(new FollowerComponent());

            return actor;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void AddHero(World world, int ix, int iy)
        {
            var playerActor = CreatePlayerActor(new Vector2(16 * ix, 16 * iy));

            playerActor.Tag = "John";

            playerActor.EnterWorld(world.Id);
        }

        #endregion Internal Methods

        #region Private Methods

        private void Dynamic2StaticCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection)
        {
            dynamicResolver.ResolveVsStatic(entityA, entityB, projection);
        }

        private void Dynamic2StaticCallbackEx(BodyFixture fixtureA, Entity entityA, BodyFixture fixtureB, Entity entityB, Vector2 projection)
        {
            dynamicResolver.ResolveVsStatic(entityA, entityB, projection);
        }

        #endregion Private Methods

        //private static void OnCollision(object sender, CollisionEventArgs args)
        //{
        //    var entity = (Entity)sender;
        //    var body = args.Entity.TryGet<BodyComponent>();

        //    var type = body.Tag;

        //    switch (type)
        //    {
        //        case "Static":
        //            DynamicHelper.ResolveVsStatic(entity, args.Entity, args.Projection);
        //            return;

        //        default:
        //            break;
        //    }
        //}
    }
}