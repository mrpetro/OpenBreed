using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
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
            var laserR = core.Animations.Anims.Create<int>("LASER_RIGHT");
            laserR.AddFrame(0, 2.0f);
            var laserRD = core.Animations.Anims.Create<int>("LASER_RIGHT_DOWN");
            laserRD.AddFrame(1, 2.0f);
            var laserD = core.Animations.Anims.Create<int>("LASER_DOWN");
            laserD.AddFrame(2, 2.0f);
            var laserDL = core.Animations.Anims.Create<int>("LASER_DOWN_LEFT");
            laserDL.AddFrame(3, 2.0f);
            var laserL = core.Animations.Anims.Create<int>("LASER_LEFT");
            laserL.AddFrame(4, 2.0f);
            var laserLU = core.Animations.Anims.Create<int>("LASER_LEFT_UP");
            laserLU.AddFrame(5, 2.0f);
            var laserU = core.Animations.Anims.Create<int>("LASER_UP");
            laserU.AddFrame(6, 2.0f);
            var laserUR = core.Animations.Anims.Create<int>("LASER_UP_RIGHT");
            laserUR.AddFrame(7, 2.0f);
        }

        public static void AddProjectile(ICore core, World world, float x, float y, float vx, float vy)
        {
            var projectile = core.Entities.Create();

            projectile.Add(core.Rendering.CreateSprite("Atlases/Sprites/Projectiles/Laser"));
            projectile.Add(Position.Create(x, y ));
            projectile.Add(Velocity.Create(vx, vy));
            projectile.Add(AxisAlignedBoxShape.Create(0, 0, 16, 16));
            projectile.Add(TextHelper.Create(core, new Vector2(-10, 10), "Door"));

            var doorSm = ProjectileHelper.CreateStateMachine(projectile);
            doorSm.SetInitialState("Fired");

            world.AddEntity(projectile);
        }

        public static StateMachine CreateStateMachine(IEntity entity)
        {
            var stateMachine = entity.AddFSM("Attacking");

            stateMachine.AddState(new FiredState("Fired_Right", "LASER_RIGHT", new Vector2(1, 0)));
            stateMachine.AddState(new FiredState("Fired_Right_Down", "LASER_RIGHT_DOWN", new Vector2(1, -1)));
            stateMachine.AddState(new FiredState("Fired_Down", "LASER_DOWN", new Vector2(0, -1)));
            stateMachine.AddState(new FiredState("Fired_Down_Left", "LASER_DOWN_LEFT", new Vector2(-1, -1)));
            stateMachine.AddState(new FiredState("Fired_Left", "LASER_LEFT", new Vector2(-1, 0)));
            stateMachine.AddState(new FiredState("Fired_Left_Up", "LASER_LEFT_UP", new Vector2(-1, 1)));
            stateMachine.AddState(new FiredState("Fired_Up", "LASER_UP", new Vector2(0, 1)));
            stateMachine.AddState(new FiredState("Fired_Up_Right", "LASER_UP_RIGHT", new Vector2(1, 1)));

            return stateMachine;
        }
    }
}
