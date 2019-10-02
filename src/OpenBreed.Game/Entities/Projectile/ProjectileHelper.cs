using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Game.Entities.Projectile.States;
using OpenBreed.Game.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities.Projectile
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

        private static void OnCollision(IEntity thisEntity, IEntity otherEntity, Vector2 projection)
        {
            thisEntity.RaiseEvent(new CollisionEvent(otherEntity));

            var body = otherEntity.Components.OfType<IBody>().FirstOrDefault();

            var type = body.Tag;

            switch (type)
            {
                case "Solid":
                    DynamicHelper.ResolveVsStatic(thisEntity, otherEntity, projection);
                    return;
                default:
                    break;
            }
        }

        public static void AddProjectile(ICore core, World world, float x, float y, float vx, float vy)
        {

            var projectile = core.Entities.Create();

            projectile.Add(core.Rendering.CreateSprite("Atlases/Sprites/Projectiles/Laser"));
            projectile.Add(new Animator(10.0f, true));
            projectile.Add(Position.Create(x, y ));
            projectile.Add(Thrust.Create(0, 0));
            projectile.Add(Body.Create(0, 1, "Dynamic", (e, c) => OnCollision(projectile, e, c)));
            projectile.Add(Velocity.Create(vx, vy));
            projectile.Add(AxisAlignedBoxShape.Create(0, 0, 16, 16));
            projectile.Add(TextHelper.Create(core, new Vector2(-10, 10), "Bullet"));
            world.AddEntity(projectile);

            var doorSm = ProjectileHelper.CreateStateMachine(projectile);
            doorSm.SetInitialState("Fired");
        }

        public static StateMachine CreateStateMachine(IEntity entity)
        {
            var stateMachine = entity.AddFSM("Attacking");

            stateMachine.AddState(new FiredState("Fired", "Animations/Laser/Fired/"));

            return stateMachine;
        }
    }
}
