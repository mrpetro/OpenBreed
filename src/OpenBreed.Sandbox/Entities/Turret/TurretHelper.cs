using OpenBreed.Core;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Sandbox.Entities.Actor.States.Rotation;
using OpenTK;
using System;

namespace OpenBreed.Sandbox.Entities.Turret
{
    public static class TurretHelper
    {
        #region Public Fields

        public const string SPRITE_TURRET = "Atlases/Sprites/Turret";

        public static void CreateAnimations(ICore core)
        {
            var animationGuarding0 = core.Animations.Create("Animations/Turret/Guarding/0", 2.0f);
            animationGuarding0.AddPart<int>(OnFrameUpdate, 0).AddFrame(0, 2.0f);
            var animationGuarding22_5 = core.Animations.Create("Animations/Guarding/Guard/22.5", 2.0f);
            animationGuarding22_5.AddPart<int>(OnFrameUpdate, 1).AddFrame(1, 2.0f);
            var animationGuarding45 = core.Animations.Create("Animations/Turret/Guarding/45", 2.0f);
            animationGuarding45.AddPart<int>(OnFrameUpdate, 2).AddFrame(2, 2.0f);
            var animationGuarding67_5 = core.Animations.Create("Animations/Turret/Guarding/67.5", 2.0f);
            animationGuarding67_5.AddPart<int>(OnFrameUpdate, 3).AddFrame(3, 2.0f);
            var animationGuarding90 = core.Animations.Create("Animations/Guarding/Guard/90", 2.0f);
            animationGuarding90.AddPart<int>(OnFrameUpdate, 4).AddFrame(4, 2.0f);
            var animationGuarding112_5 = core.Animations.Create("Animations/Turret/Guarding/112.5", 2.0f);
            animationGuarding112_5.AddPart<int>(OnFrameUpdate, 5).AddFrame(5, 2.0f);
            var animationGuarding135 = core.Animations.Create("Animations/Turret/Guarding/135", 2.0f);
            animationGuarding135.AddPart<int>(OnFrameUpdate, 6).AddFrame(6, 2.0f);
            var animationGuarding157_5 = core.Animations.Create("Animations/Turret/Guarding/157.5", 2.0f);
            animationGuarding157_5.AddPart<int>(OnFrameUpdate, 7).AddFrame(7, 2.0f);
            var animationGuarding180 = core.Animations.Create("Animations/Turret/Guarding/180", 2.0f);
            animationGuarding180.AddPart<int>(OnFrameUpdate, 8).AddFrame(8, 2.0f);
            var animationGuarding202_5 = core.Animations.Create("Animations/Turret/Guarding/202.5", 2.0f);
            animationGuarding202_5.AddPart<int>(OnFrameUpdate, 9).AddFrame(9, 2.0f);
            var animationGuarding225 = core.Animations.Create("Animations/Guarding/Guard/225", 2.0f);
            animationGuarding225.AddPart<int>(OnFrameUpdate, 10).AddFrame(10, 2.0f);
            var animationGuarding247_5 = core.Animations.Create("Animations/Turret/Guarding/247.5", 2.0f);
            animationGuarding247_5.AddPart<int>(OnFrameUpdate, 11).AddFrame(11, 2.0f);
            var animationGuarding270 = core.Animations.Create("Animations/Turret/Guarding/270", 2.0f);
            animationGuarding270.AddPart<int>(OnFrameUpdate, 12).AddFrame(12, 2.0f);
            var animationGuarding292_5 = core.Animations.Create("Animations/Guarding/Guard/292.5", 2.0f);
            animationGuarding292_5.AddPart<int>(OnFrameUpdate, 13).AddFrame(13, 2.0f);
            var animationGuarding315 = core.Animations.Create("Animations/Turret/Guarding/315", 2.0f);
            animationGuarding315.AddPart<int>(OnFrameUpdate, 14).AddFrame(14, 2.0f);
            var animationGuarding337_5 = core.Animations.Create("Animations/Turret/Guarding/337.5", 2.0f);
            animationGuarding337_5.AddPart<int>(OnFrameUpdate, 15).AddFrame(15, 2.0f);
        }
        private static void OnStop()
        {
            Console.WriteLine("Rotation -> Stopped");
        }

        public static void CreateRotationFsm(ICore core)
        {
            var stateMachine = core.StateMachines.Create<RotationState, RotationImpulse>("Turret.Rotation");

            stateMachine.AddState(new Actor.States.Rotation.IdleState());
            stateMachine.AddState(new Actor.States.Rotation.RotatingState());

            stateMachine.AddTransition(RotationState.Rotating, RotationImpulse.Stop, RotationState.Idle);
            stateMachine.AddTransition(RotationState.Idle, RotationImpulse.Rotate, RotationState.Rotating);

        }

        private static void OnFrameUpdate(Entity entity, int nextValue)
        {
            //entity.Core.Commands.Post(new SpriteSetCommand(entity.Id, nextValue));
        }

        public static Entity Create(ICore core, Vector2 pos)
        {
            var entity = core.Entities.CreateFromTemplate("Turret");
            entity.Get<PositionComponent>().Value = pos;
            entity.Add(new CollisionComponent(ColliderTypes.StaticObstacle));
            //entity.Subscribe<CollisionEventArgs>(OnCollision);

            return entity;
        }


        #endregion Public Fields
    }
}