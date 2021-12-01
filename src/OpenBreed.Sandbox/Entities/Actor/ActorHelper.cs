using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Sandbox.Components;
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
            collisionMan.RegisterFixturePair(ColliderTypes.ActorBody, ColliderTypes.FullObstacle, Dynamic2StaticCallbackEx);
            collisionMan.RegisterFixturePair(ColliderTypes.ActorBody, ColliderTypes.ActorOnlyObstacle, Dynamic2StaticCallbackEx);
        }

        public Entity CreatePlayerActor(Vector2 pos)
        {
            var actor = CreateActor(pos);

            //actor.Add(new InventoryComponent(new Bag[] { new Bag("Backpack") }));
            //actor.Add(new EquipmentComponent(new Slot[] { new Slot("Torso"), new Slot("Hands") }));
            actor.Add(new InventoryComponent(16));

            var p1 = playersMan.GetByName("P1");

            actor.Add(new WalkingInputComponent(p1.Id, 0));
            actor.Add(new AttackInputComponent(p1.Id, 0));
            actor.Add(new WalkingControlComponent());
            actor.Add(new AttackControlComponent());

            return actor;
        }

        public Entity CreateActor(Vector2 pos)
        {
            var actor = entityFactory.Create(@"Defaults\Templates\ABTA\Common\Actors\John.xml")
                .SetParameter("startX", pos.X)
                .SetParameter("startY", pos.Y)
                .Build();

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