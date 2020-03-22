using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Entities.Projectile.States;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Projectile
{
    public static class ProjectileHelper
    {
        public static void CreateAnimations(ICore core)
        {
            var laserR = core.Animations.Anims.Create<int>("Animations/Laser/Fired/Right");
            laserR.AddFrame(0, 2.0f);
            var laserRD = core.Animations.Anims.Create<int>("Animations/Laser/Fired/RightDown");
            laserRD.AddFrame(1, 2.0f);
            var laserD = core.Animations.Anims.Create<int>("Animations/Laser/Fired/Down");
            laserD.AddFrame(2, 2.0f);
            var laserDL = core.Animations.Anims.Create<int>("Animations/Laser/Fired/DownLeft");
            laserDL.AddFrame(3, 2.0f);
            var laserL = core.Animations.Anims.Create<int>("Animations/Laser/Fired/Left");
            laserL.AddFrame(4, 2.0f);
            var laserLU = core.Animations.Anims.Create<int>("Animations/Laser/Fired/LeftUp");
            laserLU.AddFrame(5, 2.0f);
            var laserU = core.Animations.Anims.Create<int>("Animations/Laser/Fired/Up");
            laserU.AddFrame(6, 2.0f);
            var laserUR = core.Animations.Anims.Create<int>("Animations/Laser/Fired/UpRight");
            laserUR.AddFrame(7, 2.0f);
        }

        private static void OnCollision(object sender, CollisionEventArgs args)
        {
            var entity = (IEntity)sender;
            var body = args.Entity.TryGetComponent<BodyComponent>();

            var type = body.Tag;

            switch (type)
            {
                case "Solid":
                    DynamicHelper.ResolveVsStatic(entity, args.Entity, args.Projection);
                    return;
                default:
                    break;
            }
        }

        public static void AddProjectile(ICore core, World world, float x, float y, float vx, float vy)
        {
            var projectile = core.Entities.CreateFromTemplate("Projectile");

            projectile.GetComponent<PositionComponent>().Value = new Vector2(x, y);
            projectile.GetComponent<VelocityComponent>().Value = new Vector2(vx, vy);

            projectile.Subscribe<CollisionEventArgs>(OnCollision);

            world.AddEntity(projectile);

            var doorSm = ProjectileHelper.CreateStateMachine(projectile);
            doorSm.SetInitialState(AttackingState.Fired);
        }

        public static StateMachine<AttackingState> CreateStateMachine(IEntity entity)
        {
            var stateMachine = entity.AddFsm<AttackingState>();

            stateMachine.AddState(new FiredState("Animations/Laser/Fired/"));

            return stateMachine;
        }
    }
}
