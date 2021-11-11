using OpenBreed.Animation.Interface;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenTK;

namespace OpenBreed.Sandbox.Entities.Projectile
{
    public class ProjectileHelper
    {
        #region Private Fields

        private IEntityTemplate projectileTemplate;

        private readonly IClipMan clipMan;

        private readonly ICollisionMan collisionMan;

        private readonly IEntityFactory entityFactory;

        private readonly DynamicResolver dynamicResolver;

        #endregion Private Fields

        #region Public Constructors

        public ProjectileHelper(IClipMan clipMan, ICollisionMan collisionMan, IEntityFactory entityFactory, DynamicResolver dynamicResolver)
        {
            this.clipMan = clipMan;
            this.collisionMan = collisionMan;
            this.entityFactory = entityFactory;
            this.dynamicResolver = dynamicResolver;
        }

        #endregion Public Constructors

        #region Public Methods

        public void CreateAnimations()
        {
            var laserR = clipMan.CreateClip("Animations/Laser/Fired/Right", 2.0f);
            laserR.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 0).AddFrame(0, 2.0f);
            var laserRD = clipMan.CreateClip("Animations/Laser/Fired/RightDown", 2.0f);
            laserRD.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 1).AddFrame(1, 2.0f);
            var laserD = clipMan.CreateClip("Animations/Laser/Fired/Down", 2.0f);
            laserD.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 2).AddFrame(2, 2.0f);
            var laserDL = clipMan.CreateClip("Animations/Laser/Fired/DownLeft", 2.0f);
            laserDL.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 3).AddFrame(3, 2.0f);
            var laserL = clipMan.CreateClip("Animations/Laser/Fired/Left", 2.0f);
            laserL.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 4).AddFrame(4, 2.0f);
            var laserLU = clipMan.CreateClip("Animations/Laser/Fired/LeftUp", 2.0f);
            laserLU.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 5).AddFrame(5, 2.0f);
            var laserU = clipMan.CreateClip("Animations/Laser/Fired/Up", 2.0f);
            laserU.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 6).AddFrame(6, 2.0f);
            var laserUR = clipMan.CreateClip("Animations/Laser/Fired/UpRight", 2.0f);
            laserUR.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 7).AddFrame(7, 2.0f);
        }

        public void RegisterCollisionPairs()
        {
            //collisionMan.RegisterCollisionPair(ColliderTypes.Projectile, ColliderTypes.StaticObstacle, Projectile2StaticObstacle);
            collisionMan.RegisterFixturePair(ColliderTypes.Projectile, ColliderTypes.StaticObstacle, Projectile2StaticObstacleEx);
        }

        public void AddProjectile(int worldId, float x, float y, float vx, float vy)
        {
            if (projectileTemplate == null)
                projectileTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Projectile\Projectile.xml");

            var projectile = entityFactory.Create(projectileTemplate);
            //var projectile = core.GetManager<IEntityMan>().CreateFromTemplate("Projectile");

            //projectile.Add(new FsmComponent());

            projectile.Get<PositionComponent>().Value = new Vector2(x, y);
            projectile.Get<VelocityComponent>().Value = new Vector2(vx, vy);

            //var projectileFsm = core.GetManager<IFsmMan>().GetByName("Projectile");
            //projectileFsm.SetInitialState(projectile, (int)AttackingState.Fired);
            projectile.EnterWorld(worldId);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnFrameUpdate(Entity entity, int nextValue)
        {
            entity.SetSpriteImageId(nextValue);
        }

        //private static void OnCollision(object sender, CollisionEventArgs args)
        //{
        //    //var entity = (Entity)sender;
        //    //var body = args.Entity.TryGet<BodyComponent>();

        //    //var type = body.Tag;

        //    //switch (type)
        //    //{
        //    //    case "Solid":
        //    //        DynamicHelper.ResolveVsStatic(entity, args.Entity, args.Projection);
        //    //        return;
        //    //    default:
        //    //        break;
        //    //}
        //}

        private void Projectile2StaticObstacle(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection)
        {
            dynamicResolver.ResolveVsStatic(entityA, entityB, projection);
        }

        private void Projectile2StaticObstacleEx(BodyFixture colliderTypeA, Entity entityA, BodyFixture colliderTypeB, Entity entityB, Vector2 projection)
        {
            dynamicResolver.ResolveVsStatic(entityA, entityB, projection);
        }

        #endregion Private Methods
    }
}