using OpenBreed.Common.Tools;
using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Physics.Generic.Helpers;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Sandbox.Entities.Projectile.States;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Physics.Interface;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Animation.Interface;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Entities;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Commands;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Core.Managers;

namespace OpenBreed.Sandbox.Entities.Projectile
{
    public class ProjectileHelper
    {
        public ProjectileHelper(ICore core)
        {
            this.core = core;
        }

        public void CreateAnimations()
        {
            var animations = core.GetManager<IClipMan>();

            var laserR = animations.CreateClip("Animations/Laser/Fired/Right", 2.0f);
            laserR.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 0).AddFrame(0, 2.0f);
            var laserRD = animations.CreateClip("Animations/Laser/Fired/RightDown", 2.0f);
            laserRD.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 1).AddFrame(1, 2.0f);
            var laserD = animations.CreateClip("Animations/Laser/Fired/Down", 2.0f);
            laserD.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 2).AddFrame(2, 2.0f);
            var laserDL = animations.CreateClip("Animations/Laser/Fired/DownLeft", 2.0f);
            laserDL.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 3).AddFrame(3, 2.0f);
            var laserL = animations.CreateClip("Animations/Laser/Fired/Left", 2.0f);
            laserL.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 4).AddFrame(4, 2.0f);
            var laserLU = animations.CreateClip("Animations/Laser/Fired/LeftUp", 2.0f);
            laserLU.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 5).AddFrame(5, 2.0f);
            var laserU = animations.CreateClip("Animations/Laser/Fired/Up", 2.0f);
            laserU.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 6).AddFrame(6, 2.0f);
            var laserUR = animations.CreateClip("Animations/Laser/Fired/UpRight", 2.0f);
            laserUR.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 7).AddFrame(7, 2.0f);
        }

        private void OnFrameUpdate(Entity entity, int nextValue)
        {
            core.Commands.Post(new SpriteSetCommand(entity.Id, nextValue));
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

        public void RegisterCollisionPairs()
        {
            var collisionMan = core.GetManager<ICollisionMan>();

            collisionMan.RegisterCollisionPair(ColliderTypes.Projectile, ColliderTypes.StaticObstacle, Projectile2StaticObstacle);
        }

        private void Projectile2StaticObstacle(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection)
        {
            DynamicHelper.ResolveVsStatic(entityA, entityB, projection);
        }

        static IEntityTemplate projectileTemplate;
        private readonly ICore core;

        public void AddProjectile(int worldId, float x, float y, float vx, float vy)
        {
            if(projectileTemplate == null)
                projectileTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Projectile\Projectile.xml");

            var projectile = core.GetManager<IEntityFactory>().Create(projectileTemplate);
            //var projectile = core.GetManager<IEntityMan>().CreateFromTemplate("Projectile");

            //projectile.Add(new FsmComponent());

            projectile.Get<PositionComponent>().Value = new Vector2(x, y);
            projectile.Get<VelocityComponent>().Value = new Vector2(vx, vy);
            projectile.Add(new ColliderComponent(ColliderTypes.Projectile));

            //var projectileFsm = core.GetManager<IFsmMan>().GetByName("Projectile");
            //projectileFsm.SetInitialState(projectile, (int)AttackingState.Fired);
            core.Commands.Post(new AddEntityCommand(worldId, projectile.Id));
            //world.AddEntity(projectile);

        }
    }
}
