using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Helpers;
using OpenBreed.Core.Modules.Rendering.Commands;
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
            var laserR = core.Animations.Create<int>("Animations/Laser/Fired/Right", OnFrameUpdate);
            laserR.AddFrame(0, 2.0f);
            var laserRD = core.Animations.Create<int>("Animations/Laser/Fired/RightDown", OnFrameUpdate);
            laserRD.AddFrame(1, 2.0f);
            var laserD = core.Animations.Create<int>("Animations/Laser/Fired/Down", OnFrameUpdate);
            laserD.AddFrame(2, 2.0f);
            var laserDL = core.Animations.Create<int>("Animations/Laser/Fired/DownLeft", OnFrameUpdate);
            laserDL.AddFrame(3, 2.0f);
            var laserL = core.Animations.Create<int>("Animations/Laser/Fired/Left", OnFrameUpdate);
            laserL.AddFrame(4, 2.0f);
            var laserLU = core.Animations.Create<int>("Animations/Laser/Fired/LeftUp", OnFrameUpdate);
            laserLU.AddFrame(5, 2.0f);
            var laserU = core.Animations.Create<int>("Animations/Laser/Fired/Up", OnFrameUpdate);
            laserU.AddFrame(6, 2.0f);
            var laserUR = core.Animations.Create<int>("Animations/Laser/Fired/UpRight", OnFrameUpdate);
            laserUR.AddFrame(7, 2.0f);
        }

        private static void OnFrameUpdate(IEntity entity, int nextValue)
        {
            entity.PostCommand(new SpriteSetCommand(entity.Id, nextValue));
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
            //projectile.Add(new FsmComponent());

            projectile.GetComponent<PositionComponent>().Value = new Vector2(x, y);
            projectile.GetComponent<VelocityComponent>().Value = new Vector2(vx, vy);

            projectile.Subscribe<CollisionEventArgs>(OnCollision);

            //var projectileFsm = core.StateMachines.GetByName("Projectile");
            //projectileFsm.SetInitialState(projectile, (int)AttackingState.Fired);
            world.PostCommand(new AddEntityCommand(world.Id, projectile.Id));
            //world.AddEntity(projectile);

        }

        public static void CreateFsm(ICore core)
        {
            var stateMachine = core.StateMachines.Create<AttackingState, AttackingImpulse>("Projectile");
            stateMachine.AddState(new FiredState("Animations/Laser/Fired/"));
        }
    }
}
