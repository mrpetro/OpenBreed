using OpenBreed.Common.Tools;
using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Modules;
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

namespace OpenBreed.Sandbox.Entities.Projectile
{
    public static class ProjectileHelper
    {
        public static void CreateAnimations(ICore core)
        {
            var animations = core.GetManager<IAnimMan>();

            var laserR = animations.Create("Animations/Laser/Fired/Right", 2.0f);
            laserR.AddPart<int>(OnFrameUpdate, 0).AddFrame(0, 2.0f);
            var laserRD = animations.Create("Animations/Laser/Fired/RightDown", 2.0f);
            laserRD.AddPart<int>(OnFrameUpdate, 1).AddFrame(1, 2.0f);
            var laserD = animations.Create("Animations/Laser/Fired/Down", 2.0f);
            laserD.AddPart<int>(OnFrameUpdate, 2).AddFrame(2, 2.0f);
            var laserDL = animations.Create("Animations/Laser/Fired/DownLeft", 2.0f);
            laserDL.AddPart<int>(OnFrameUpdate, 3).AddFrame(3, 2.0f);
            var laserL = animations.Create("Animations/Laser/Fired/Left", 2.0f);
            laserL.AddPart<int>(OnFrameUpdate, 4).AddFrame(4, 2.0f);
            var laserLU = animations.Create("Animations/Laser/Fired/LeftUp", 2.0f);
            laserLU.AddPart<int>(OnFrameUpdate, 5).AddFrame(5, 2.0f);
            var laserU = animations.Create("Animations/Laser/Fired/Up", 2.0f);
            laserU.AddPart<int>(OnFrameUpdate, 6).AddFrame(6, 2.0f);
            var laserUR = animations.Create("Animations/Laser/Fired/UpRight", 2.0f);
            laserUR.AddPart<int>(OnFrameUpdate, 7).AddFrame(7, 2.0f);
        }

        private static void OnFrameUpdate(Entity entity, int nextValue)
        {
            entity.Core.Commands.Post(new SpriteSetCommand(entity.Id, nextValue));
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

        public static void RegisterCollisionPairs(ICore core)
        {
            var collisionMan = core.GetModule<IPhysicsModule>().Collisions;

            collisionMan.RegisterCollisionPair(ColliderTypes.Projectile, ColliderTypes.StaticObstacle, Projectile2StaticObstacle);
        }

        private static void Projectile2StaticObstacle(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection)
        {
            DynamicHelper.ResolveVsStatic(entityA, entityB, projection);
        }

        static IEntityTemplate projectileTemplate;

        public static void AddProjectile(ICore core, World world, float x, float y, float vx, float vy)
        {
            if(projectileTemplate == null)
                projectileTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Projectile\Projectile.xml");

            var projectile = core.GetManager<IEntityFactory>().Create(projectileTemplate);
            //var projectile = core.GetManager<IEntityMan>().CreateFromTemplate("Projectile");

            //projectile.Add(new FsmComponent());

            projectile.Get<PositionComponent>().Value = new Vector2(x, y);
            projectile.Get<VelocityComponent>().Value = new Vector2(vx, vy);
            projectile.Add(new CollisionComponent(ColliderTypes.Projectile));

            //var projectileFsm = core.GetManager<IFsmMan>().GetByName("Projectile");
            //projectileFsm.SetInitialState(projectile, (int)AttackingState.Fired);
            world.Core.Commands.Post(new AddEntityCommand(world.Id, projectile.Id));
            //world.AddEntity(projectile);

        }

        public static void CreateFsm(ICore core)
        {
            var stateMachine = core.GetManager<IFsmMan>().Create<AttackingState, AttackingImpulse>("Projectile");
            stateMachine.AddState(new FiredState("Animations/Laser/Fired/"));
        }
    }
}
