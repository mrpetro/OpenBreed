using OpenBreed.Common.Tools;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Physics.Generic;
using OpenBreed.Physics.Generic.Helpers;
using OpenBreed.Physics.Interface;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenTK;
using OpenBreed.Animation.Interface;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Input.Generic;
using OpenBreed.Input.Interface;
using OpenBreed.Common;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public  class ActorHelper
    {
        public ActorHelper(ICore core)
        {
            this.core = core;
        }

        #region Public Fields

        public const string SPRITE_ARROW = "Atlases/Sprites/Arrow";
        private readonly ICore core;

        #endregion Public Fields

        #region Public Methods

        public void RegisterCollisionPairs()
        {
            var collisionMan = core.GetManager<ICollisionMan>();

            collisionMan.RegisterCollisionPair(ColliderTypes.ActorBody, ColliderTypes.StaticObstacle, Dynamic2StaticCallback);
        }

        public void CreateAnimations()
        {
            var animations = core.GetManager<IClipMan>();
            var dataLoaderFactory = core.GetManager<IDataLoaderFactory>();
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
            var playersMan = core.GetManager<IPlayersMan>();

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
            //var actor = core.GetManager<IEntityMan>().Create();

            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Actor\Arrow.xml");
            var actor = core.GetManager<IEntityFactory>().Create(entityTemplate);
            //var actor = core.GetManager<IEntityMan>().CreateFromTemplate("Arrow");


            actor.Add(new AngularVelocityComponent(0));
            actor.Add(new AngularThrustComponent(0));
            actor.Add(new CollisionComponent(ColliderTypes.ActorBody));
            //actor.Add(new InventoryComponent(new Bag[] { new Bag("Backpack") }));
            //actor.Add(new EquipmentComponent(new Slot[] { new Slot("Torso"), new Slot("Hands") }));
            //actor.Add(AxisAlignedBoxShape.Create(0, 0, 32, 32));
            actor.Add(new FollowerComponent());
            actor.Get<PositionComponent>().Value = pos;

            //actor.Subscribe<CollisionEventArgs>(OnCollision);

            return actor;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnFrameUpdate(Entity entity, int nextValue)
        {
            core.Commands.Post(new SpriteSetCommand(entity.Id, nextValue));
        }

        private void Dynamic2StaticCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection)
        {
            DynamicHelper.ResolveVsStatic(entityA, entityB, projection);
        }

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

        #endregion Private Methods
    }
}